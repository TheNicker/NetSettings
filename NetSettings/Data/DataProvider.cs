using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class DataProvider
    {
        public delegate void ItemChangedDelegate(ItemChangedArgs changedArgs);
        public event ItemChangedDelegate ItemChanged = delegate { };
        public ItemTree fRootTemplate;
        public Dictionary<string, ItemTree> fQualifiedNames;
        private Dictionary<string, object> fDataBinding;

        private List<DataView> fBoundViews;

        public Dictionary<string, object> DataBinding
        {
            get
            {
                return fDataBinding;
            }
            set 
            {
                fDataBinding = value;
                NormalizeData();
            }
        }

        public void AddView(DataView aDataView)
        {
             DataView dataview =  fBoundViews.FirstOrDefault( x=> x == aDataView);
            if (dataview == default(DataView))
            {
                fBoundViews.Add(aDataView);
            }
        }

        public void RemoveView(DataView aDataView)
        {
            if (aDataView != null)
                fBoundViews.Remove(aDataView);
            
        }


        private void NormalizeData()
        {
            Dictionary<string, object> normalizedValues = new Dictionary<string, object>();
            foreach(KeyValuePair<string,object> pair in fDataBinding )
            {
                object obj = fDataBinding[pair.Key];
                ItemHelpers.NormalizeItemData(fQualifiedNames[pair.Key], ref obj);
                normalizedValues.Add(pair.Key, obj);
            }

            foreach (KeyValuePair<string, object> pair in normalizedValues)
                fDataBinding[pair.Key] = normalizedValues[pair.Key];
        }

        
        
        public DataProvider(ItemTree aRoot)
        {
            fRootTemplate = aRoot;
            fBoundViews = new List<DataView>();
            Initialize();
        }

        private void Initialize()
        {
            ItemHelpers.BuildQualifiedNames(fRootTemplate, out fQualifiedNames);
            fDataBinding = GenerateDefaultOptionsSet();
        }
        public Dictionary<string, object> GenerateDefaultOptionsSet()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (KeyValuePair<string, ItemTree> pair in fQualifiedNames)
                result.Add(pair.Key, pair.Value.defaultvalue);

            return result;
        }

        public void SetValue(ItemChangedArgs aArgs)
        {
            string name = aArgs.Key;
            object val = aArgs.Val;
            object currentObject = fDataBinding[name];
            if (!val.Equals(currentObject) || aArgs.ChangedMode == ItemChangedMode.UserConfirmed)
            {
                fDataBinding[name] = val;
                ItemChanged(aArgs);
                UpdateViews(aArgs.sender);
            }
        }

        private void UpdateViews(object exclude)
        {
            foreach (DataView view in fBoundViews)
                if (view != exclude)
                    view.RefreshViewFromData();
            
        }

        public object GetValue(string key)
        {
            object val = fDataBinding[key];
            if (val == null)
                val = fQualifiedNames[key].defaultvalue;
            return val;
        }

        public T GetValue<T>(string key)
        {
            object val = fDataBinding[key];
            if (val != null)
            {
                if (val is T)
                    return (T)val;
                else
                    throw new Exception("Bad type");
            }
            throw new Exception("Default value is not set");
        }


    }
}
