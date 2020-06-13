using NetSettings.View;
using System;
using System.Collections.Generic;
using System.Linq;
using NetSettingsCore.Common;

namespace NetSettings.Data
{
    public class DataProvider //: IDataProvider
    {
        public delegate void ItemChangedDelegate(ItemChangedArgs changedArgs);
        public event ItemChangedDelegate ItemChanged;// = delegate { };
        //private readonly ItemTree fRootTemplate;
        private Dictionary<string, ItemTree> fQualifiedNames;
        private Dictionary<string, object> fDataBinding;

        private List<DataView> fBoundViews;

        //internal ItemTree RootTemplate { get { return fRootTemplate; } }
        public ItemTree RootTemplate { get; }

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
            DataView dataview = fBoundViews.FirstOrDefault(x => x == aDataView);
            if (dataview == default(DataView))
            {
                fBoundViews.Add((DataView)aDataView);
            }
        }

        public void RemoveView(DataView aDataView)
        {
            if (aDataView != null)
                fBoundViews.Remove((DataView)aDataView);
        }

        private void NormalizeData()
        {
            Dictionary<string, object> normalizedValues = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> pair in fDataBinding)
            {
                object obj = fDataBinding[pair.Key];
                if (fQualifiedNames.ContainsKey(pair.Key))
                {
                    ItemHelpers.NormalizeItemData(fQualifiedNames[pair.Key], ref obj);
                    normalizedValues.Add(pair.Key, obj);
                }
            }

            foreach (KeyValuePair<string, object> pair in normalizedValues)
                fDataBinding[pair.Key] = normalizedValues[pair.Key];
        }



        public DataProvider(ItemTree aRoot)
        {
            RootTemplate = aRoot;
            fBoundViews = new List<DataView>();
            Initialize();
        }

        private void Initialize()
        {
            ItemHelpers.BuildQualifiedNames(RootTemplate, out fQualifiedNames);
            fDataBinding = GenerateDefaultOptionsSet();
        }
        public Dictionary<string, object> GenerateDefaultOptionsSet()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (KeyValuePair<string, ItemTree> pair in fQualifiedNames)
                result.Add(pair.Key, pair.Value.defaultValue);

            return result;
        }

        public void SetValue(ItemChangedArgs aArgs)
        {
            string name = aArgs.Key;
            object valueNew = aArgs.Val;

            object valueCurrent;
            fDataBinding.TryGetValue(name, out valueCurrent);

            if (!Object.Equals(valueNew, valueCurrent) || aArgs.ChangedMode == ItemChangedMode.UserConfirmed)
            {
                fDataBinding[name] = valueNew;
                ItemChanged(aArgs);
                UpdateViews(aArgs.sender);
            }
        }

        public void SynthesizeAllChanged()
        {
            foreach (string key in fQualifiedNames.Keys.ToList())
                ItemChanged(new ItemChangedArgs()
                {
                    ChangedMode = ItemChangedMode.Synthesized,
                    Key = key,
                    Val = GetValueOrDefault(key)
                });
        }


        private void UpdateViews(object exclude)
        {
            foreach (DataView view in fBoundViews)
                if (view != exclude)
                    view.RefreshViewFromData();

        }

        private object GetDefaultValue(string key)
        {
            ItemTree itemTree;
            if (fQualifiedNames.TryGetValue(key, out itemTree))
            {
                return itemTree.defaultValue;
            }
            return null;
        }

        public object GetValueOrDefault(string key)
        {
            object val;
            if (!fDataBinding.TryGetValue(key, out val))
                val = GetDefaultValue(key);
            return val;
        }

        public object GetValue(string key)
        {
            object val;
            fDataBinding.TryGetValue(key, out val);
            return val;
        }

        public T GetValue<T>(string key)
        {
            object val = null;
            fDataBinding.TryGetValue(key, out val);

            if (val == null)
                val = GetDefaultValue(key);


            if (val is T)
                return (T)val;
            else
                throw new Exception("Bad type or Default value is not set");

        }


    }
}
