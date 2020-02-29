using FluentAssertions;
using NUnit.Framework;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration;
using RuntimeTestDataCollector.ObjectInitializationGeneration.CodeGeneration.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpStackToCSharpCodeTests.ObjectInitializationGeneration
{
    [TestFixture]
    class GenerateUriTypeTests
    {
        private CodeGeneratorManager _codeGeneratorManager;

        [SetUp]
        public void Setup()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);
            var readOnlyTypeConstructorDefinition = new Dictionary<string, IReadOnlyList<string>>
            {
                ["ExpressionData"] = new List<string> { "type", "value", "name", "underlyingExpressionData", "typeWithNamespace" }

            };
            _codeGeneratorManager = CodeGeneratorManagerFactory.Create(readOnlyTypeConstructorDefinition);
        }

        [Test]
        public void ShouldGenerate_TimeSpan()
        {
            var stackObject = GetUriDefinition();

            var generated = _codeGeneratorManager.GenerateStackDump(stackObject);

            generated.Should().Be("var uri = new Uri(\"localhost:80/test/test\");\n");
        }

        private ExpressionData GetUriDefinition()
        {
            return new ExpressionData("Uri", "{localhost:80/test/test}", "uri", new List<ExpressionData>()
            {
                new ExpressionData("string", "80/test/test", "AbsolutePath", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "localhost:80/test/test", "AbsoluteUri", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("bool", "false", "AllowIdn", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("string", "", "Authority", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "", "DnsSafeHost", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "", "Fragment", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("bool", "false", "HasAuthority", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("string", "", "Host", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("UriHostNameType", "Unknown", "HostNameType", new List<ExpressionData>()
                {
                }, "System.UriHostNameType"),
                new ExpressionData("Flags", "HostTypeMask", "HostType", new List<ExpressionData>()
                {
                }, "System.Uri.Flags"),
                new ExpressionData("string", "", "IdnHost", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("bool", "true", "IsAbsoluteUri", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "true", "IsDefaultPort", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "IsDosPath", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "IsFile", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "IsImplicitFile", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "IsLoopback", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "IsNotAbsoluteUri", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "IsUnc", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "IsUncOrDosPath", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "IsUncPath", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("string", "80/test/test", "LocalPath", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "localhost:80/test/test", "OriginalString", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("bool", "false", "OriginalStringSwitched", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("string", "80/test/test", "PathAndQuery", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("int", "-1", "Port", new List<ExpressionData>()
                {
                }, "int"),
                new ExpressionData("string", "80/test/test", "PrivateAbsolutePath", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "", "Query", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "localhost", "Scheme", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("ushort", "0", "SecuredPathIndex", new List<ExpressionData>()
                {
                }, "ushort"),
                new ExpressionData("string[]", "{string[3]}", "Segments", new List<ExpressionData>()
                {
                    new ExpressionData("string", "80/", "[0]", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "test/", "[1]", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "test", "[2]", new List<ExpressionData>()
                    {
                    }, "string")
                }, "string[]"),
                new ExpressionData("BuiltInUriParser", "{System.UriParser.BuiltInUriParser}", "Syntax", new List<ExpressionData>()
                {
                    new ExpressionData("int", "-1", "DefaultPort", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("UriSyntaxFlags", "OptionalAuthority | MayHaveUserInfo | MayHavePort | MayHavePath | MayHaveQuery | MayHaveFragment | AllowEmptyHost | AllowUncHost | AllowAnInternetHost | V1_UnknownUri | SimpleUserSyntax | BuiltInSyntax | AllowDOSPath | PathIsRooted | ConvertPathSlashes | CompressPath | AllowIdn | AllowIriParsing", "Flags", new List<ExpressionData>()
                    {
                    }, "System.UriSyntaxFlags"),
                    new ExpressionData("bool", "true", "IsSimple", new List<ExpressionData>()
                    {
                    }, "bool"),
                    new ExpressionData("string", "localhost", "SchemeName", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("UriSyntaxFlags", "OptionalAuthority | MayHaveUserInfo | MayHavePort | MayHavePath | MayHaveQuery | MayHaveFragment | AllowEmptyHost | AllowUncHost | AllowAnInternetHost | V1_UnknownUri | SimpleUserSyntax | BuiltInSyntax | AllowDOSPath | PathIsRooted | ConvertPathSlashes | CompressPath | AllowIdn | AllowIriParsing", "_flags", new List<ExpressionData>()
                    {
                    }, "System.UriSyntaxFlags"),
                    new ExpressionData("int", "-1", "_port", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("string", "localhost", "_scheme", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("UriSyntaxFlags", "None", "_updatableFlags", new List<ExpressionData>()
                    {
                    }, "System.UriSyntaxFlags"),
                    new ExpressionData("bool", "false", "_updatableFlagsUsed", new List<ExpressionData>()
                    {
                    }, "bool")
                }, "System.UriParser {System.UriParser.BuiltInUriParser}"),
                new ExpressionData("bool", "false", "UserDrivenParsing", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("bool", "false", "UserEscaped", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("string", "", "UserInfo", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "null", "_dnsSafeHost", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("Flags", "HostTypeMask | MinimalUriInfoSet | AllUriInfoSet | RestUnicodeNormalized", "_flags", new List<ExpressionData>()
                {
                }, "System.Uri.Flags"),
                new ExpressionData("UriInfo", "{System.Uri.UriInfo}", "_info", new List<ExpressionData>()
                {
                    new ExpressionData("string", "null", "DnsSafeHost", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "", "Host", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("MoreInfo", "{System.Uri.MoreInfo}", "MoreInfo", new List<ExpressionData>()
                    {
                        new ExpressionData("string", "localhost:80/test/test", "AbsoluteUri", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "", "Fragment", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("int", "0", "Hash", new List<ExpressionData>()
                        {
                        }, "int"),
                        new ExpressionData("string", "80/test/test", "Path", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "", "Query", new List<ExpressionData>()
                        {
                        }, "string"),
                        new ExpressionData("string", "null", "RemoteUrl", new List<ExpressionData>()
                        {
                        }, "string")
                    }, "System.Uri.MoreInfo"),
                    new ExpressionData("Offset", "{System.Uri.Offset}", "Offset", new List<ExpressionData>()
                    {
                        new ExpressionData("ushort", "22", "End", new List<ExpressionData>()
                        {
                        }, "ushort"),
                        new ExpressionData("ushort", "22", "Fragment", new List<ExpressionData>()
                        {
                        }, "ushort"),
                        new ExpressionData("ushort", "10", "Host", new List<ExpressionData>()
                        {
                        }, "ushort"),
                        new ExpressionData("ushort", "10", "Path", new List<ExpressionData>()
                        {
                        }, "ushort"),
                        new ExpressionData("ushort", "0", "PortValue", new List<ExpressionData>()
                        {
                        }, "ushort"),
                        new ExpressionData("ushort", "22", "Query", new List<ExpressionData>()
                        {
                        }, "ushort"),
                        new ExpressionData("ushort", "0", "Scheme", new List<ExpressionData>()
                        {
                        }, "ushort"),
                        new ExpressionData("ushort", "10", "User", new List<ExpressionData>()
                        {
                        }, "ushort")
                    }, "System.Uri.Offset"),
                    new ExpressionData("string", "null", "ScopeId", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("string", "localhost:80/test/test", "String", new List<ExpressionData>()
                    {
                    }, "string")
                }, "System.Uri.UriInfo"),
                new ExpressionData("bool", "true", "_iriParsing", new List<ExpressionData>()
                {
                }, "bool"),
                new ExpressionData("string", "null", "_originalUnicodeString", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("string", "localhost:80/test/test", "_string", new List<ExpressionData>()
                {
                }, "string"),
                new ExpressionData("BuiltInUriParser", "{System.UriParser.BuiltInUriParser}", "_syntax", new List<ExpressionData>()
                {
                    new ExpressionData("int", "-1", "DefaultPort", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("UriSyntaxFlags", "OptionalAuthority | MayHaveUserInfo | MayHavePort | MayHavePath | MayHaveQuery | MayHaveFragment | AllowEmptyHost | AllowUncHost | AllowAnInternetHost | V1_UnknownUri | SimpleUserSyntax | BuiltInSyntax | AllowDOSPath | PathIsRooted | ConvertPathSlashes | CompressPath | AllowIdn | AllowIriParsing", "Flags", new List<ExpressionData>()
                    {
                    }, "System.UriSyntaxFlags"),
                    new ExpressionData("bool", "true", "IsSimple", new List<ExpressionData>()
                    {
                    }, "bool"),
                    new ExpressionData("string", "localhost", "SchemeName", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("UriSyntaxFlags", "OptionalAuthority | MayHaveUserInfo | MayHavePort | MayHavePath | MayHaveQuery | MayHaveFragment | AllowEmptyHost | AllowUncHost | AllowAnInternetHost | V1_UnknownUri | SimpleUserSyntax | BuiltInSyntax | AllowDOSPath | PathIsRooted | ConvertPathSlashes | CompressPath | AllowIdn | AllowIriParsing", "_flags", new List<ExpressionData>()
                    {
                    }, "System.UriSyntaxFlags"),
                    new ExpressionData("int", "-1", "_port", new List<ExpressionData>()
                    {
                    }, "int"),
                    new ExpressionData("string", "localhost", "_scheme", new List<ExpressionData>()
                    {
                    }, "string"),
                    new ExpressionData("UriSyntaxFlags", "None", "_updatableFlags", new List<ExpressionData>()
                    {
                    }, "System.UriSyntaxFlags"),
                    new ExpressionData("bool", "false", "_updatableFlagsUsed", new List<ExpressionData>()
                    {
                    }, "bool")
                }, "System.UriParser {System.UriParser.BuiltInUriParser}")
            }, "System.Uri");

        }        
    }
}
