﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net.Http;
#if NETSTANDARD1_4
using System.Text;
#endif
using System.Threading.Tasks;

namespace Flurl.Http
{
	/// <summary>
	/// Async extension methods that can be chained off Task&lt;HttpResponseMessage&gt;, avoiding nested awaits.
	/// </summary>
	public static class HttpResponseMessageExtensions
	{
		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to object of type T. Intended to chain off an async HTTP.
		/// </summary>
		/// <typeparam name="T">A type whose structure matches the expected JSON response.</typeparam>
		/// <returns>A Task whose result is an object containing data in the response body.</returns>
		/// <example>x = await url.PostAsync(data).ReceiveJson&lt;T&gt;()</example>
		/// <exception cref="FlurlHttpException">Condition.</exception>
		public static async Task<T> ReceiveJson<T>(this Task<HttpResponseMessage> response) {
			var resp = await response.ConfigureAwait(false);
			if (resp == null) return default(T);

			var call = HttpCall.Get(resp.RequestMessage);
			try {
				using (var stream = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false))
					return call.Settings.JsonSerializer.Deserialize<T>(stream);
			}
			catch (Exception ex) {
				call.Exception = ex;
				throw new FlurlHttpException(call, ex);
			}
		}

		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to a dynamic object. Intended to chain off an async call.
		/// </summary>
		/// <returns>A Task whose result is a dynamic object containing data in the response body.</returns>
		/// <example>d = await url.PostAsync(data).ReceiveJson()</example>
		/// <exception cref="FlurlHttpException">Condition.</exception>
		public static async Task<dynamic> ReceiveJson(this Task<HttpResponseMessage> response) {
			return await response.ReceiveJson<ExpandoObject>().ConfigureAwait(false);
		}

		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to a list of dynamic objects. Intended to chain off an async call.
		/// </summary>
		/// <returns>A Task whose result is a list of dynamic objects containing data in the response body.</returns>
		/// <example>d = await url.PostAsync(data).ReceiveJsonList()</example>
		/// <exception cref="FlurlHttpException">Condition.</exception>
		public static async Task<IList<dynamic>> ReceiveJsonList(this Task<HttpResponseMessage> response) {
			dynamic[] d = await response.ReceiveJson<ExpandoObject[]>().ConfigureAwait(false);
			return d;
		}

		/// <summary>
		/// Returns HTTP response body as a string. Intended to chain off an async call.
		/// </summary>
		/// <returns>A Task whose result is the response body as a string.</returns>
		/// <example>s = await url.PostAsync(data).ReceiveString()</example>
		public static async Task<string> ReceiveString(this Task<HttpResponseMessage> response) {
#if NETSTANDARD1_4
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
			var resp = await response.ConfigureAwait(false);
			if (resp == null) return null;

			return await resp.Content.StripCharsetQuotes().ReadAsStringAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Returns HTTP response body as a stream. Intended to chain off an async call.
		/// </summary>
		/// <returns>A Task whose result is the response body as a stream.</returns>
		/// <example>stream = await url.PostAsync(data).ReceiveStream()</example>
		public static async Task<Stream> ReceiveStream(this Task<HttpResponseMessage> response) {
			var resp = await response.ConfigureAwait(false);
			if (resp == null) return null;

			return await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Returns HTTP response body as a byte array. Intended to chain off an async call.
		/// </summary>
		/// <returns>A Task whose result is the response body as a byte array.</returns>
		/// <example>bytes = await url.PostAsync(data).ReceiveBytes()</example>
		public static async Task<byte[]> ReceiveBytes(this Task<HttpResponseMessage> response) {
			var resp = await response.ConfigureAwait(false);
			if (resp == null) return null;

			return await resp.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
		}

		// https://github.com/tmenier/Flurl/pull/76, https://github.com/dotnet/corefx/issues/5014
		internal static HttpContent StripCharsetQuotes(this HttpContent content) {
			var header = content?.Headers?.ContentType;
			if (header?.CharSet != null)
				header.CharSet = header.CharSet.Trim().TrimStart('"').TrimEnd('"');
			return content;
		}
	}
}