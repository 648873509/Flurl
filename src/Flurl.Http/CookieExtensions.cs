﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Flurl.Util;

namespace Flurl.Http
{
	/// <summary>
	/// Fluent extension methods for working with HTTP cookies.
	/// </summary>
	public static class CookieExtensions
	{
		/// <summary>
		/// Allows cookies to be sent and received. Not necessary to call when setting cookies via WithCookie/WithCookies.
		/// </summary>
		/// <param name="clientOrRequest">The IFlurlClient or IFlurlRequest.</param>
		/// <returns>This IFlurlClient.</returns>
		public static T EnableCookies<T>(this T clientOrRequest) where T : IHttpSettingsContainer {
			clientOrRequest.Settings.CookiesEnabled = true;
			return clientOrRequest;
		}

		/// <summary>
		/// Sets an HTTP cookie to be sent with this IFlurlRequest or all requests made with this IFlurlClient.
		/// </summary>
		/// <param name="clientOrRequest">The IFlurlClient or IFlurlRequest.</param>
		/// <param name="cookie">The cookie to set.</param>
		/// <returns>This IFlurlClient.</returns>
		public static T WithCookie<T>(this T clientOrRequest, Cookie cookie) where T : IHttpSettingsContainer {
			clientOrRequest.Settings.CookiesEnabled = true;
			clientOrRequest.Cookies[cookie.Name] = cookie;
			return clientOrRequest;
		}

		/// <summary>
		/// Sets an HTTP cookie to be sent with this IFlurlRequest or all requests made with this IFlurlClient.
		/// </summary>
		/// <param name="clientOrRequest">The IFlurlClient or IFlurlRequest.</param>
		/// <param name="name">The cookie name.</param>
		/// <param name="value">The cookie value.</param>
		/// <param name="expires">The cookie expiration (optional). If excluded, cookie only lives for duration of session.</param>
		/// <returns>This IFlurlClient.</returns>
		public static T WithCookie<T>(this T clientOrRequest, string name, object value, DateTime? expires = null) where T : IHttpSettingsContainer {
			var cookie = new Cookie(name, value?.ToInvariantString()) { Expires = expires ?? DateTime.MinValue };
			return clientOrRequest.WithCookie(cookie);
		}

		/// <summary>
		/// Sets HTTP cookies to be sent with this IFlurlRequest or all requests made with this IFlurlClient, based on property names/values of the provided object, or keys/values if object is a dictionary.
		/// </summary>
		/// <param name="clientOrRequest">The IFlurlClient or IFlurlRequest.</param>
		/// <param name="cookies">Names/values of HTTP cookies to set. Typically an anonymous object or IDictionary.</param>
		/// <param name="expires">Expiration for all cookies (optional). If excluded, cookies only live for duration of session.</param>
		/// <returns>This IFlurlClient.</returns>
		public static T WithCookies<T>(this T clientOrRequest, object cookies, DateTime? expires = null) where T : IHttpSettingsContainer {
			if (cookies == null)
				return clientOrRequest;

			foreach (var kv in cookies.ToKeyValuePairs()) {
				if (kv.Value is Cookie cookie)
					clientOrRequest.WithCookie(cookie);
				else
					clientOrRequest.WithCookie(kv.Key, kv.Value, expires);
			}

			return clientOrRequest;
		}

		/// <summary>
		/// Sets a collection of HTTP cookies that will be sent with the request. May be modified when the response is received, if the server returns any cookies.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cookies">The cookies to send.</param>
		/// <returns></returns>
		public static IFlurlRequest WithCookies(this IFlurlRequest request, IDictionary<string, Cookie> cookies) {
			request.Cookies = cookies;
			return request;
		}

		/// <summary>
		/// Provides access to the collection that will receive HTTP cookies from the server, which can then be sent in subsequent requests.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cookies">The cookie collection.</param>
		/// <returns></returns>
		public static IFlurlRequest WithCookies(this IFlurlRequest request, out IDictionary<string, Cookie> cookies) {
			cookies = request.Cookies;
			return request;
		}

		/// <summary>
		/// Creates a new CookieSession, under which all requests and responses share a cookie collection.
		/// </summary>
		public static CookieSession StartCookieSession(this IFlurlClient client) => new CookieSession(client);
	}
}
