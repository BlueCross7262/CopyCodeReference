namespace CopyCodeReference
{
    internal class CodeReferenceOptions
    {
        public CodeReferenceFormat Format { get; set; } = CodeReferenceFormat.Colon;

        public bool UseForwardSlash { get; set; }

        public MultiLineBody MultiLineBody { get; set; } = MultiLineBody.LocationOnly;
    }
}
