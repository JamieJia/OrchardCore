using System.Threading.Tasks;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Entities.DisplayManagement;
using OrchardCore.Environment.Shell;
using OrchardCore.Forms.Configuration;
using OrchardCore.Forms.ViewModels;
using OrchardCore.Settings;

namespace OrchardCore.Forms.Drivers
{
    public class NoCaptchaSettingsDisplay : SectionDisplayDriver<ISite, NoCaptchaSettings>
    {
        public const string GroupId = "nocaptcha";
        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;

        public NoCaptchaSettingsDisplay(IShellHost shellHost, ShellSettings shellSettings)
        {
            _shellHost = shellHost;
            _shellSettings = shellSettings;
        }

        public override IDisplayResult Edit(NoCaptchaSettings section, BuildEditorContext context)
        {
            return Initialize<NoCaptchaSettingsViewModel>("NoCaptchaSettings_Edit", model =>
                {
                    model.SiteKey = section.SiteKey;
                    model.SiteSecret = section.SiteSecret;
                })
                .Location("Content")
                .OnGroup(GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(NoCaptchaSettings section, IUpdateModel updater, string groupId)
        {
            if (groupId == GroupId)
            {
                var model = new NoCaptchaSettingsViewModel();

                if (await updater.TryUpdateModelAsync(model, Prefix))
                {

                    section.SiteKey = model.SiteKey?.Trim();
                    section.SiteSecret = model.SiteSecret?.Trim();

                    // Reload tenant to apply settings.
                    _shellHost.ReloadShellContext(_shellSettings);
                }
            }

            return Edit(section);
        }
    }
}
