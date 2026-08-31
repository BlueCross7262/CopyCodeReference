namespace CopyCodeReference
{
    internal static class CodeReferenceBuilder
    {
        public static string Build(string filePath, int startLine, int endLine, string selectedText)
        {
            if (startLine != endLine)
            {
                return $"{filePath}:{startLine}-{endLine}";
            }

            return $"{filePath}:{startLine} {selectedText}";
        }
    }
}
