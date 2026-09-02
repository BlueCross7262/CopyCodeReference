global using Task = System.Threading.Tasks.Task;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CopyCodeReference
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(GeneralOptionsPage), Vsix.Name, "General", 0, 0, true, SupportsProfiles = true)]
    [Guid(PackageGuids.CopyCodeReferenceString)]
    public sealed class CopyCodeReferencePackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();
        }
    }
}
