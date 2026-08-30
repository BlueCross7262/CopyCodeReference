using System;

namespace CopyCodeReference
{
    internal struct LineRange
    {
        public LineRange(int startLine, int endLine)
        {
            StartLine = startLine;
            EndLine = endLine;
        }

        public int StartLine { get; }

        public int EndLine { get; }
    }

    internal static class LineRangeCalculator
    {
        public static LineRange Calculate(
            int spanStart,
            int spanLength,
            int bufferLength,
            Func<int, int> lineNumberFromPosition)
        {
            if (lineNumberFromPosition == null)
            {
                throw new ArgumentNullException(nameof(lineNumberFromPosition));
            }

            if (bufferLength < 0)
            {
                bufferLength = 0;
            }

            int start = Clamp(spanStart, 0, bufferLength);
            int length = spanLength < 0 ? 0 : spanLength;

            long rawEnd = (long)start + length;
            int end = rawEnd > bufferLength ? bufferLength : (int)rawEnd;

            int inclusiveEnd = end > start ? end - 1 : start;

            int startLine = lineNumberFromPosition(start) + 1;
            int endLine = lineNumberFromPosition(inclusiveEnd) + 1;

            if (endLine < startLine)
            {
                endLine = startLine;
            }

            return new LineRange(startLine, endLine);
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }

            return value > max ? max : value;
        }
    }
}
