namespace CopyCodeReference
{
    [Command(PackageIds.CopyCodeReferenceRelativeCommand)]
    internal sealed class CopyCodeReferenceRelativeCommand : BaseCommand<CopyCodeReferenceRelativeCommand>
    {
        protected override Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            return CopyCodeReferenceExecutor.ExecuteAsync(true);
        }
    }
}
