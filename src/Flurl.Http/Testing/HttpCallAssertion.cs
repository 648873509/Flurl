﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Flurl.Util;

namespace Flurl.Http.Testing
{
	/// <summary>
	/// Provides fluent helpers for asserting against fake HTTP calls. Usually created fluently
	/// via HttpTest.ShouldHaveCalled or HttpTest.ShouldNotHaveCalled, rather than instantiated directly.
	/// </summary>
	public class HttpCallAssertion
	{
		private readonly bool _negate;
		private readonly IList<string> _expectedConditions = new List<string>();

		private IList<HttpCall> _calls;

		/// <summary>
		/// Constructs a new instance of HttpCallAssertion.
		/// </summary>
		/// <param name="loggedCalls">Set of calls (usually from HttpTest.CallLog) to assert against.</param>
		/// <param name="negate">If true, assertions pass when calls matching criteria were NOT made.</param>
		public HttpCallAssertion(IEnumerable<HttpCall> loggedCalls, bool negate = false) {
			_calls = loggedCalls.ToList();
			_negate = negate;
		}

	    /// <summary>
	    /// Assert whether calls matching specified criteria were made a specific number of times. (When not specified,
	    /// assertions verify whether any calls matching criteria were made.)
	    /// </summary>
	    /// <param name="expectedCount">Exact number of expected calls</param>
	    /// <exception cref="ArgumentException"><paramref name="expectedCount"/> must be greater than or equal to 0.</exception>
	    public void Times(int expectedCount) {
			if (expectedCount < 0)
				throw new ArgumentException("expectedCount must be greater than or equal to 0.");

			Assert(expectedCount);
		}

	    /// <summary>
	    /// Asserts whether calls were made matching the given predicate function.
	    /// </summary>
	    /// <param name="match">Predicate (usually a lambda expression) that tests an HttpCall and returns a bool.</param>
	    /// <param name="descrip">A description of what is being asserted.</param>
	    public HttpCallAssertion With(Func<HttpCall, bool> match, string descrip = null) {
		    if (!string.IsNullOrEmpty(descrip))
			    _expectedConditions.Add(descrip);
		    _calls = _calls.Where(match).ToList();
		    Assert();
		    return this;
	    }

	    /// <summary>
	    /// Asserts whether calls were made that do NOT match the given predicate function.
	    /// </summary>
	    /// <param name="match">Predicate (usually a lambda expression) that tests an HttpCall and returns a bool.</param>
	    /// <param name="descrip">A description of what is being asserted.</param>
	    public HttpCallAssertion Without(Func<HttpCall, bool> match, string descrip = null) {
		    return With(c => !match(c), descrip);
	    }

		/// <summary>
		/// Asserts whether calls were made matching given URL or URL pattern.
		/// </summary>
		/// <param name="urlPattern">Can contain * wildcard.</param>
		public HttpCallAssertion WithUrlPattern(string urlPattern) {
			return With(c => Util.MatchesPattern(c.FlurlRequest.Url, urlPattern), $"URL pattern {urlPattern}");
		}

		/// <summary>
		/// Asserts whether calls were made with any of the given HTTP verbs.
		/// </summary>
		public HttpCallAssertion WithVerb(params HttpMethod[] verbs) {
			var list = string.Join(", ", verbs.Select(v => v.Method));
			return With(call => call.HasAnyVerb(verbs), $"verb {list}");
		}

		/// <summary>
		/// Asserts whether calls were made with any of the given HTTP verbs.
		/// </summary>
		public HttpCallAssertion WithVerb(params string[] verbs) {
			var list = string.Join(", ", verbs);
			return With(call => call.HasAnyVerb(verbs), $"verb {list}");
		}

		/// <summary>
		/// Asserts whether calls were made containing the given query parameter name and (optionally) value.
		/// </summary>
		public HttpCallAssertion WithQueryParam(string name, object value = null) {
			var qp = (value == null) ? name : $"{name}={value}";
			return With(c => c.HasQueryParam(name, value), $"query parameter {qp}");
		}

		/// <summary>
		/// Asserts whether calls were made NOT containing the given query parameter and (optionally) value.
		/// </summary>
		public HttpCallAssertion WithoutQueryParam(string name, object value = null) {
			var qp = (value == null) ? name : $"{name}={value}";
			return Without(c => c.HasQueryParam(name, value), $"no query parameter {qp}");
		}

		/// <summary>
		/// Asserts whether calls were made containing ALL the given query parameters (regardless of their values).
		/// </summary>
		public HttpCallAssertion WithQueryParams(params string[] names) {
			return names.Select(n => WithQueryParam(n)).LastOrDefault() ?? this;
		}

		/// <summary>
		/// Asserts whether calls were made NOT containing any of the given query parameters.
		/// If no names are provided, asserts no calls were made with any query parameters.
		/// </summary>
		public HttpCallAssertion WithoutQueryParams(params string[] names) {
			if (!names.Any())
				return With(c => !c.FlurlRequest.Url.QueryParams.Any(), "no query parameters");
			return names.Select(n => WithoutQueryParam(n)).LastOrDefault() ?? this;
		}

		/// <summary>
		/// Asserts whether calls were made containing ANY the given query parameters (regardless of their values).
		/// If no names are provided, asserts that calls were made containing at least one query parameter with any name.
		/// </summary>
		public HttpCallAssertion WithAnyQueryParam(params string[] names) {
			var descrip = names.Any() ? $"any of query parameters {string.Join(", ", names)}" : "any query parameters";
			return With(c => c.HasAnyQueryParam(names), descrip);
		}

		/// <summary>
		/// Asserts whether calls were made containing all of the given query parameter values.
		/// </summary>
		/// <param name="values">Object (usually anonymous) or dictionary that is parsed to name/value query parameters to check for. Values may contain * wildcard.</param>
		public HttpCallAssertion WithQueryParams(object values) {
			return values.ToKeyValuePairs().Select(kv => WithQueryParam(kv.Key, kv.Value)).LastOrDefault() ?? this;
		}

		/// <summary>
		/// Asserts whether calls were made NOT containing any of the given query parameter values.
		/// </summary>
		/// <param name="values">Object (usually anonymous) or dictionary that is parsed to name/value query parameters to check for. Values may contain * wildcard.</param>
		public HttpCallAssertion WithoutQueryParams(object values) {
			return values.ToKeyValuePairs().Select(kv => WithoutQueryParam(kv.Key, kv.Value)).LastOrDefault() ?? this;
		}

		/// <summary>
		/// Asserts whether calls were made containing given request body. body may contain * wildcard.
		/// </summary>
		public HttpCallAssertion WithRequestBody(string bodyPattern) {
			return With(c => Util.MatchesPattern(c.RequestBody, bodyPattern), $"body matching pattern {bodyPattern}");
		}

		/// <summary>
		/// Asserts whether calls were made containing given JSON-encoded request body. body may contain * wildcard.
		/// </summary>
		public HttpCallAssertion WithRequestJson(object body) {
			var serializedBody = FlurlHttp.GlobalSettings.JsonSerializer.Serialize(body);
			return WithRequestBody(serializedBody);
		}

		/// <summary>
		/// Asserts whether calls were made containing given URL-encoded request body. body may contain * wildcard.
		/// </summary>
		public HttpCallAssertion WithRequestUrlEncoded(object body) {
			var serializedBody = FlurlHttp.GlobalSettings.UrlEncodedSerializer.Serialize(body);
			return WithRequestBody(serializedBody);
		}

		/// <summary>
		/// Asserts whether calls were made containing the given request header and (optionally) value.
		/// value may contain * wildcard.
		/// </summary>
		public HttpCallAssertion WithHeader(string name, string valuePattern = "*", string descrip = null) {
			descrip = descrip ?? $"header {name}: {valuePattern}";
			return With(c => c.HasHeader(name, valuePattern), descrip);
		}

		/// <summary>
		/// Asserts whether calls were made that do NOT contain the given request header and (optionally) value.
		/// value may contain * wildcard.
		/// </summary>
		public HttpCallAssertion WithoutHeader(string name, string valuePattern = "*") {
			return Without(c => c.HasHeader(name, valuePattern), $"no header {name}: {valuePattern}");
		}

		/// <summary>
		/// Asserts whether calls were made with a request body of the given content (MIME) type.
		/// </summary>
		public HttpCallAssertion WithContentType(string mediaType) {
			return WithHeader("Content-Type", mediaType, "content type " + mediaType);
		}

		/// <summary>
		/// Asserts whether an Authorization header was set with the given Bearer token, or any Bearer token if excluded.
		/// Token can contain * wildcard.
		/// </summary>
		public HttpCallAssertion WithOAuthBearerToken(string token = "*") {
			return WithHeader("Authorization", $"Bearer {token}", "bearer token " + token);
		}

		/// <summary>
		/// Asserts whether the Authorization header was set with Basic auth and (optionally) the given credentials.
		/// Username and password can contain * wildcard.
		/// </summary>
		public HttpCallAssertion WithBasicAuth(string username = "*", string password = "*") {
			return With(call => {
				var val = call.Request.GetHeaderValue("Authorization");
				if (val == null) return false;
				if (!val.StartsWith("Basic ")) return false;
				if ((username ?? "*") == "*" && (password ?? "*") == "*") return true;
				var encodedCreds = val.Substring(6);
				try {
					var bytes = Convert.FromBase64String(encodedCreds);
					var creds = Encoding.UTF8.GetString(bytes, 0, bytes.Length).SplitOnFirstOccurence(":");
					return creds.Length == 2 && Util.MatchesPattern(creds[0], username) && Util.MatchesPattern(creds[1], password);
				}
				catch (FormatException) {
					return false;
				}
			});
		}

		private void Assert(int? count = null) {
			var pass = count.HasValue ? (_calls.Count == count.Value) : _calls.Any();
			if (_negate) pass = !pass;

			if (!pass)
				throw new HttpTestException(_expectedConditions, count, _calls.Count);
		}
	}
}
