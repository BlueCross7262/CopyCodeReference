using System;

namespace CopyCodeReference
{
    internal static class CodeReferenceBuilder
    {
        private const string NewLine = "\r\n";
        private const char FenceCharacter = '`';
        private const int MinimumFenceLength = 3;

        public static string Build(string filePath, int startLine, int endLine, string selectedText)
        {
            return Build(filePath, startLine, endLine, selectedText, CodeReferenceFormat.Colon);
        }

        public static string Build(string filePath, int startLine, int endLine, string selectedText, CodeReferenceFormat format)
        {
            return Build(filePath, startLine, endLine, selectedText, new CodeReferenceOptions { Format = format });
        }

        public static string Build(string filePath, int startLine, int endLine, string selectedText, CodeReferenceOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            string displayPath = options.UseForwardSlash && filePath != null
                ? filePath.Replace('\\', '/')
                : filePath;

            string location = BuildLocation(displayPath, startLine, endLine, options.Format);

            if (startLine == endLine)
            {
                return $"{location} {selectedText}";
            }

            return BuildMultiLine(location, filePath, selectedText, options);
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

        private static string BuildMultiLine(string location, string filePath, string selectedText, CodeReferenceOptions options)
        {
            switch (options.MultiLineBody)
            {
                case MultiLineBody.LocationOnly:
                    return location;
                case MultiLineBody.Code:
                    return location + NewLine + TrimTrailingNewLine(selectedText);
                case MultiLineBody.FencedCode:
                    string body = TrimTrailingNewLine(selectedText);
                    string fence = BuildFence(body);
                    return location + NewLine + fence + CodeFenceLanguage.FromPath(filePath) + NewLine + body + NewLine + fence;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options), options.MultiLineBody, null);
            }
        }

        private static string TrimTrailingNewLine(string text)
        {
            return text == null ? string.Empty : text.TrimEnd('\r', '\n');
        }

        private static string BuildFence(string body)
        {
            return new string(FenceCharacter, LongestFenceRun(body) + 1);
        }

        private static int LongestFenceRun(string body)
        {
            int longest = MinimumFenceLength - 1;
            int current = 0;

            foreach (char character in body)
            {
                if (character == FenceCharacter)
                {
                    current++;

                    if (current > longest)
                    {
                        longest = current;
                    }

                    continue;
                }

                current = 0;
            }

            return longest;
        }
    }
}
