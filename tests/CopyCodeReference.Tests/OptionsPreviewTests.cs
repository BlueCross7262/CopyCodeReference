using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyCodeReference.Tests
{
    [TestClass]
    public class OptionsPreviewTests
    {
        [TestMethod]
        public void SingleLine_DefaultOptions_AppendsSampleText()
        {
            string actual = OptionsPreview.SingleLine(new CodeReferenceOptions());

            Assert.AreEqual(OptionsPreview.SamplePath + ":42 " + OptionsPreview.SampleLineText, actual);
        }

        [TestMethod]
        public void MultiLine_DefaultOptions_IsLocationOnly()
        {
            string actual = OptionsPreview.MultiLine(new CodeReferenceOptions());

            Assert.AreEqual(OptionsPreview.SamplePath + ":42-44", actual);
        }

        [TestMethod]
        public void MultiLine_FencedCode_MatchesBuilderOutput()
        {
            CodeReferenceOptions options = new CodeReferenceOptions { MultiLineBody = MultiLineBody.FencedCode };

            string actual = OptionsPreview.MultiLine(options);

            StringAssert.StartsWith(actual, OptionsPreview.SamplePath + ":42-44\r\n```csharp\r\n");
            StringAssert.EndsWith(actual, "\r\n```");
        }

        [TestMethod]
        public void EmptySelection_WithoutCaretText_IsLocationOnly()
        {
            string actual = OptionsPreview.EmptySelection(new CodeReferenceOptions(), false);

            Assert.AreEqual(OptionsPreview.SamplePath + ":42", actual);
        }

        [TestMethod]
        public void EmptySelection_WithCaretText_AppendsLineText()
        {
            string actual = OptionsPreview.EmptySelection(new CodeReferenceOptions(), true);

            Assert.AreEqual(OptionsPreview.SamplePath + ":42 " + OptionsPreview.SampleLineText, actual);
        }

        [TestMethod]
        public void AllRows_FollowFormatAndSeparator()
        {
            CodeReferenceOptions options = new CodeReferenceOptions { Format = CodeReferenceFormat.GitHub, UseForwardSlash = true };
            string path = OptionsPreview.SamplePath.Replace('\\', '/');

            StringAssert.StartsWith(OptionsPreview.SingleLine(options), path + "#L42 ");
            Assert.AreEqual(path + "#L42-L44", OptionsPreview.MultiLine(options));
            Assert.AreEqual(path + "#L42", OptionsPreview.EmptySelection(options, false));
        }
    }
}
