using NetSettings.Data;
using NetSettings.Forms;

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
      
        static OnItemChangedDelegate OnItemChanged;

        public static void Initialize(CreateParams createParams)
        {
            OnItemChanged = createParams.callback;
            var dataProvider = new DataProvider(ItemTree.FromFile(createParams.templateFilePath));
            dataProvider.ItemChanged += DataProvider_ItemChanged;
            if (form == null)
            {
                form = new SettingsForm(dataProvider);
            }
            
        }

        public static void SetVisible(bool visible)
        {
            form.Visible = visible;
        }

        private static void DataProvider_ItemChanged(NetSettings.Data.ItemChangedArgs changedArgs)
        {
            ItemChangedEventArgs args;
            args.key = changedArgs.Key;
            args.value = changedArgs.Val.ToString();
            args.mode = changedArgs.ChangedMode;
            args.type = changedArgs.type;
            OnItemChanged(args);
            
        }
        static void Visible(bool show)
        {
            form.Visible = show;
        }
    }
}
