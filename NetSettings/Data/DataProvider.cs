using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class DataProvider
    {
        public delegate void ItemChangedDelegate(string key, object val);
        public event ItemChangedDelegate ItemChanged = delegate { };
        public ItemTree fRootTemplate;
        public Dictionary<string, ItemTree> fQualifiedNames;
        private Dictionary<string, object> fDataBinding;

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

        public void SetValue(string name, object val)
        {
            object currentObject = fDataBinding[name];
            if (!val.Equals(currentObject))
            {
                fDataBinding[name] = val;
                ItemChanged(name, val);
            }
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
