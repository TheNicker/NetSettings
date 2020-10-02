using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NetSettings.Data;
using NetSettings.View;

namespace NetSettingsTest.Forms
{
    public class App : Application
    {
        private readonly DataView fView;
        private readonly DataProvider fData;
        private readonly DataViewParams fDataViewParams;
        private readonly SettingsForm fSettingsForm;

        public App()
        {
            fView = new DataView();
            const string SettingsFilePath = @"Resources\GuiTemplate.json";
            fData = new DataProvider(ItemTree.FromFile(SettingsFilePath));
            //Create manually view[1]
            //fDataViewParams = new DataViewParams
            //{
            //    guiProvider = new WinFormGuiProvider(),
            //    dataProvider = fData,
            //    container = userControl11,
            //    descriptionContainer = controlContainer1
            //};

            //Create view[2] with predefined 'SettingsForm' from the same data provider
            //fSettingsForm = new SettingsForm((DataProvider)fData, new DataViewParams(), new DataView());
            fSettingsForm = new SettingsForm(fData);
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
