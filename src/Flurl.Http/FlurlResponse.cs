﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Configuration;

namespace Flurl.Http
{
	/// <summary>
	/// Represents an HTTP response.
	/// </summary>
	public interface IFlurlResponse : IDisposable
	{
		/// <summary>
		/// Gets the collection of response headers received.
		/// </summary>
		IDictionary<string, string> Headers { get; }

		/// <summary>
		/// Gets the collection of HttpCookies received from the server.
		/// </summary>
		IDictionary<string, Cookie> Cookies { get; }

		/// <summary>
		/// Gets the raw HttpResponseMessage that this IFlurlResponse wraps.
		/// </summary>
		HttpResponseMessage ResponseMessage { get; }

		/// <summary>
		/// Gets the status code of the response.
		/// </summary>
		int StatusCode { get; }

		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to object of type T.
		/// </summary>
		/// <typeparam name="T">A type whose structure matches the expected JSON response.</typeparam>
		/// <returns>A Task whose result is an object containing data in the response body.</returns>
		/// <example>x = await url.PostAsync(data).GetJson&lt;T&gt;()</example>
		/// <exception cref="FlurlHttpException">Condition.</exception>
		Task<T> GetJsonAsync<T>();

		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to a dynamic object.
		/// </summary>
		/// <returns>A Task whose result is a dynamic object containing data in the response body.</returns>
		/// <example>d = await url.PostAsync(data).GetJson()</example>
		/// <exception cref="FlurlHttpException">Condition.</exception>
		Task<dynamic> GetJsonAsync();

		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to a list of dynamic objects.
		/// </summary>
		/// <returns>A Task whose result is a list of dynamic objects containing data in the response body.</returns>
		/// <example>d = await url.PostAsync(data).GetJsonList()</example>
		/// <exception cref="FlurlHttpException">Condition.</exception>
		Task<IList<dynamic>> GetJsonListAsync();

		/// <summary>
		/// Returns HTTP response body as a string.
		/// </summary>
		/// <returns>A Task whose result is the response body as a string.</returns>
		/// <example>s = await url.PostAsync(data).GetString()</example>
		Task<string> GetStringAsync();

		/// <summary>
		/// Returns HTTP response body as a stream.
		/// </summary>
		/// <returns>A Task whose result is the response body as a stream.</returns>
		/// <example>stream = await url.PostAsync(data).GetStream()</example>
		Task<Stream> GetStreamAsync();

		/// <summary>
		/// Returns HTTP response body as a byte array.
		/// </summary>
		/// <returns>A Task whose result is the response body as a byte array.</returns>
		/// <example>bytes = await url.PostAsync(data).GetBytes()</example>
		Task<byte[]> GetBytesAsync();
	}

	/// <inheritdoc />
	public class FlurlResponse : IFlurlResponse
	{
		private readonly Lazy<IDictionary<string, string>> _headers;
		private object _capturedBody = null;
		private bool _streamRead = false;
		private ISerializer _serializer = null;

		/// <inheritdoc />
		public IDictionary<string, string> Headers => _headers.Value;

		/// <inheritdoc />
		public IDictionary<string, Cookie> Cookies { get; } = new Dictionary<string, Cookie>();

		/// <inheritdoc />
		public HttpResponseMessage ResponseMessage { get; }

		/// <inheritdoc />
		public int StatusCode => (int)ResponseMessage.StatusCode;

		/// <summary>
		/// Creates a new FlurlResponse that wraps the give HttpResponseMessage.
		/// </summary>
		/// <param name="resp"></param>
		public FlurlResponse(HttpResponseMessage resp) {
			ResponseMessage = resp;
			_headers = new Lazy<IDictionary<string, string>>(BuildHeaders);
		}

		private IDictionary<string, string> BuildHeaders() => ResponseMessage.Headers
			.Concat(ResponseMessage.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
			.GroupBy(h => h.Key)
			.ToDictionary(g => g.Key, g => string.Join(", ", g.SelectMany(h => h.Value)));

		/// <inheritdoc />
		public async Task<T> GetJsonAsync<T>() {
			if (_streamRead)
				return _capturedBody is T body ? body : default(T);

			var call = ResponseMessage.RequestMessage.GetHttpCall();
			_serializer = call.Request.Settings.JsonSerializer;
			using (var stream = await ResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false)) {
				try {
					_capturedBody = _serializer.Deserialize<T>(stream);
					_streamRead = true;
					return (T)_capturedBody;
				}
				catch (Exception ex) {
					_serializer = null;
					_capturedBody = await ResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
					_streamRead = true;
					call.Exception = new FlurlParsingException(call, "JSON", ex);
					await FlurlRequest.HandleExceptionAsync(call, call.Exception, CancellationToken.None).ConfigureAwait(false);
					return default(T);
				}
			}
		}

		/// <inheritdoc />
		public async Task<dynamic> GetJsonAsync() {
			dynamic d = await GetJsonAsync<ExpandoObject>().ConfigureAwait(false);
			return d;
		}

		/// <inheritdoc />
		public async Task<IList<dynamic>> GetJsonListAsync() {
			dynamic[] d = await GetJsonAsync<ExpandoObject[]>().ConfigureAwait(false);
			return d;
		}

		/// <inheritdoc />
		public async Task<string> GetStringAsync() {
			if (_streamRead) {
				return
					(_capturedBody == null) ? null :
					// if GetJsonAsync<T> was called, we streamed the response directly to a T (for memory efficiency)
					// without first capturing a string. it's too late to get it, so the best we can do is serialize the T
					(_serializer != null) ? _serializer.Serialize(_capturedBody) :
					_capturedBody?.ToString();
			}

#if NETSTANDARD1_3 || NETSTANDARD2_0
			// https://stackoverflow.com/questions/46119872/encoding-issues-with-net-core-2 (#86)
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
			_capturedBody = await ResponseMessage.Content.StripCharsetQuotes().ReadAsStringAsync();
			_streamRead = true;
			return (string)_capturedBody;
		}

		/// <inheritdoc />
		public Task<Stream> GetStreamAsync() {
			_streamRead = true;
			return ResponseMessage.Content.ReadAsStreamAsync();
		}

		/// <inheritdoc />
		public async Task<byte[]> GetBytesAsync() {
			if (_streamRead)
				return _capturedBody as byte[];

			_capturedBody = await ResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			_streamRead = true;
			return (byte[])_capturedBody;
		}

		/// <summary>
		/// Disposes the underlying HttpResponseMessage.
		/// </summary>
		public void Dispose() => ResponseMessage.Dispose();
	}
}
