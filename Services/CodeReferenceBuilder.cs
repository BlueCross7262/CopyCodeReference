using System;

namespace CopyCodeReference
{
    internal static class CodeReferenceBuilder
    {
        public static string Build(string filePath, int startLine, int endLine, string selectedText)
        {
            return Build(filePath, startLine, endLine, selectedText, CodeReferenceFormat.Colon);
        }

        public static string Build(string filePath, int startLine, int endLine, string selectedText, CodeReferenceFormat format)
        {
            string location = BuildLocation(filePath, startLine, endLine, format);
            if (startLine != endLine)
            {
                return location;
            }

            return $"{location} {selectedText}";
        }

        private static string BuildLocation(string filePath, int startLine, int endLine, CodeReferenceFormat format)
        {
            bool isRange = startLine != endLine;
            switch (format)
            {
                case CodeReferenceFormat.Colon:
                    return isRange ? $"{filePath}:{startLine}-{endLine}" : $"{filePath}:{startLine}";
                case CodeReferenceFormat.Parentheses:
                    return isRange ? $"{filePath}({startLine}-{endLine})" : $"{filePath}({startLine})";
                case CodeReferenceFormat.GitHub:
                    return isRange ? $"{filePath}#L{startLine}-L{endLine}" : $"{filePath}#L{startLine}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}
