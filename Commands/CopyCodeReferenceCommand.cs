namespace CopyCodeReference
{
    [Command(PackageIds.CopyCodeReferenceCommand)]
    internal sealed class CopyCodeReferenceCommand : BaseCommand<CopyCodeReferenceCommand>
    {
        protected override Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            return CopyCodeReferenceExecutor.ExecuteAsync(false);
        }
    }
}
