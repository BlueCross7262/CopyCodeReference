namespace CopyCodeReference
{
    internal static class OptionsPreview
    {
        public const string SamplePath = @"D:\Project\App\Main.cs";
        public const string SampleLineText = "var data = await LoadAsync();";

        private const int FirstLine = 42;
        private const int LastLine = 44;

        private const string SampleBlockText =
            "var data = await LoadAsync();\r\n" +
            "Items.AddRange(data);\r\n" +
            "IsLoaded = true;\r\n";

        public static string SingleLine(CodeReferenceOptions options)
        {
            return CodeReferenceBuilder.Build(SamplePath, FirstLine, FirstLine, SampleLineText, options);
        }

        public static string MultiLine(CodeReferenceOptions options)
        {
            return CodeReferenceBuilder.Build(SamplePath, FirstLine, LastLine, SampleBlockText, options);
        }

        public static string EmptySelection(CodeReferenceOptions options, bool includeCaretLineText)
        {
            return includeCaretLineText
                ? CodeReferenceBuilder.Build(SamplePath, FirstLine, FirstLine, SampleLineText, options)
                : CodeReferenceBuilder.BuildLocation(SamplePath, FirstLine, options);
        }
    }
}
