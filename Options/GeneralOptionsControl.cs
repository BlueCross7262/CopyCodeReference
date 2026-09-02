using System.Windows;
using System.Windows.Controls;

namespace CopyCodeReference
{
    public class GeneralOptionsControl : UserControl
    {
        private const string FormatGroupName = "CopyCodeReferenceFormat";
        private const string MultiLineGroupName = "CopyCodeReferenceMultiLine";

        private readonly RadioButton _colon;
        private readonly RadioButton _parentheses;
        private readonly RadioButton _gitHub;
        private readonly CheckBox _forwardSlash;
        private readonly RadioButton _locationOnly;
        private readonly RadioButton _code;
        private readonly RadioButton _fencedCode;
        private readonly CheckBox _caretLine;

        public GeneralOptionsControl()
        {
            _colon = CreateRadioButton(FormatGroupName, "Colon    Foo.cs:12    Foo.cs:12-15");
            _parentheses = CreateRadioButton(FormatGroupName, "Parentheses    Foo.cs(12)    Foo.cs(12-15)");
            _gitHub = CreateRadioButton(FormatGroupName, "GitHub    Foo.cs#L12    Foo.cs#L12-L15");

            _forwardSlash = CreateCheckBox("Use forward slashes in paths    D:/Project/Foo.cs");

            _locationOnly = CreateRadioButton(MultiLineGroupName, "Location only");
            _code = CreateRadioButton(MultiLineGroupName, "Location and the selected code");
            _fencedCode = CreateRadioButton(MultiLineGroupName, "Location and the selected code in a Markdown fence");

            _caretLine = CreateCheckBox("Copy the caret line when nothing is selected");

            StackPanel panel = new StackPanel
            {
                Margin = new Thickness(8)
            };

            panel.Children.Add(CreateHeader("Location format", false));
            panel.Children.Add(_colon);
            panel.Children.Add(_parentheses);
            panel.Children.Add(_gitHub);
            panel.Children.Add(CreateNote("A single-line selection appends the selected text after one space."));

            panel.Children.Add(CreateHeader("Path separator", true));
            panel.Children.Add(_forwardSlash);

            panel.Children.Add(CreateHeader("Multi-line selections", true));
            panel.Children.Add(_locationOnly);
            panel.Children.Add(_code);
            panel.Children.Add(_fencedCode);
            panel.Children.Add(CreateNote("A trailing line break is removed from the copied code. Single-line selections are not affected by this setting."));

            panel.Children.Add(CreateHeader("Empty selection", true));
            panel.Children.Add(_caretLine);

            Content = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = panel
            };

            Format = CodeReferenceFormat.Colon;
            MultiLineBody = MultiLineBody.LocationOnly;
        }

        public CodeReferenceFormat Format
        {
            get
            {
                if (_parentheses.IsChecked == true)
                {
                    return CodeReferenceFormat.Parentheses;
                }

                if (_gitHub.IsChecked == true)
                {
                    return CodeReferenceFormat.GitHub;
                }

                return CodeReferenceFormat.Colon;
            }
            set
            {
                _colon.IsChecked = value == CodeReferenceFormat.Colon;
                _parentheses.IsChecked = value == CodeReferenceFormat.Parentheses;
                _gitHub.IsChecked = value == CodeReferenceFormat.GitHub;
            }
        }

        public bool UseForwardSlash
        {
            get { return _forwardSlash.IsChecked == true; }
            set { _forwardSlash.IsChecked = value; }
        }

        public MultiLineBody MultiLineBody
        {
            get
            {
                if (_code.IsChecked == true)
                {
                    return MultiLineBody.Code;
                }

                if (_fencedCode.IsChecked == true)
                {
                    return MultiLineBody.FencedCode;
                }

                return MultiLineBody.LocationOnly;
            }
            set
            {
                _locationOnly.IsChecked = value == MultiLineBody.LocationOnly;
                _code.IsChecked = value == MultiLineBody.Code;
                _fencedCode.IsChecked = value == MultiLineBody.FencedCode;
            }
        }

        public bool CopyCaretLineWhenNoSelection
        {
            get { return _caretLine.IsChecked == true; }
            set { _caretLine.IsChecked = value; }
        }

        private static RadioButton CreateRadioButton(string groupName, string label)
        {
            return new RadioButton
            {
                GroupName = groupName,
                Content = label,
                Margin = new Thickness(0, 2, 0, 2)
            };
        }

        private static CheckBox CreateCheckBox(string label)
        {
            return new CheckBox
            {
                Content = label,
                Margin = new Thickness(0, 2, 0, 2)
            };
        }

        private static TextBlock CreateHeader(string text, bool spaced)
        {
            return new TextBlock
            {
                Text = text,
                Margin = new Thickness(0, spaced ? 14 : 0, 0, 6)
            };
        }

        private static TextBlock CreateNote(string text)
        {
            return new TextBlock
            {
                Text = text,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 6, 0, 0)
            };
        }
    }
}
