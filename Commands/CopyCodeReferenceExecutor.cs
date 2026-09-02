using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace CopyCodeReference
{
    internal static class CopyCodeReferenceExecutor
    {
        private const int ClipboardRetryCount = 5;
        private const int ClipboardRetryDelayMilliseconds = 20;

        public static async Task ExecuteAsync(bool useSolutionRelativePath)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                DocumentView documentView = await VS.Documents.GetActiveDocumentViewAsync();
                IWpfTextView textView = documentView?.TextView;

                if (textView == null || textView.IsClosed)
                {
                    return;
                }

                string filePath = TryGetFilePath(textView.TextBuffer);

                if (filePath == null)
                {
                    return;
                }

                General options = await General.GetLiveInstanceAsync();

                LineRange range;
                string selectedText;

                if (!TryGetSelectedLines(textView, out range, out selectedText))
                {
                    if (!options.CopyCaretLineWhenNoSelection)
                    {
                        return;
                    }

                    GetCaretLine(textView, out range, out selectedText);
                }

                string displayPath = useSolutionRelativePath
                    ? await ResolveSolutionRelativePathAsync(filePath)
                    : filePath;

                CodeReferenceOptions referenceOptions = new CodeReferenceOptions
                {
                    Format = options.Format,
                    UseForwardSlash = options.UseForwardSlash,
                    MultiLineBody = options.MultiLineBody
                };

                string reference = CodeReferenceBuilder.Build(
                    displayPath,
                    range.StartLine,
                    range.EndLine,
                    selectedText,
                    referenceOptions);

                if (await TrySetClipboardTextAsync(reference))
                {
                    await VS.StatusBar.ShowMessageAsync(BuildStatusMessage(displayPath, range));
                }
            }
            catch (Exception ex)
            {
                await ex.LogAsync();
            }
        }

        private static bool TryGetSelectedLines(IWpfTextView textView, out LineRange range, out string selectedText)
        {
            range = default(LineRange);
            selectedText = null;

            ITextSelection selection = textView.Selection;

            if (selection == null || selection.IsEmpty)
            {
                return false;
            }

            SnapshotSpan span = selection.StreamSelectionSpan.SnapshotSpan;

            if (span.IsEmpty)
            {
                return false;
            }

            ITextSnapshot snapshot = span.Snapshot;

            range = LineRangeCalculator.Calculate(
                span.Start.Position,
                span.Length,
                snapshot.Length,
                position => snapshot.GetLineNumberFromPosition(position));

            selectedText = span.GetText();
            return true;
        }

        private static void GetCaretLine(IWpfTextView textView, out LineRange range, out string selectedText)
        {
            ITextSnapshotLine line = textView.Caret.Position.BufferPosition.GetContainingLine();
            int lineNumber = line.LineNumber + 1;

            range = new LineRange(lineNumber, lineNumber);
            selectedText = line.GetText();
        }

        private static async Task<string> ResolveSolutionRelativePathAsync(string filePath)
        {
            try
            {
                SolutionItem solution = await VS.Solutions.GetCurrentSolutionAsync();
                string solutionPath = solution?.FullPath;

                if (string.IsNullOrEmpty(solutionPath))
                {
                    return filePath;
                }

                string solutionDirectory = Path.GetDirectoryName(solutionPath);

                return RelativePathResolver.Resolve(filePath, solutionDirectory) ?? filePath;
            }
            catch (Exception)
            {
                return filePath;
            }
        }

        private static string TryGetFilePath(ITextBuffer textBuffer)
        {
            if (textBuffer == null)
            {
                return null;
            }

            if (!textBuffer.Properties.TryGetProperty(typeof(ITextDocument), out ITextDocument textDocument))
            {
                return null;
            }

            string filePath = textDocument?.FilePath;

            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            try
            {
                if (!Path.IsPathRooted(filePath))
                {
                    return null;
                }
            }
            catch (ArgumentException)
            {
                return null;
            }

            return filePath;
        }

        private static async Task<bool> TrySetClipboardTextAsync(string text)
        {
            for (int attempt = 0; attempt < ClipboardRetryCount; attempt++)
            {
                try
                {
                    Clipboard.SetText(text);
                    return true;
                }
                catch (COMException)
                {
                    if (attempt == ClipboardRetryCount - 1)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                await Task.Delay(ClipboardRetryDelayMilliseconds);
            }

            return false;
        }

        private static string BuildStatusMessage(string displayPath, LineRange range)
        {
            string fileName = SafeGetFileName(displayPath);

            return range.StartLine == range.EndLine
                ? $"Copied {fileName}:{range.StartLine}"
                : $"Copied {fileName}:{range.StartLine}-{range.EndLine}";
        }

        private static string SafeGetFileName(string displayPath)
        {
            try
            {
                return Path.GetFileName(displayPath);
            }
            catch (ArgumentException)
            {
                return displayPath;
            }
        }
    }
}
