using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyCodeReference.Tests
{
    [TestClass]
    public class CodeReferenceBuilderTests
    {
        private static readonly string NewLine = Environment.NewLine;

        [TestMethod]
        public void Build_SingleLine_UsesSingleLineNumberFormat()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 12, 12, "var a = 1;");

            Assert.AreEqual(@"D:\Project\Test.cs:12" + NewLine + NewLine + "var a = 1;", actual);
        }

        [TestMethod]
        public void Build_MultipleLines_UsesLineRangeFormat()
        {
            string selectedText =
                "private async Task LoadAsync()" + NewLine +
                "{" + NewLine +
                "}";

            string actual = CodeReferenceBuilder.Build(
                @"D:\Project\SampleApp\ViewModels\MainViewModel.cs",
                42,
                46,
                selectedText);

            Assert.AreEqual(
                @"D:\Project\SampleApp\ViewModels\MainViewModel.cs:42-46" + NewLine + NewLine + selectedText,
                actual);
        }

        [TestMethod]
        public void Build_EmptySelectedText_KeepsLocationAndBlankLine()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 7, 7, string.Empty);

            Assert.AreEqual(@"D:\Project\Test.cs:7" + NewLine + NewLine, actual);
        }

        [TestMethod]
        public void Build_IndentedText_PreservesLeadingWhitespace()
        {
            string selectedText = "        var indented = true;";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 3, 3, selectedText);

            Assert.IsTrue(actual.EndsWith(selectedText, StringComparison.Ordinal));
            Assert.IsTrue(actual.Contains("        var indented = true;"));
        }

        [TestMethod]
        public void Build_TabbedText_PreservesTabCharacters()
        {
            string selectedText = "\t\tvar tabbed = true;";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 5, 5, selectedText);

            Assert.IsTrue(actual.EndsWith(selectedText, StringComparison.Ordinal));
            Assert.IsTrue(actual.Contains("\t\t"));
        }

        [TestMethod]
        public void Build_CrLfText_PreservesCarriageReturnLineFeed()
        {
            string selectedText = "first\r\nsecond\r\n";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 10, 11, selectedText);

            Assert.IsTrue(actual.EndsWith(selectedText, StringComparison.Ordinal));
            Assert.IsTrue(actual.Contains("first\r\nsecond\r\n"));
        }

        [TestMethod]
        public void Build_TrailingWhitespace_IsNotTrimmed()
        {
            string selectedText = "   var padded = 1;   ";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 8, 8, selectedText);

            Assert.IsTrue(actual.EndsWith("   var padded = 1;   ", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Build_KoreanFilePath_IsPreservedVerbatim()
        {
            string filePath = @"D:\프로젝트\소스\메인화면.cs";

            string actual = CodeReferenceBuilder.Build(filePath, 1, 2, "code");

            Assert.AreEqual(filePath + ":1-2" + NewLine + NewLine + "code", actual);
        }

        [TestMethod]
        public void Build_UnicodeSelectedText_IsPreservedVerbatim()
        {
            string selectedText = "var 메시지 = \"안녕하세요 \u4E16\u754C \uD83D\uDE80\";";

            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 21, 21, selectedText);

            Assert.AreEqual(@"D:\Project\Test.cs:21" + NewLine + NewLine + selectedText, actual);
        }

        [TestMethod]
        public void Build_SeparatorBetweenLocationAndCode_IsExactlyOneBlankLine()
        {
            string actual = CodeReferenceBuilder.Build(@"D:\Project\Test.cs", 1, 1, "x");

            Assert.AreEqual(@"D:\Project\Test.cs:1" + NewLine + NewLine + "x", actual);
            Assert.IsFalse(actual.Contains(NewLine + NewLine + NewLine));
        }
    }
}
