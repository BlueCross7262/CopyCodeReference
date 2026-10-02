using System.Globalization;

namespace CopyCodeReference
{
    internal sealed class UiText
    {
        private const string KoreanLanguage = "ko";

        private static readonly UiText English = new UiText
        {
            PreviewHeader = "What gets copied",
            PreviewSingleLine = "One line selected",
            PreviewMultiLine = "Several lines selected",
            PreviewNoSelection = "Nothing selected",
            LocationFormatHeader = "Location format",
            FormatColon = "Colon",
            FormatParentheses = "Parentheses",
            FormatGitHub = "GitHub",
            LocationFormatNote = "A single-line selection adds the selected text after one space.",
            PathSeparatorHeader = "Path separator",
            ForwardSlash = "Use forward slashes (/) in paths",
            ForwardSlashNote = "Suits GitHub and Markdown. Backslashes in the selected text are kept.",
            MultiLineHeader = "Several lines selected",
            MultiLineLocationOnly = "Location only",
            MultiLineCode = "Location and the selected code",
            MultiLineFencedCode = "Location and the code in a Markdown fence",
            MultiLineNote = "The line break at the end of the selection is not copied.",
            EmptySelectionHeader = "Nothing selected",
            CaretLineText = "Also copy the text of the caret line",
            EmptySelectionNote = "The location of the line that holds the caret is copied.",
            CopiedStatusFormat = "Copied {0}"
        };

        private static readonly UiText Korean = new UiText
        {
            PreviewHeader = "복사 결과 미리보기",
            PreviewSingleLine = "한 줄을 선택했을 때",
            PreviewMultiLine = "여러 줄을 선택했을 때",
            PreviewNoSelection = "선택하지 않았을 때",
            LocationFormatHeader = "위치 표기",
            FormatColon = "콜론",
            FormatParentheses = "괄호",
            FormatGitHub = "GitHub",
            LocationFormatNote = "한 줄을 선택하면 위치 뒤에 공백 한 칸을 두고 선택한 텍스트를 붙입니다.",
            PathSeparatorHeader = "경로 구분자",
            ForwardSlash = "경로에 슬래시(/) 사용",
            ForwardSlashNote = "GitHub 와 Markdown 용입니다. 선택한 텍스트는 바꾸지 않습니다.",
            MultiLineHeader = "여러 줄을 선택했을 때",
            MultiLineLocationOnly = "위치만",
            MultiLineCode = "위치와 선택한 코드",
            MultiLineFencedCode = "위치와 Markdown 코드 펜스로 감싼 코드",
            MultiLineNote = "선택 영역 끝의 줄바꿈은 복사하지 않습니다.",
            EmptySelectionHeader = "선택하지 않았을 때",
            CaretLineText = "캐럿이 있는 줄의 텍스트도 복사",
            EmptySelectionNote = "캐럿이 있는 줄의 위치를 복사합니다.",
            CopiedStatusFormat = "복사함: {0}"
        };

        private UiText()
        {
        }

        public static UiText Current => For(CultureInfo.CurrentUICulture);

        public string PreviewHeader { get; private set; }

        public string PreviewSingleLine { get; private set; }

        public string PreviewMultiLine { get; private set; }

        public string PreviewNoSelection { get; private set; }

        public string LocationFormatHeader { get; private set; }

        public string FormatColon { get; private set; }

        public string FormatParentheses { get; private set; }

        public string FormatGitHub { get; private set; }

        public string LocationFormatNote { get; private set; }

        public string PathSeparatorHeader { get; private set; }

        public string ForwardSlash { get; private set; }

        public string ForwardSlashNote { get; private set; }

        public string MultiLineHeader { get; private set; }

        public string MultiLineLocationOnly { get; private set; }

        public string MultiLineCode { get; private set; }

        public string MultiLineFencedCode { get; private set; }

        public string MultiLineNote { get; private set; }

        public string EmptySelectionHeader { get; private set; }

        public string CaretLineText { get; private set; }

        public string EmptySelectionNote { get; private set; }

        public string CopiedStatusFormat { get; private set; }

        public static UiText For(CultureInfo culture)
        {
            return culture != null && culture.TwoLetterISOLanguageName == KoreanLanguage
                ? Korean
                : English;
        }

        public string CopiedStatus(string location)
        {
            return string.Format(CultureInfo.CurrentCulture, CopiedStatusFormat, location);
        }
    }
}
