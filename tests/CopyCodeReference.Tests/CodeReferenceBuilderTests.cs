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
    }
}
