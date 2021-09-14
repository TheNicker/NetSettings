using NetSettings.Data;
using NetSettings.Forms;
using System.Collections.Generic;
using System.IO;

namespace GuiProxy
{
    public class GuiProvider
    {

        public struct ItemChangedEventArgs
        {
            public string key;
            public string value;
            public string type;
            public NetSettings.Data.ItemChangedMode mode;
        };

        public delegate void OnItemChangedDelegate(ItemChangedEventArgs args);

        public class CreateParams
        {
            public string templateFilePath;
            public string userSettingsFilePath;
            public OnItemChangedDelegate callback;
        }

        static SettingsForm form;
        static Dictionary<string, object> fData;
        private static CreateParams fCreateParams;
        static OnItemChangedDelegate OnItemChanged;
        static DataProvider fDataProvider;

        public static void Initialize(CreateParams createParams)
        {
            fCreateParams = createParams;
            OnItemChanged = createParams.callback;
            fDataProvider = new DataProvider(ItemTree.FromFile(createParams.templateFilePath));
            if (File.Exists(fCreateParams.userSettingsFilePath))
                fData = UserDataSerializer.LoadFromFile(fCreateParams.userSettingsFilePath);
            else 
                fData = fDataProvider.GenerateDefaultOptionsSet();

            fDataProvider.ItemChanged += DataProvider_ItemChanged;
            fDataProvider.DataBinding = fData;
            if (form == null)
            {
                form = new SettingsForm(fDataProvider);
                form.OnSave += Form_OnSave;
            }
            
        }

        private static void SaveUserSettingsFile()
        {
            UserDataSerializer.SaveToFile(fDataProvider.RootTemplate, fData, fCreateParams.userSettingsFilePath);
        }

        private static void Form_OnSave()
        {
            SaveUserSettingsFile();
        }

        public static void SetVisible(bool visible)
        {
            //if form is already visible - bring to front
            if (form.Visible == true && visible == true)
            {
                form.BringToFront();
            }
            else
            {
                form.Visible = visible;
            }
        }
        public static void SaveUserSettings()
        {
            SaveUserSettingsFile();
        }

        private static void DataProvider_ItemChanged(NetSettings.Data.ItemChangedArgs changedArgs)
        {
            ItemChangedEventArgs args;
            args.key = changedArgs.Key;
            args.value = changedArgs.Val != null ? changedArgs.Val.ToString().ToLower() : "";
            args.mode = changedArgs.ChangedMode;
            args.type = changedArgs.type;
            OnItemChanged(args);
        }
    }
}
