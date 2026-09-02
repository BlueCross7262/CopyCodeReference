using System.ComponentModel;
using System.Runtime.InteropServices;
using Community.VisualStudio.Toolkit;

namespace CopyCodeReference
{
    public class General : BaseOptionModel<General>
    {
        [Category("Copy Format")]
        [DisplayName("Location format")]
        [Description("How the file path and line numbers are written. Colon: Foo.cs:12 / Foo.cs:12-15. Parentheses: Foo.cs(12) / Foo.cs(12-15). GitHub: Foo.cs#L12 / Foo.cs#L12-L15.")]
        [DefaultValue(CodeReferenceFormat.Colon)]
        public CodeReferenceFormat Format { get; set; } = CodeReferenceFormat.Colon;

        [Category("Copy Format")]
        [DisplayName("Use forward slashes in paths")]
        [Description("Writes the path with / instead of \\, which matches how paths are written on GitHub and in Markdown. The selected text is never changed.")]
        [DefaultValue(false)]
        public bool UseForwardSlash { get; set; }

        [Category("Copy Format")]
        [DisplayName("Multi-line selections")]
        [Description("What a multi-line selection copies. LocationOnly: the location line only. Code: the location line and the selected code. FencedCode: the location line and the selected code inside a Markdown fence.")]
        [DefaultValue(MultiLineBody.LocationOnly)]
        public MultiLineBody MultiLineBody { get; set; } = MultiLineBody.LocationOnly;

        [Category("Copy Format")]
        [DisplayName("Copy the caret line when nothing is selected")]
        [Description("Copies the line that holds the caret when there is no selection. When this is off the command does nothing and leaves the clipboard untouched.")]
        [DefaultValue(false)]
        public bool CopyCaretLineWhenNoSelection { get; set; }
    }
}
