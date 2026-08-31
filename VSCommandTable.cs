namespace CopyCodeReference
{
    using System;

    internal sealed partial class PackageGuids
    {
        public const string CopyCodeReferenceString = "cf0867e8-3f40-427f-9f65-d4da570b0145";
        public static Guid CopyCodeReference = new Guid(CopyCodeReferenceString);
    }

    internal sealed partial class PackageIds
    {
        public const int CopyCodeReferenceGroup = 0x0001;
        public const int CopyCodeReferenceContextGroup = 0x0002;
        public const int CopyCodeReferenceCommand = 0x0100;
        public const int CopyCodeReferenceRelativeCommand = 0x0200;
    }
}
