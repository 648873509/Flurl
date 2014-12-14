﻿using System.Net.Http;
using System.Threading.Tasks;

namespace Flurl.Http
{
	public static class HeadExtensions
	{
		/// <summary>
		/// Sends an asynchronous HEAD request.
		/// </summary>
		/// <returns>A Task whose result is the received HttpResponseMessage.</returns>
		public static Task<HttpResponseMessage> HeadAsync(this FlurlClient client)
		{
			var message = new HttpRequestMessage(HttpMethod.Head, client.Url);
			return client.DoCallAsync(http => http.SendAsync(message));
		}

		/// <summary>
		/// Creates a FlurlClient from the URL and sends an asynchronous HEAD request.
		/// </summary>
		/// <returns>A Task whose result is the received HttpResponseMessage.</returns>
		public static Task<HttpResponseMessage> HeadAsync(this string url)
		{
			return new FlurlClient(url, true).HeadAsync();
		}

		/// <summary>
		/// Creates a FlurlClient from the URL and sends an asynchronous HEAD request.
		/// </summary>
		/// <returns>A Task whose result is the received HttpResponseMessage.</returns>
		public static Task<HttpResponseMessage> HeadAsync(this Url url)
		{
			return new FlurlClient(url, true).HeadAsync();
		}
	}
}
