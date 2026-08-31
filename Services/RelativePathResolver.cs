using System;

namespace CopyCodeReference
{
    internal static class RelativePathResolver
    {
        public static string Resolve(string filePath, string baseDirectory)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(baseDirectory))
            {
                return null;
            }

            string normalizedBase = NormalizeDirectory(baseDirectory);

            if (normalizedBase == null)
            {
                return null;
            }

            string normalizedFile = filePath.Replace('/', '\\');

            if (normalizedFile.Length <= normalizedBase.Length)
            {
                return null;
            }

            if (!normalizedFile.StartsWith(normalizedBase, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            string relative = normalizedFile.Substring(normalizedBase.Length);

            return relative.Length == 0 ? null : relative;
        }

        private static string NormalizeDirectory(string directory)
        {
            string normalized = directory.Replace('/', '\\').TrimEnd('\\');

            return normalized.Length == 0 ? null : normalized + "\\";
        }
    }
}
