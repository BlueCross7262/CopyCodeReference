using System;

namespace CopyCodeReference
{
    internal static class CodeReferenceBuilder
    {
        public static string Build(string filePath, int startLine, int endLine, string selectedText)
        {
            string location = startLine == endLine
                ? $"{filePath}:{startLine}"
                : $"{filePath}:{startLine}-{endLine}";

            return location + Environment.NewLine + Environment.NewLine + selectedText;
        }
    }
}
