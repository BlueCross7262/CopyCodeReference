using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyCodeReference.Tests
{
    [TestClass]
    public class RelativePathResolverTests
    {
        [TestMethod]
        public void Resolve_FileDirectlyUnderBase_ReturnsFileName()
        {
            string actual = RelativePathResolver.Resolve(@"D:\Solution\Program.cs", @"D:\Solution");

            Assert.AreEqual(@"Program.cs", actual);
        }

        [TestMethod]
        public void Resolve_FileInSubfolder_ReturnsSubfolderPath()
        {
            string actual = RelativePathResolver.Resolve(
                @"D:\Solution\ViewModels\MainViewModel.cs",
                @"D:\Solution");

            Assert.AreEqual(@"ViewModels\MainViewModel.cs", actual);
        }

        [TestMethod]
        public void Resolve_BaseWithTrailingSeparator_IsNormalized()
        {
            string actual = RelativePathResolver.Resolve(
                @"D:\Solution\ViewModels\MainViewModel.cs",
                @"D:\Solution\");

            Assert.AreEqual(@"ViewModels\MainViewModel.cs", actual);
        }

        [TestMethod]
        public void Resolve_CaseDifferenceOnly_IsTreatedAsMatch()
        {
            string actual = RelativePathResolver.Resolve(
                @"d:\solution\ViewModels\MainViewModel.cs",
                @"D:\Solution");

            Assert.AreEqual(@"ViewModels\MainViewModel.cs", actual);
        }

        [TestMethod]
        public void Resolve_SiblingDirectoryWithSharedPrefix_ReturnsNull()
        {
            string actual = RelativePathResolver.Resolve(@"D:\SolutionOther\Program.cs", @"D:\Solution");

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Resolve_FileOutsideBase_ReturnsNull()
        {
            string actual = RelativePathResolver.Resolve(@"D:\Other\Program.cs", @"D:\Solution");

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Resolve_FileOnDifferentDrive_ReturnsNull()
        {
            string actual = RelativePathResolver.Resolve(@"C:\Solution\Program.cs", @"D:\Solution");

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Resolve_FileAboveBase_ReturnsNull()
        {
            string actual = RelativePathResolver.Resolve(@"D:\Program.cs", @"D:\Solution");

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Resolve_FileEqualToBase_ReturnsNull()
        {
            string actual = RelativePathResolver.Resolve(@"D:\Solution", @"D:\Solution");

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Resolve_KoreanPath_ReturnsRelativePath()
        {
            string actual = RelativePathResolver.Resolve(
                @"D:\프로젝트\소스\메인화면.cs",
                @"D:\프로젝트");

            Assert.AreEqual(@"소스\메인화면.cs", actual);
        }

        [TestMethod]
        public void Resolve_ForwardSlashesInInput_AreNormalizedToBackslashes()
        {
            string actual = RelativePathResolver.Resolve(
                "D:/Solution/ViewModels/MainViewModel.cs",
                "D:/Solution");

            Assert.AreEqual(@"ViewModels\MainViewModel.cs", actual);
        }

        [TestMethod]
        public void Resolve_UncPath_ReturnsRelativePath()
        {
            string actual = RelativePathResolver.Resolve(
                @"\\server\share\Solution\App\Program.cs",
                @"\\server\share\Solution");

            Assert.AreEqual(@"App\Program.cs", actual);
        }

        [TestMethod]
        public void Resolve_NullOrEmptyInput_ReturnsNull()
        {
            Assert.IsNull(RelativePathResolver.Resolve(null, @"D:\Solution"));
            Assert.IsNull(RelativePathResolver.Resolve(@"D:\Solution\Program.cs", null));
            Assert.IsNull(RelativePathResolver.Resolve(string.Empty, @"D:\Solution"));
            Assert.IsNull(RelativePathResolver.Resolve(@"D:\Solution\Program.cs", string.Empty));
        }

        [TestMethod]
        public void Resolve_BaseThatIsOnlySeparators_ReturnsNull()
        {
            Assert.IsNull(RelativePathResolver.Resolve(@"D:\Solution\Program.cs", @"\"));
        }

        [TestMethod]
        public void Resolve_DeepNesting_KeepsFullRelativeSegmentChain()
        {
            string actual = RelativePathResolver.Resolve(
                @"D:\Solution\src\App\ViewModels\Main\MainViewModel.cs",
                @"D:\Solution");

            Assert.AreEqual(@"src\App\ViewModels\Main\MainViewModel.cs", actual);
        }
    }
}
