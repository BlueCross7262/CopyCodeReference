using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyCodeReference.Tests
{
    [TestClass]
    public class UiTextTests
    {
        [TestMethod]
        public void For_KoreanCulture_ReturnsKoreanText()
        {
            UiText text = UiText.For(new CultureInfo("ko-KR"));

            Assert.AreEqual("위치 표기", text.LocationFormatHeader);
        }

        [TestMethod]
        public void For_NeutralKoreanCulture_ReturnsKoreanText()
        {
            UiText text = UiText.For(new CultureInfo("ko"));

            Assert.AreEqual("위치 표기", text.LocationFormatHeader);
        }

        [TestMethod]
        public void For_EnglishCulture_ReturnsEnglishText()
        {
            UiText text = UiText.For(new CultureInfo("en-US"));

            Assert.AreEqual("Location format", text.LocationFormatHeader);
        }

        [TestMethod]
        public void For_OtherCulture_FallsBackToEnglish()
        {
            UiText text = UiText.For(new CultureInfo("ja-JP"));

            Assert.AreEqual("Location format", text.LocationFormatHeader);
        }

        [TestMethod]
        public void For_NullCulture_FallsBackToEnglish()
        {
            UiText text = UiText.For(null);

            Assert.AreEqual("Location format", text.LocationFormatHeader);
        }

        [TestMethod]
        public void CopiedStatus_English_PrefixesCopied()
        {
            Assert.AreEqual("Copied Foo.cs:12", UiText.For(new CultureInfo("en-US")).CopiedStatus("Foo.cs:12"));
        }

        [TestMethod]
        public void CopiedStatus_Korean_UsesKoreanPrefix()
        {
            Assert.AreEqual("복사함: Foo.cs:12", UiText.For(new CultureInfo("ko-KR")).CopiedStatus("Foo.cs:12"));
        }

        [TestMethod]
        public void AllStrings_AreFilledInBothLanguages()
        {
            foreach (UiText text in new[] { UiText.For(new CultureInfo("en-US")), UiText.For(new CultureInfo("ko-KR")) })
            {
                foreach (var property in typeof(UiText).GetProperties())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        string value = (string)property.GetValue(text);
                        Assert.IsFalse(string.IsNullOrWhiteSpace(value), property.Name);
                    }
                }
            }
        }
    }
}
