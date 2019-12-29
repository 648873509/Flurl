﻿using System;
using System.Net;
using System.Net.Http;
using Flurl.Http.Content;

namespace Flurl.Http
{
	/// <summary>
	/// Encapsulates request, response, and other details associated with an HTTP call. Useful for diagnostics and available in
	/// global event handlers and FlurlHttpException.Call.
	/// </summary>
	public class FlurlCall
	{
		/// <summary>
		/// The IFlurlRequest associated with this call.
		/// </summary>
		public IFlurlRequest Request { get; set; }

		/// <summary>
		/// The raw HttpRequestMessage associated with this call.
		/// </summary>
		public HttpRequestMessage HttpRequestMessage { get; set; }

		/// <summary>
		/// Captured request body. Available ONLY if HttpRequestMessage.Content is a Flurl.Http.Content.CapturedStringContent.
		/// </summary>
		public string RequestBody => (HttpRequestMessage.Content as CapturedStringContent)?.Content;

		/// <summary>
		/// The IFlurlResponse associated with this call if the call completed, otherwise null.
		/// </summary>
		public IFlurlResponse Response { get; set; }

		/// <summary>
		/// The raw HttpResponseMessage associated with the call if the call completed, otherwise null.
		/// </summary>
		public HttpResponseMessage HttpResponseMessage { get; set; }

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
		public TimeSpan? Duration => EndedUtc - StartedUtc;

		/// <summary>
		/// True if a response was received, regardless of whether it is an error status.
		/// </summary>
		public bool Completed => HttpResponseMessage != null;

		/// <summary>
		/// True if a response with a successful HTTP status was received.
		/// </summary>
		public bool Succeeded => Completed && 
			(HttpResponseMessage.IsSuccessStatusCode || HttpStatusRangeParser.IsMatch(Request.Settings.AllowedHttpStatusRange, HttpResponseMessage.StatusCode));

		/// <summary>
		/// Returns the verb and absolute URI associated with this call.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return $"{HttpRequestMessage.Method:U} {Request.Url}";
		}
	}
}
