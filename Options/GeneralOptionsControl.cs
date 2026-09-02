using System.Windows;
using System.Windows.Controls;

namespace CopyCodeReference
{
    public class GeneralOptionsControl : UserControl
    {
        private const string GroupName = "CopyCodeReferenceFormat";

        private readonly RadioButton _colon;
        private readonly RadioButton _parentheses;
        private readonly RadioButton _gitHub;

        public GeneralOptionsControl()
        {
            _colon = CreateRadioButton("Colon    Foo.cs:12    Foo.cs:12-15");
            _parentheses = CreateRadioButton("Parentheses    Foo.cs(12)    Foo.cs(12-15)");
            _gitHub = CreateRadioButton("GitHub    Foo.cs#L12    Foo.cs#L12-L15");

            StackPanel panel = new StackPanel
            {
                Margin = new Thickness(8)
            };
            panel.Children.Add(new TextBlock
            {
                Text = "Location format",
                Margin = new Thickness(0, 0, 0, 6)
            });
            panel.Children.Add(_colon);
            panel.Children.Add(_parentheses);
            panel.Children.Add(_gitHub);
            panel.Children.Add(new TextBlock
            {
                Text = "A single-line selection appends the selected text after one space. A multi-line selection copies the location only.",
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 10, 0, 0)
            });

            Content = panel;
            Format = CodeReferenceFormat.Colon;
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

        private static RadioButton CreateRadioButton(string label)
        {
            return new RadioButton
            {
                GroupName = GroupName,
                Content = label,
                Margin = new Thickness(0, 2, 0, 2)
            };
        }
    }
}
