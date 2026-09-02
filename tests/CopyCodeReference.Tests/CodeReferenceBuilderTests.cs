using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyCodeReference.Tests
{
    [TestClass]
    public class CodeReferenceBuilderTests
    {
        [TestMethod]
        public void Build_SingleLine_JoinsLocationAndTextWithSingleSpace()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;");

            Assert.AreEqual(@"D:\Project\Test.cs:12 var a = 1;", actual);
        }

        [TestMethod]
        public void Build_MultipleLines_UsesLineRangeWithoutSelectedText()
        {
            string selectedText =
                "private async Task LoadAsync()" + Environment.NewLine +
                "{" + Environment.NewLine +
                "}";

            string actual = CodeReferenceBuilder.Build(
                @"D:\Project\SampleApp\ViewModels\MainViewModel.cs",
                42,
                46,
                selectedText);

            Assert.AreEqual(@"D:\Project\SampleApp\ViewModels\MainViewModel.cs:42-46", actual);
        }

        [TestMethod]
        public void Build_MultipleLines_OmitsSeparatorAndCode()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 10, 11, "first\r\nsecond");

            Assert.IsFalse(actual.Contains("first"));
            Assert.IsFalse(actual.Contains("second"));
            Assert.IsFalse(actual.Contains("\r"));
            Assert.IsFalse(actual.Contains("\n"));
        }

        [TestMethod]
        public void Build_EmptySelectedText_KeepsLocationAndSeparator()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 7, 7, string.Empty);

            Assert.AreEqual(@"D:\Project\Test.cs:7 ", actual);
        }

        [TestMethod]
        public void Build_IndentedText_PreservesLeadingWhitespace()
        {
            string selectedText = "        var indented = true;";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 3, 3, selectedText);

            Assert.AreEqual(@"D:\Project\Test.cs:3 " + selectedText, actual);
            Assert.IsTrue(actual.EndsWith("        var indented = true;", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Build_TabbedText_PreservesTabCharacters()
        {
            string selectedText = "\t\tvar tabbed = true;";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 5, 5, selectedText);

            Assert.AreEqual(@"D:\Project\Test.cs:5 " + selectedText, actual);
            Assert.IsTrue(actual.Contains("\t\t"));
        }

        [TestMethod]
        public void Build_SingleLineEndingWithCrLf_PreservesCarriageReturnLineFeed()
        {
            string selectedText = "var line = 1;\r\n";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 10, 10, selectedText);

            Assert.AreEqual(@"D:\Project\Test.cs:10 " + selectedText, actual);
            Assert.IsTrue(actual.EndsWith("var line = 1;\r\n", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Build_TrailingWhitespace_IsNotTrimmed()
        {
            string selectedText = "   var padded = 1;   ";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 8, 8, selectedText);

            Assert.AreEqual(@"D:\Project\Test.cs:8 " + selectedText, actual);
            Assert.IsTrue(actual.EndsWith("   var padded = 1;   ", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Build_KoreanFilePath_IsPreservedVerbatim()
        {
            string filePath = @"D:\프로젝트\소스\메인화면.cs";

            Assert.AreEqual(filePath + ":1 code", CodeReferenceBuilder.Build(filePath, 1, 1, "code"));
            Assert.AreEqual(filePath + ":1-2", CodeReferenceBuilder.Build(filePath, 1, 2, "code"));
        }

        [TestMethod]
        public void Build_UnicodeSelectedText_IsPreservedVerbatim()
        {
            string selectedText = "var 메시지 = \"안녕하세요 \u4E16\u754C \uD83D\uDE80\";";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 21, 21, selectedText);

            Assert.AreEqual(@"D:\Project\Test.cs:21 " + selectedText, actual);
        }

        [TestMethod]
        public void Build_SingleLine_SeparatorIsExactlyOneSpace()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 1, 1, "x");

            Assert.AreEqual(@"D:\Project\Test.cs:1 x", actual);
            Assert.IsFalse(actual.Contains("  "));
            Assert.IsFalse(actual.Contains(Environment.NewLine));
        }

        [TestMethod]
        public void Build_DefaultOverload_UsesColonFormat()
        {
            string expected = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 15, "ignored", CodeReferenceFormat.Colon);

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 15, "ignored");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Build_ColonFormat_SingleLine_UsesColonAndAppendsText()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;", CodeReferenceFormat.Colon);

            Assert.AreEqual(@"D:\Project\Test.cs:12 var a = 1;", actual);
        }

        [TestMethod]
        public void Build_ColonFormat_MultipleLines_UsesColonAndHyphenRange()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 15, "ignored", CodeReferenceFormat.Colon);

            Assert.AreEqual(@"D:\Project\Test.cs:12-15", actual);
        }

        [TestMethod]
        public void Build_ParenthesesFormat_SingleLine_WrapsLineAndAppendsText()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;", CodeReferenceFormat.Parentheses);

            Assert.AreEqual(@"D:\Project\Test.cs(12) var a = 1;", actual);
        }

        [TestMethod]
        public void Build_ParenthesesFormat_MultipleLines_WrapsRangeWithoutText()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 15, "ignored", CodeReferenceFormat.Parentheses);

            Assert.AreEqual(@"D:\Project\Test.cs(12-15)", actual);
        }

        [TestMethod]
        public void Build_GitHubFormat_SingleLine_UsesHashLineAndAppendsText()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;", CodeReferenceFormat.GitHub);

            Assert.AreEqual(@"D:\Project\Test.cs#L12 var a = 1;", actual);
        }

        [TestMethod]
        public void Build_GitHubFormat_MultipleLines_UsesHashLineRangeWithoutText()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 15, "ignored", CodeReferenceFormat.GitHub);

            Assert.AreEqual(@"D:\Project\Test.cs#L12-L15", actual);
        }

        [TestMethod]
        public void Build_UnknownFormat_ThrowsArgumentOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 1, 1, "x", (CodeReferenceFormat)99));
        }

        [TestMethod]
        public void Build_OptionsOverload_WithDefaults_MatchesLegacyOverload()
        {
            string expected = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;");

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;", new CodeReferenceOptions());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Build_NullOptions_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 1, 1, "x", (CodeReferenceOptions)null));
        }

        [TestMethod]
        public void Build_ForwardSlash_SingleLine_ConvertsPathSeparators()
        {
            var options = new CodeReferenceOptions { UseForwardSlash = true };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;", options);

            Assert.AreEqual("D:/Project/Test.cs:12 var a = 1;", actual);
        }

        [TestMethod]
        public void Build_ForwardSlash_MultipleLines_ConvertsPathSeparators()
        {
            var options = new CodeReferenceOptions { UseForwardSlash = true };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 15, "ignored", options);

            Assert.AreEqual("D:/Project/Test.cs:12-15", actual);
        }

        [TestMethod]
        public void Build_ForwardSlash_GitHubFormat_ProducesGitHubStyleReference()
        {
            var options = new CodeReferenceOptions
            {
                Format = CodeReferenceFormat.GitHub,
                UseForwardSlash = true
            };

            string actual = CodeReferenceBuilder.Build(@"ViewModels\MainViewModel.cs", 12, 15, "ignored", options);

            Assert.AreEqual("ViewModels/MainViewModel.cs#L12-L15", actual);
        }

        [TestMethod]
        public void Build_ForwardSlash_DoesNotTouchSelectedText()
        {
            var options = new CodeReferenceOptions { UseForwardSlash = true };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, @"var path = @""C:\Temp\a.txt"";", options);

            Assert.AreEqual(@"D:/Project/Test.cs:12 var path = @""C:\Temp\a.txt"";", actual);
        }

        [TestMethod]
        public void Build_ForwardSlash_UncPath_KeepsBothLeadingSeparators()
        {
            var options = new CodeReferenceOptions { UseForwardSlash = true };

            string actual = CodeReferenceBuilder.Build(@"\\server\share\Test.cs", 3, 3, "x", options);

            Assert.AreEqual("//server/share/Test.cs:3 x", actual);
        }

        [TestMethod]
        public void Build_ForwardSlashDisabled_KeepsBackslashes()
        {
            var options = new CodeReferenceOptions { UseForwardSlash = false };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;", options);

            Assert.AreEqual(@"D:\Project\Test.cs:12 var a = 1;", actual);
        }

        [TestMethod]
        public void Build_MultiLineCode_AppendsSelectedTextOnNextLine()
        {
            var options = new CodeReferenceOptions { MultiLineBody = MultiLineBody.Code };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 13, "var a = 1;\r\nvar b = 2;", options);

            Assert.AreEqual("D:\\Project\\Test.cs:12-13\r\nvar a = 1;\r\nvar b = 2;", actual);
        }

        [TestMethod]
        public void Build_MultiLineCode_TrimsTrailingNewLine()
        {
            var options = new CodeReferenceOptions { MultiLineBody = MultiLineBody.Code };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 13, "var a = 1;\r\nvar b = 2;\r\n", options);

            Assert.AreEqual("D:\\Project\\Test.cs:12-13\r\nvar a = 1;\r\nvar b = 2;", actual);
        }

        [TestMethod]
        public void Build_MultiLineCode_PreservesInnerIndentation()
        {
            var options = new CodeReferenceOptions { MultiLineBody = MultiLineBody.Code };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 1, 2, "    var a = 1;\r\n\tvar b = 2;", options);

            Assert.AreEqual("D:\\Project\\Test.cs:1-2\r\n    var a = 1;\r\n\tvar b = 2;", actual);
        }

        [TestMethod]
        public void Build_MultiLineFencedCode_WrapsBodyWithLanguageTag()
        {
            var options = new CodeReferenceOptions { MultiLineBody = MultiLineBody.FencedCode };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 13, "var a = 1;\r\nvar b = 2;", options);

            Assert.AreEqual("D:\\Project\\Test.cs:12-13\r\n```csharp\r\nvar a = 1;\r\nvar b = 2;\r\n```", actual);
        }

        [TestMethod]
        public void Build_MultiLineFencedCode_UnknownExtension_UsesBareFence()
        {
            var options = new CodeReferenceOptions { MultiLineBody = MultiLineBody.FencedCode };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\notes.zzz", 1, 2, "alpha\r\nbeta", options);

            Assert.AreEqual("D:\\Project\\notes.zzz:1-2\r\n```\r\nalpha\r\nbeta\r\n```", actual);
        }

        [TestMethod]
        public void Build_MultiLineFencedCode_BodyContainingFence_UsesLongerFence()
        {
            var options = new CodeReferenceOptions { MultiLineBody = MultiLineBody.FencedCode };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\notes.md", 1, 3, "```\r\ncode\r\n```", options);

            Assert.AreEqual("D:\\Project\\notes.md:1-3\r\n````markdown\r\n```\r\ncode\r\n```\r\n````", actual);
        }

        [TestMethod]
        public void Build_MultiLineFencedCode_TrimsTrailingNewLineBeforeClosingFence()
        {
            var options = new CodeReferenceOptions { MultiLineBody = MultiLineBody.FencedCode };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 1, 2, "var a = 1;\r\nvar b = 2;\r\n", options);

            Assert.AreEqual("D:\\Project\\Test.cs:1-2\r\n```csharp\r\nvar a = 1;\r\nvar b = 2;\r\n```", actual);
        }

        [TestMethod]
        public void Build_MultiLineCode_ForwardSlash_DoesNotTouchBodyBackslashes()
        {
            var options = new CodeReferenceOptions
            {
                UseForwardSlash = true,
                MultiLineBody = MultiLineBody.Code
            };

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 1, 2, "var a = 1;\r\nvar p = @\"C:\\Temp\";", options);

            Assert.AreEqual("D:/Project/Test.cs:1-2\r\nvar a = 1;\r\nvar p = @\"C:\\Temp\";", actual);
        }

        [TestMethod]
        public void Build_MultiLineBody_DoesNotChangeSingleLineOutput()
        {
            foreach (MultiLineBody body in new[] { MultiLineBody.LocationOnly, MultiLineBody.Code, MultiLineBody.FencedCode })
            {
                var options = new CodeReferenceOptions { MultiLineBody = body };

                string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;", options);

                Assert.AreEqual(@"D:\Project\Test.cs:12 var a = 1;", actual, body.ToString());
            }
        }

        [TestMethod]
        public void Build_UnknownMultiLineBody_ThrowsArgumentOutOfRange()
        {
            var options = new CodeReferenceOptions { MultiLineBody = (MultiLineBody)99 };

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 1, 2, "x", options));
        }
    }
}
