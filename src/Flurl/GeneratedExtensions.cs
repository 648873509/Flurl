// This file was auto-generated by Flurl.Http.CodeGen. Do not edit directly.
using System;
using System.Collections.Generic;

namespace Flurl
{
	/// <summary>
	/// Fluent URL-building extension methods on String and Uri.
	/// </summary>
	public static class GeneratedExtensions
	{
		/// <summary>
		/// Creates a new Url object from the string and appends a segment to the URL path, ensuring there is one and only one '/' character as a separator.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="segment">The segment to append</param>
		/// <param name="fullyEncode">If true, URL-encodes reserved characters such as '/', '+', and '%'. Otherwise, only encodes strictly illegal characters (including '%' but only when not followed by 2 hex characters).</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url AppendPathSegment(this string url, object segment, bool fullyEncode = false) {
			return new Url(url).AppendPathSegment(segment, fullyEncode);
		}
		
		/// <summary>
		/// Appends multiple segments to the URL path, ensuring there is one and only one '/' character as a separator.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="segments">The segments to append</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url AppendPathSegments(this string url, params object[] segments) {
			return new Url(url).AppendPathSegments(segments);
		}
		
		/// <summary>
		/// Appends multiple segments to the URL path, ensuring there is one and only one '/' character as a separator.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="segments">The segments to append</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url AppendPathSegments(this string url, IEnumerable<object> segments) {
			return new Url(url).AppendPathSegments(segments);
		}
		
		/// <summary>
		/// Removes the last path segment from the URL.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemovePathSegment(this string url) {
			return new Url(url).RemovePathSegment();
		}
		
		/// <summary>
		/// Removes the entire path component of the URL.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemovePath(this string url) {
			return new Url(url).RemovePath();
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds a parameter to the query, overwriting the value if name exists.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="name">Name of query parameter</param>
		/// <param name="value">Value of query parameter</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParam(this string url, string name, object value, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			return new Url(url).SetQueryParam(name, value, nullValueHandling);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds a parameter to the query, overwriting the value if name exists.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="name">Name of query parameter</param>
		/// <param name="value">Value of query parameter</param>
		/// <param name="isEncoded">Set to true to indicate the value is already URL-encoded. Defaults to false.</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing).</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParam(this string url, string name, string value, bool isEncoded = false, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			return new Url(url).SetQueryParam(name, value, isEncoded, nullValueHandling);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds a parameter without a value to the query, removing any existing value.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="name">Name of query parameter</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParam(this string url, string name) {
			return new Url(url).SetQueryParam(name);
		}
		
		/// <summary>
		/// Creates a new Url object from the string, parses values object into name/value pairs, and adds them to the query, overwriting any that already exist.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="values">Typically an anonymous object, ie: new { x = 1, y = 2 }</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParams(this string url, object values, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			return new Url(url).SetQueryParams(values, nullValueHandling);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds multiple parameters without values to the query.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="names">Names of query parameters.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParams(this string url, IEnumerable<string> names) {
			return new Url(url).SetQueryParams(names);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds multiple parameters without values to the query.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="names">Names of query parameters</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParams(this string url, params string[] names) {
			return new Url(url).SetQueryParams(names);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and removes a name/value pair from the query by name.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="name">Query string parameter name to remove</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveQueryParam(this string url, string name) {
			return new Url(url).RemoveQueryParam(name);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and removes multiple name/value pairs from the query by name.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="names">Query string parameter names to remove</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveQueryParams(this string url, params string[] names) {
			return new Url(url).RemoveQueryParams(names);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and removes multiple name/value pairs from the query by name.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="names">Query string parameter names to remove</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveQueryParams(this string url, IEnumerable<string> names) {
			return new Url(url).RemoveQueryParams(names);
		}
		
		/// <summary>
		/// Removes the entire query component of the URL.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveQuery(this string url) {
			return new Url(url).RemoveQuery();
		}
		
		/// <summary>
		/// Set the URL fragment fluently.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <param name="fragment">The part of the URL after #</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetFragment(this string url, string fragment) {
			return new Url(url).SetFragment(fragment);
		}
		
		/// <summary>
		/// Removes the URL fragment including the #.
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveFragment(this string url) {
			return new Url(url).RemoveFragment();
		}
		
		/// <summary>
		/// Trims the URL to its root, including the scheme, any user info, host, and port (if specified).
		/// </summary>
		/// <param name="url">This URL.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url ResetToRoot(this string url) {
			return new Url(url).ResetToRoot();
		}
		
		/// <summary>
		/// Creates a new Url object from the string and appends a segment to the URL path, ensuring there is one and only one '/' character as a separator.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="segment">The segment to append</param>
		/// <param name="fullyEncode">If true, URL-encodes reserved characters such as '/', '+', and '%'. Otherwise, only encodes strictly illegal characters (including '%' but only when not followed by 2 hex characters).</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url AppendPathSegment(this Uri uri, object segment, bool fullyEncode = false) {
			return new Url(uri).AppendPathSegment(segment, fullyEncode);
		}
		
		/// <summary>
		/// Appends multiple segments to the URL path, ensuring there is one and only one '/' character as a separator.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="segments">The segments to append</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url AppendPathSegments(this Uri uri, params object[] segments) {
			return new Url(uri).AppendPathSegments(segments);
		}
		
		/// <summary>
		/// Appends multiple segments to the URL path, ensuring there is one and only one '/' character as a separator.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="segments">The segments to append</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url AppendPathSegments(this Uri uri, IEnumerable<object> segments) {
			return new Url(uri).AppendPathSegments(segments);
		}
		
		/// <summary>
		/// Removes the last path segment from the URL.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemovePathSegment(this Uri uri) {
			return new Url(uri).RemovePathSegment();
		}
		
		/// <summary>
		/// Removes the entire path component of the URL.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemovePath(this Uri uri) {
			return new Url(uri).RemovePath();
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds a parameter to the query, overwriting the value if name exists.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="name">Name of query parameter</param>
		/// <param name="value">Value of query parameter</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParam(this Uri uri, string name, object value, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			return new Url(uri).SetQueryParam(name, value, nullValueHandling);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds a parameter to the query, overwriting the value if name exists.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="name">Name of query parameter</param>
		/// <param name="value">Value of query parameter</param>
		/// <param name="isEncoded">Set to true to indicate the value is already URL-encoded. Defaults to false.</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing).</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParam(this Uri uri, string name, string value, bool isEncoded = false, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			return new Url(uri).SetQueryParam(name, value, isEncoded, nullValueHandling);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds a parameter without a value to the query, removing any existing value.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="name">Name of query parameter</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParam(this Uri uri, string name) {
			return new Url(uri).SetQueryParam(name);
		}
		
		/// <summary>
		/// Creates a new Url object from the string, parses values object into name/value pairs, and adds them to the query, overwriting any that already exist.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="values">Typically an anonymous object, ie: new { x = 1, y = 2 }</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParams(this Uri uri, object values, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			return new Url(uri).SetQueryParams(values, nullValueHandling);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds multiple parameters without values to the query.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="names">Names of query parameters.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParams(this Uri uri, IEnumerable<string> names) {
			return new Url(uri).SetQueryParams(names);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and adds multiple parameters without values to the query.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="names">Names of query parameters</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetQueryParams(this Uri uri, params string[] names) {
			return new Url(uri).SetQueryParams(names);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and removes a name/value pair from the query by name.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="name">Query string parameter name to remove</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveQueryParam(this Uri uri, string name) {
			return new Url(uri).RemoveQueryParam(name);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and removes multiple name/value pairs from the query by name.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="names">Query string parameter names to remove</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveQueryParams(this Uri uri, params string[] names) {
			return new Url(uri).RemoveQueryParams(names);
		}
		
		/// <summary>
		/// Creates a new Url object from the string and removes multiple name/value pairs from the query by name.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="names">Query string parameter names to remove</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveQueryParams(this Uri uri, IEnumerable<string> names) {
			return new Url(uri).RemoveQueryParams(names);
		}
		
		/// <summary>
		/// Removes the entire query component of the URL.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveQuery(this Uri uri) {
			return new Url(uri).RemoveQuery();
		}
		
		/// <summary>
		/// Set the URL fragment fluently.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <param name="fragment">The part of the URL after #</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url SetFragment(this Uri uri, string fragment) {
			return new Url(uri).SetFragment(fragment);
		}
		
		/// <summary>
		/// Removes the URL fragment including the #.
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url RemoveFragment(this Uri uri) {
			return new Url(uri).RemoveFragment();
		}
		
		/// <summary>
		/// Trims the URL to its root, including the scheme, any user info, host, and port (if specified).
		/// </summary>
		/// <param name="uri">This System.Uri.</param>
		/// <returns>A new Flurl.Url object.</returns>
		public static Url ResetToRoot(this Uri uri) {
			return new Url(uri).ResetToRoot();
		}
		
	}
}
