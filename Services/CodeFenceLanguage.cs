using System;
using System.IO;

namespace CopyCodeReference
{
    internal static class CodeFenceLanguage
    {
        public static string FromPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return string.Empty;
            }

            string extension;

            try
            {
                extension = Path.GetExtension(filePath);
            }
            catch (ArgumentException)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(extension))
            {
                return string.Empty;
            }

            switch (extension.ToLowerInvariant())
            {
                case ".cs":
                    return "csharp";
                case ".xaml":
                case ".xml":
                    return "xml";
                case ".json":
                    return "json";
                case ".js":
                    return "javascript";
                case ".ts":
                    return "typescript";
                case ".py":
                    return "python";
                case ".cpp":
                case ".h":
                    return "cpp";
                case ".sql":
                    return "sql";
                case ".md":
                    return "markdown";
                default:
                    return string.Empty;
            }
        }
    }
}
