﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Flurl.Http.Configuration;

namespace Flurl.Http
{
	/// <summary>
	/// Encapsulates request, response, and other details associated with an HTTP call. Useful for diagnostics and available in
	/// global event handlers and FlurlHttpException.Call.
	/// </summary>
	public class HttpCall
	{
		internal HttpCall(HttpRequestMessage request, FlurlHttpSettings settings) {
			Request = request;
			Settings = settings;
		}

		/// <summary>
		/// FlurlHttpSettings used for this call.
		/// </summary>
		public FlurlHttpSettings Settings { get; private set; }

		/// <summary>
		/// HttpRequestMessage associated with this call.
		/// </summary>
		public HttpRequestMessage Request { get; private set; }

		/// <summary>
		/// Captured request body. More reliably available than HttpRequestMessage.Content, which is a forward-only, read-once stream.
		/// </summary>
		public string RequestBody { get; set; }

		/// <summary>
		/// HttpResponseMessage associated with the call if the call completed, otherwise null.
		/// </summary>
		public HttpResponseMessage Response { get; set; }

		/// <summary>
		/// Exception that occurred while sending the HttpRequestMessage.
		/// </summary>
		public Exception Exception { get; set; }
	
		/// <summary>
		/// User code should set this to true inside global event handlers (OnError, etc) to indicate
		/// that the exception was handled and should not be propagated further.
		/// </summary>
		public bool ExceptionHandled { get; set; }

		/// <summary>
		/// DateTime the moment the request was sent.
		/// </summary>
		public DateTime StartedUtc { get; set; }

		/// <summary>
		/// DateTime the moment a response was received.
		/// </summary>
		public DateTime? EndedUtc { get; set; }

		/// <summary>
		/// Total duration of the call if it completed, otherwise null.
		/// </summary>
		public TimeSpan? Duration {
			get { return EndedUtc - StartedUtc; }
		}

		/// <summary>
		/// Absolute URI being called.
		/// </summary>
		public string Url {
			get { return Request.RequestUri.AbsoluteUri; }
		}

		/// <summary>
		/// True if a response was received, regardless of whether it is an error status.
		/// </summary>
		public bool Completed {
			get { return Response != null; }
		}

		/// <summary>
		/// True if a response with a successful HTTP status was received.
		/// </summary>
		public bool Succeeded {
			get {
				return
					!Completed ? false :
					Response.IsSuccessStatusCode ? true :
					HttpStatusRangeParser.IsMatch(Settings.AllowedHttpStatusRange, Response.StatusCode);
			}
		}

		/// <summary>
		/// HttpStatusCode of the response if the call completed, otherwise null.
		/// </summary>
		public HttpStatusCode? HttpStatus {
			get { return Completed ? (HttpStatusCode?)Response.StatusCode : null; }
		}

		/// <summary>
		/// Body of the HTTP response if unsuccessful, otherwise null. (Successful responses are not captured as strings, mainly for performance reasons.)
		/// </summary>
		public string ErrorResponseBody { get; set; }
	}

	/// <summary>
	/// Flurl extensions to HttpRequestMessage
	/// </summary>
	public static class HttpRequestMessageExtensions
	{
		/// <summary>
		/// Gets the Flurl HttpCall that was set for the given request, or null if Flurl.Http was not used to make the request
		/// </summary>
		public static HttpCall GetFlurlHttpCall(this HttpRequestMessage request) {
			object obj;
			if (request != null && request.Properties != null && request.Properties.TryGetValue("FlurlHttpCall", out obj) && obj is HttpCall)
				return (HttpCall)obj;
			return null;
		}

		internal static void SetFlurlHttpCall(this HttpRequestMessage request, FlurlHttpSettings settings) {
			if (request?.Properties != null)
				request.Properties["FlurlHttpCall"] = new HttpCall(request, settings);
		}
	}
}
