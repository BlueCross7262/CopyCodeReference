using System.ComponentModel;
using System.Runtime.InteropServices;
using Community.VisualStudio.Toolkit;

namespace CopyCodeReference
{
    [ComVisible(true)]
    public class General : BaseOptionModel<General>
    {
        [Category("Copy Format")]
        [DisplayName("Location format")]
        [Description("How the file path and line numbers are written. Colon: Foo.cs:12 / Foo.cs:12-15. Parentheses: Foo.cs(12) / Foo.cs(12-15). GitHub: Foo.cs#L12 / Foo.cs#L12-L15.")]
        [DefaultValue(CodeReferenceFormat.Colon)]
        public CodeReferenceFormat Format { get; set; } = CodeReferenceFormat.Colon;
    }
}
