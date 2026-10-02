using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CopyCodeReference
{
    public class GeneralOptionsControl : UserControl
    {
        private const string FormatGroupName = "CopyCodeReferenceFormat";
        private const string MultiLineGroupName = "CopyCodeReferenceMultiLine";
        private const double WideLayoutMinWidth = 640;
        private const double PreviewColumnWidth = 340;
        private const double ColumnGap = 28;
        private const double FormatNameWidth = 96;
        private const double SectionGap = 18;

        private static readonly FontFamily MonospaceFont = new FontFamily("Cascadia Mono, Consolas, Courier New");

        private readonly UiText _text = UiText.Current;
        private readonly RadioButton _colon;
        private readonly RadioButton _parentheses;
        private readonly RadioButton _gitHub;
        private readonly CheckBox _forwardSlash;
        private readonly RadioButton _locationOnly;
        private readonly RadioButton _code;
        private readonly RadioButton _fencedCode;
        private readonly CheckBox _caretLine;
        private readonly TextBlock _singleLinePreview;
        private readonly TextBlock _multiLinePreview;
        private readonly TextBlock _emptySelectionPreview;
        private readonly FrameworkElement _settingsPanel;
        private readonly FrameworkElement _previewPanel;
        private readonly Grid _root;
        private bool? _isWideLayout;

        public GeneralOptionsControl()
        {
            _colon = CreateFormatRadioButton(_text.FormatColon, "Foo.cs:12", "Foo.cs:12-15");
            _parentheses = CreateFormatRadioButton(_text.FormatParentheses, "Foo.cs(12)", "Foo.cs(12-15)");
            _gitHub = CreateFormatRadioButton(_text.FormatGitHub, "Foo.cs#L12", "Foo.cs#L12-L15");
            _forwardSlash = CreateCheckBox(_text.ForwardSlash);
            _locationOnly = CreateRadioButton(MultiLineGroupName, _text.MultiLineLocationOnly);
            _code = CreateRadioButton(MultiLineGroupName, _text.MultiLineCode);
            _fencedCode = CreateRadioButton(MultiLineGroupName, _text.MultiLineFencedCode);
            _caretLine = CreateCheckBox(_text.CaretLineText);

            _singleLinePreview = CreatePreviewValue();
            _multiLinePreview = CreatePreviewValue();
            _emptySelectionPreview = CreatePreviewValue();

            _settingsPanel = CreateSettingsPanel();
            _previewPanel = CreatePreviewPanel();

            _root = new Grid { Margin = new Thickness(12, 10, 12, 12) };
            _root.Children.Add(_settingsPanel);
            _root.Children.Add(_previewPanel);

            Content = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = _root
            };

            Format = CodeReferenceFormat.Colon;
            MultiLineBody = MultiLineBody.LocationOnly;

            ApplyLayout(true);
            SizeChanged += (sender, e) => ApplyLayout(e.NewSize.Width >= WideLayoutMinWidth);
            UpdatePreview();
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

        private FrameworkElement CreateSettingsPanel()
        {
            StackPanel panel = new StackPanel();

            AddSection(panel, _text.LocationFormatHeader, false, _colon, _parentheses, _gitHub, CreateNote(_text.LocationFormatNote));
            AddSection(panel, _text.PathSeparatorHeader, true, _forwardSlash, CreateNote(_text.ForwardSlashNote));
            AddSection(panel, _text.MultiLineHeader, true, _locationOnly, _code, _fencedCode, CreateNote(_text.MultiLineNote));
            AddSection(panel, _text.EmptySelectionHeader, true, CreateNote(_text.EmptySelectionNote, 0, 4), _caretLine);

            return panel;
        }

        private FrameworkElement CreatePreviewPanel()
        {
            StackPanel rows = new StackPanel { Margin = new Thickness(12, 10, 12, 2) };
            rows.Children.Add(CreatePreviewRow(_text.PreviewSingleLine, _singleLinePreview));
            rows.Children.Add(CreatePreviewRow(_text.PreviewMultiLine, _multiLinePreview));
            rows.Children.Add(CreatePreviewRow(_text.PreviewNoSelection, _emptySelectionPreview));

            Grid frame = new Grid();
            frame.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3) });
            frame.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Border accent = new Border { Background = SystemColors.HighlightBrush };
            Grid.SetColumn(accent, 0);
            Grid.SetColumn(rows, 1);
            frame.Children.Add(accent);
            frame.Children.Add(rows);

            Border box = new Border
            {
                Background = SystemColors.WindowBrush,
                BorderBrush = SystemColors.ControlDarkBrush,
                BorderThickness = new Thickness(0, 1, 1, 1),
                Child = frame
            };

            StackPanel panel = new StackPanel();
            panel.Children.Add(CreateHeader(_text.PreviewHeader, false));
            panel.Children.Add(box);
            return panel;
        }

        private void ApplyLayout(bool wide)
        {
            if (_isWideLayout == wide)
            {
                return;
            }

            _isWideLayout = wide;
            _root.ColumnDefinitions.Clear();
            _root.RowDefinitions.Clear();

            if (wide)
            {
                _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                _root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(PreviewColumnWidth) });
                Place(_settingsPanel, 0, 0, new Thickness(0, 0, ColumnGap, 0));
                Place(_previewPanel, 0, 1, new Thickness(0));
                return;
            }

            _root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            _root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Place(_previewPanel, 0, 0, new Thickness(0, 0, 0, SectionGap + 4));
            Place(_settingsPanel, 1, 0, new Thickness(0));
        }

        private void UpdatePreview()
        {
            CodeReferenceOptions options = new CodeReferenceOptions
            {
                Format = Format,
                UseForwardSlash = UseForwardSlash,
                MultiLineBody = MultiLineBody
            };

            _singleLinePreview.Text = OptionsPreview.SingleLine(options);
            _multiLinePreview.Text = OptionsPreview.MultiLine(options);
            _emptySelectionPreview.Text = OptionsPreview.EmptySelection(options, CopyCaretLineWhenNoSelection);
        }

        private RadioButton CreateFormatRadioButton(string name, string singleLineSample, string rangeSample)
        {
            Grid content = new Grid();
            content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(FormatNameWidth) });
            content.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            TextBlock nameText = new TextBlock { Text = name };
            TextBlock sampleText = new TextBlock
            {
                Text = singleLineSample + "   " + rangeSample,
                FontFamily = MonospaceFont,
                Foreground = SystemColors.GrayTextBrush
            };

            Grid.SetColumn(nameText, 0);
            Grid.SetColumn(sampleText, 1);
            content.Children.Add(nameText);
            content.Children.Add(sampleText);

            RadioButton button = CreateRadioButton(FormatGroupName, null);
            button.Content = content;
            return button;
        }

        private RadioButton CreateRadioButton(string groupName, string label)
        {
            RadioButton button = new RadioButton
            {
                GroupName = groupName,
                Content = label,
                Margin = new Thickness(0, 3, 0, 3),
                VerticalContentAlignment = VerticalAlignment.Center
            };

            button.Checked += (sender, e) => UpdatePreview();
            return button;
        }

        private CheckBox CreateCheckBox(string label)
        {
            CheckBox box = new CheckBox
            {
                Content = label,
                Margin = new Thickness(0, 3, 0, 3),
                VerticalContentAlignment = VerticalAlignment.Center
            };

            box.Checked += (sender, e) => UpdatePreview();
            box.Unchecked += (sender, e) => UpdatePreview();
            return box;
        }

        private static void AddSection(Panel panel, string header, bool spaced, params UIElement[] items)
        {
            panel.Children.Add(CreateHeader(header, spaced));

            foreach (UIElement item in items)
            {
                panel.Children.Add(item);
            }
        }

        private static void Place(FrameworkElement element, int row, int column, Thickness margin)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            element.Margin = margin;
        }

        private static FrameworkElement CreatePreviewRow(string label, TextBlock value)
        {
            StackPanel row = new StackPanel { Margin = new Thickness(0, 0, 0, 10) };
            row.Children.Add(new TextBlock
            {
                Text = label,
                Foreground = SystemColors.GrayTextBrush,
                Margin = new Thickness(0, 0, 0, 3)
            });
            row.Children.Add(value);
            return row;
        }

        private static TextBlock CreatePreviewValue()
        {
            return new TextBlock
            {
                FontFamily = MonospaceFont,
                Foreground = SystemColors.WindowTextBrush,
                TextWrapping = TextWrapping.Wrap
            };
        }

        private static TextBlock CreateHeader(string text, bool spaced)
        {
            return new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, spaced ? SectionGap : 0, 0, 6)
            };
        }

        private static TextBlock CreateNote(string text)
        {
            return CreateNote(text, 4, 0);
        }

        private static TextBlock CreateNote(string text, double top, double bottom)
        {
            return new TextBlock
            {
                Text = text,
                TextWrapping = TextWrapping.Wrap,
                Foreground = SystemColors.GrayTextBrush,
                Margin = new Thickness(0, top, 0, bottom)
            };
        }
    }
}
