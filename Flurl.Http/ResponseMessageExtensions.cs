﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Flurl.Http
{
	/// <summary>
	/// Async extension methods that can be chained off Task&lt;HttpResponseMessage&gt;, avoiding nested await.
	/// Example: var data = await "http://api.com".PostJsonAsync(...).ReceiveJsonAsync();
	/// </summary>
	public static class ResponseMessageExtensions
	{
		/// <summary>
		/// Deserializes JSON-formatted response body to object of type T. Intended to chain off an async HTTP call.
		/// </summary>
 		/// <typeparam name="T">A type whose structure matches the expected JSON response.</typeparam>
		/// <returns>A Task whose result is an object containing data in the response body.</returns>
		public static async Task<T> ReceiveJsonAsync<T>(this Task<HttpResponseMessage> response) {
			using (var stream = await (await response).Content.ReadAsStreamAsync())
				return JsonHelper.ReadJsonFromStream<T>(stream);
		}

		/// <summary>
		/// Deserializes JSON-formatted response body to a dynamic object. Intended to chain off an async HTTP call.
		/// </summary>
		/// <returns>A Task whose result is a dynamic object containing data in the response body.</returns>
		public static async Task<dynamic> ReceiveJsonAsync(this Task<HttpResponseMessage> response) {
			return await response.ReceiveJsonAsync<ExpandoObject>();
		}

		/// <summary>
		/// Deserializes JSON-formatted response body to a list of dynamic objects. Intended to chain off an async HTTP call.
		/// </summary>
		/// <returns>A Task whose result is a list of dynamic objects containing data in the response body.</returns>
		public static async Task<IList<dynamic>> ReceiveJsonListAsync(this Task<HttpResponseMessage> response) {
			dynamic[] d = await response.ReceiveJsonAsync<ExpandoObject[]>();
			return d;
		}

		/// <summary>
		/// Returns response body as a string. Intended to chain off an async HTTP call.
		/// </summary>
		/// <returns>A Task whose result is the response body.</returns>
		public static async Task<string> ReceiveStringAsync(this Task<HttpResponseMessage> response) {
			return await (await response).Content.ReadAsStringAsync();
		}
	}
}
