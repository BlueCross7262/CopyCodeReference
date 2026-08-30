using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace CopyCodeReference
{
    [Command(PackageIds.CopyCodeReferenceCommand)]
    internal sealed class CopyCodeReferenceCommand : BaseCommand<CopyCodeReferenceCommand>
    {
        private const int ClipboardRetryCount = 5;
        private const int ClipboardRetryDelayMilliseconds = 20;

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            try
            {
                await Package.JoinableTaskFactory.SwitchToMainThreadAsync();

                DocumentView documentView = await VS.Documents.GetActiveDocumentViewAsync();
                IWpfTextView textView = documentView?.TextView;

                if (textView == null || textView.IsClosed)
                {
                    return;
                }

                ITextSelection selection = textView.Selection;

                if (selection == null || selection.IsEmpty)
                {
                    return;
                }

                SnapshotSpan span = selection.StreamSelectionSpan.SnapshotSpan;

                if (span.IsEmpty)
                {
                    return;
                }

                string filePath = TryGetFilePath(textView.TextBuffer);

                if (filePath == null)
                {
                    return;
                }

                ITextSnapshot snapshot = span.Snapshot;

                LineRange range = LineRangeCalculator.Calculate(
                    span.Start.Position,
                    span.Length,
                    snapshot.Length,
                    position => snapshot.GetLineNumberFromPosition(position));

                string reference = CodeReferenceBuilder.Build(
                    filePath,
                    range.StartLine,
                    range.EndLine,
                    span.GetText());

                if (await TrySetClipboardTextAsync(reference))
                {
                    await VS.StatusBar.ShowMessageAsync(BuildStatusMessage(filePath, range));
                }
            }
            catch (Exception ex)
            {
                await ex.LogAsync();
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

        private static string BuildStatusMessage(string filePath, LineRange range)
        {
            string fileName = SafeGetFileName(filePath);

            return range.StartLine == range.EndLine
                ? $"Copied {fileName}:{range.StartLine}"
                : $"Copied {fileName}:{range.StartLine}-{range.EndLine}";
        }

        private static string SafeGetFileName(string filePath)
        {
            try
            {
                return Path.GetFileName(filePath);
            }
            catch (ArgumentException)
            {
                return filePath;
            }
        }
    }
}
