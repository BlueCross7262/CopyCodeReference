using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CopyCodeReference.Tests
{
    [TestClass]
    public class LineRangeCalculatorTests
    {
        private const string FourLineBuffer = "AAA\r\nBBB\r\nCCC\r\nDDD";

        [TestMethod]
        public void Calculate_SelectionEndingAtStartOfNextLine_ExcludesThatLine()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(0);
            int end = snapshot.GetLineStart(3);

            LineRange range = Calculate(snapshot, start, end - start);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(3, range.EndLine);
        }

        [TestMethod]
        public void Calculate_SelectionEndingMidLine_IncludesThatLine()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(0);
            int end = snapshot.GetLineStart(3) + 2;

            LineRange range = Calculate(snapshot, start, end - start);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(4, range.EndLine);
        }

        [TestMethod]
        public void Calculate_SingleLineSelection_ReturnsSameStartAndEndLine()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(1);

            LineRange range = Calculate(snapshot, start, 3);

            Assert.AreEqual(2, range.StartLine);
            Assert.AreEqual(2, range.EndLine);
        }

        [TestMethod]
        public void Calculate_SelectionEndingOnCarriageReturn_StaysOnSameLine()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(0);
            int end = snapshot.GetLineStart(0) + 4;

            LineRange range = Calculate(snapshot, start, end - start);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(1, range.EndLine);
        }

        [TestMethod]
        public void Calculate_SelectionCoveringOnlyLineBreak_StaysOnOwningLine()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(0) + 3;

            LineRange range = Calculate(snapshot, start, 2);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(1, range.EndLine);
        }

        [TestMethod]
        public void Calculate_SelectionEndingAtEndOfBuffer_ReturnsLastLine()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(2);

            LineRange range = Calculate(snapshot, start, snapshot.Length - start);

            Assert.AreEqual(3, range.StartLine);
            Assert.AreEqual(4, range.EndLine);
        }

        [TestMethod]
        public void Calculate_SelectionCoveringWholeBufferEndingWithNewLine_ExcludesPhantomLine()
        {
            var snapshot = new FakeSnapshot("AAA\r\nBBB\r\n");

            LineRange range = Calculate(snapshot, 0, snapshot.Length);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(2, range.EndLine);
        }

        [TestMethod]
        public void Calculate_ZeroLengthSpan_ReturnsCaretLineForBothEnds()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(2);

            LineRange range = Calculate(snapshot, start, 0);

            Assert.AreEqual(3, range.StartLine);
            Assert.AreEqual(3, range.EndLine);
        }

        [TestMethod]
        public void Calculate_ZeroLengthSpanAtStartOfLine_DoesNotUnderflowToPreviousLine()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(3);

            LineRange range = Calculate(snapshot, start, 0);

            Assert.AreEqual(4, range.StartLine);
            Assert.AreEqual(4, range.EndLine);
        }

        [TestMethod]
        public void Calculate_EmptyBuffer_ReturnsFirstLine()
        {
            var snapshot = new FakeSnapshot(string.Empty);

            LineRange range = Calculate(snapshot, 0, 0);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(1, range.EndLine);
        }

        [TestMethod]
        public void Calculate_LengthBeyondBuffer_IsClampedToBufferEnd()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);

            LineRange range = Calculate(snapshot, 0, snapshot.Length + 1000);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(4, range.EndLine);
        }

        [TestMethod]
        public void Calculate_LengthCausingIntegerOverflow_IsClampedToBufferEnd()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);

            LineRange range = Calculate(snapshot, 2, int.MaxValue);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(4, range.EndLine);
        }

        [TestMethod]
        public void Calculate_NegativeStart_IsClampedToBufferStart()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);

            LineRange range = Calculate(snapshot, -5, 3);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(1, range.EndLine);
        }

        [TestMethod]
        public void Calculate_NegativeLength_IsTreatedAsZeroLength()
        {
            var snapshot = new FakeSnapshot(FourLineBuffer);
            int start = snapshot.GetLineStart(1);

            LineRange range = Calculate(snapshot, start, -10);

            Assert.AreEqual(2, range.StartLine);
            Assert.AreEqual(2, range.EndLine);
        }

        [TestMethod]
        public void Calculate_LineFeedOnlyBuffer_ComputesLineNumbers()
        {
            var snapshot = new FakeSnapshot("AAA\nBBB\nCCC");
            int start = snapshot.GetLineStart(0);
            int end = snapshot.GetLineStart(2);

            LineRange range = Calculate(snapshot, start, end - start);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(2, range.EndLine);
        }

        [TestMethod]
        public void Calculate_CarriageReturnOnlyBuffer_ComputesLineNumbers()
        {
            var snapshot = new FakeSnapshot("AAA\rBBB\rCCC");
            int start = snapshot.GetLineStart(0);
            int end = snapshot.GetLineStart(2);

            LineRange range = Calculate(snapshot, start, end - start);

            Assert.AreEqual(1, range.StartLine);
            Assert.AreEqual(2, range.EndLine);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Calculate_NullLineLookup_Throws()
        {
            LineRangeCalculator.Calculate(0, 1, 10, null);
        }

        private static LineRange Calculate(FakeSnapshot snapshot, int spanStart, int spanLength)
        {
            return LineRangeCalculator.Calculate(
                spanStart,
                spanLength,
                snapshot.Length,
                snapshot.GetLineNumberFromPosition);
        }

        private sealed class FakeSnapshot
        {
            private readonly string _text;
            private readonly List<int> _lineStarts;

            public FakeSnapshot(string text)
            {
                _text = text ?? string.Empty;
                _lineStarts = new List<int> { 0 };

                for (int i = 0; i < _text.Length; i++)
                {
                    char current = _text[i];

                    if (current == '\r')
                    {
                        if (i + 1 < _text.Length && _text[i + 1] == '\n')
                        {
                            i++;
                        }

                        _lineStarts.Add(i + 1);
                    }
                    else if (current == '\n')
                    {
                        _lineStarts.Add(i + 1);
                    }
                }
            }

            public int Length
            {
                get { return _text.Length; }
            }

            public int GetLineStart(int zeroBasedLine)
            {
                return _lineStarts[zeroBasedLine];
            }

            public int GetLineNumberFromPosition(int position)
            {
                if (position < 0 || position > _text.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(position));
                }

                int line = 0;

                for (int i = 1; i < _lineStarts.Count; i++)
                {
                    if (_lineStarts[i] <= position)
                    {
                        line = i;
                    }
                    else
                    {
                        break;
                    }
                }

                return line;
            }
        }
    }
}
