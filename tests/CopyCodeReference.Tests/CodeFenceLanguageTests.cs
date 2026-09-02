using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyCodeReference.Tests
{
    [TestClass]
    public class CodeFenceLanguageTests
    {
        [TestMethod]
        public void FromPath_CSharpFile_ReturnsCsharp()
        {
            Assert.AreEqual("csharp", CodeFenceLanguage.FromPath(@"D:\Project\Test.cs"));
        }

        [TestMethod]
        public void FromPath_MarkupFiles_ReturnXml()
        {
            Assert.AreEqual("xml", CodeFenceLanguage.FromPath(@"D:\Project\MainWindow.xaml"));
            Assert.AreEqual("xml", CodeFenceLanguage.FromPath(@"D:\Project\App.config.xml"));
        }

        [TestMethod]
        public void FromPath_MarkdownFile_ReturnsMarkdown()
        {
            Assert.AreEqual("markdown", CodeFenceLanguage.FromPath(@"D:\Project\README.md"));
        }

        [TestMethod]
        public void FromPath_ExtensionCasing_IsIgnored()
        {
            Assert.AreEqual("csharp", CodeFenceLanguage.FromPath(@"D:\Project\TEST.CS"));
        }

        [TestMethod]
        public void FromPath_UnknownExtension_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, CodeFenceLanguage.FromPath(@"D:\Project\notes.zzz"));
        }

        [TestMethod]
        public void FromPath_NoExtension_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, CodeFenceLanguage.FromPath(@"D:\Project\LICENSE"));
        }

        [TestMethod]
        public void FromPath_NullOrEmpty_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, CodeFenceLanguage.FromPath(null));
            Assert.AreEqual(string.Empty, CodeFenceLanguage.FromPath(string.Empty));
        }

        [TestMethod]
        public void FromPath_InvalidPathCharacters_ReturnsEmpty()
        {
            Assert.AreEqual(string.Empty, CodeFenceLanguage.FromPath("a\0b"));
        }

        [TestMethod]
        public void FromPath_RelativePath_UsesExtension()
        {
            Assert.AreEqual("javascript", CodeFenceLanguage.FromPath(@"src\app\main.js"));
        }
    }
}
