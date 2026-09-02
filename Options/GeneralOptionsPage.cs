using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.Shell;

namespace CopyCodeReference
{
    [ComVisible(true)]
    [Guid(PageGuidString)]
    public class GeneralOptionsPage : UIElementDialogPage
    {
        public const string PageGuidString = "5ec4dac6-7f28-4bf3-a5d3-b0fe84f44688";

        private GeneralOptionsControl _control;

        protected override UIElement Child
        {
            get
            {
                if (_control == null)
                {
                    _control = new GeneralOptionsControl();
                    PushModelToControl();
                }

                return _control;
            }
        }

        public override object AutomationObject => General.Instance;

        public override void LoadSettingsFromStorage()
        {
            General.Instance.Load();
            PushModelToControl();
        }

        public override void SaveSettingsToStorage()
        {
            PullControlToModel();
            General.Instance.Save();
        }

        public override void ResetSettings()
        {
            General.Instance.Format = CodeReferenceFormat.Colon;
            General.Instance.UseForwardSlash = false;
            General.Instance.MultiLineBody = MultiLineBody.LocationOnly;
            General.Instance.CopyCaretLineWhenNoSelection = false;
            PushModelToControl();
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);
            PushModelToControl();
        }

        private void PushModelToControl()
        {
            if (_control != null)
            {
                _control.Format = General.Instance.Format;
                _control.UseForwardSlash = General.Instance.UseForwardSlash;
                _control.MultiLineBody = General.Instance.MultiLineBody;
                _control.CopyCaretLineWhenNoSelection = General.Instance.CopyCaretLineWhenNoSelection;
            }
        }

        private void PullControlToModel()
        {
            if (_control != null)
            {
                General.Instance.Format = _control.Format;
                General.Instance.UseForwardSlash = _control.UseForwardSlash;
                General.Instance.MultiLineBody = _control.MultiLineBody;
                General.Instance.CopyCaretLineWhenNoSelection = _control.CopyCaretLineWhenNoSelection;
            }
        }
    }
}
