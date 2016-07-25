﻿#if !PORTABLE
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
#if NETCOREAPP1_0
using NUnit.Framework.Internal;
#endif

namespace Flurl.Test
{
	[TestFixture]
	public class PackageTests
	{
		[Test]
		public void flurl_versions_are_consistent() {
			var projVer = GetProjectVersion(@"src\Flurl\project.json");
			var nuspecVer = GetNuspecVersion(@"build\nuspec\Flurl.nuspec");
			Assert.AreEqual(projVer, nuspecVer);
		}

		[Test]
		public void flurlhttp_versions_are_consistent() {
			var projVer = GetProjectVersion(@"src\Flurl.Http\project.json");
			var nuspecVer = GetNuspecVersion(@"build\nuspec\Flurl.Http.nuspec");
			Assert.AreEqual(projVer, nuspecVer);
		}

		private string GetNuspecVersion(string pathFromSolutionRoot) {
			var path = GetFullPath(pathFromSolutionRoot);
			return XDocument.Load(path).Descendants("version").FirstOrDefault()?.Value;
		}

		private string GetProjectVersion(string pathFromSolutionRoot) {
			var path = GetFullPath(pathFromSolutionRoot);
			dynamic d = JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText(path));
			return d.version;
		}

		private string GetFullPath(string pathFromSolutionRoot) {
#if NET45
			return Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\..", pathFromSolutionRoot);
#elif NETCOREAPP1_0
			return Path.Combine(AssemblyHelper.GetAssemblyPath(GetType()), @"..\..\..\..\..\..\", pathFromSolutionRoot);
#endif
		}
	}
}
#endif