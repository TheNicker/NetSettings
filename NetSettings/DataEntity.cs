using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class DataEntity
    {
        public delegate void ItemChangedDelegate(string key, object val);
        public event ItemChangedDelegate ItemChanged = delegate { };
        public ItemTree fRootTemplate;
        public Dictionary<string, ItemTree> fQualifiedNames;
        private Dictionary<string, object> fDataProvider;

        public Dictionary<string, object> DataProvider
        {
            get
            {
                return fDataProvider;
            }
            set 
            {
                fDataProvider = value;
                NormalizeData();
            }
        }

        private void NormalizeData()
        {
            Dictionary<string, object> normalizedValues = new Dictionary<string, object>();
            foreach(KeyValuePair<string,object> pair in fDataProvider )
            {
                object obj = fDataProvider[pair.Key];
                ItemHelpers.NormalizeItemData(fQualifiedNames[pair.Key], ref obj);
                normalizedValues.Add(pair.Key, obj);
            }

            foreach (KeyValuePair<string, object> pair in normalizedValues)
                fDataProvider[pair.Key] = normalizedValues[pair.Key];
        }

        
        
        public DataEntity(ItemTree aRoot)
        {
            fRootTemplate = aRoot;
            Initialize();
        }

        private void Initialize()
        {
            ItemHelpers.BuildQualifiedNames(fRootTemplate, out fQualifiedNames);
            fDataProvider = GenerateDefaultOptionsSet();
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
            fDataProvider[name] = val;
            ItemChanged(name,val);
        }

        public object GetValue(string key)
        {
            object val = fDataProvider[key];
            if (val == null)
                val = fQualifiedNames[key].defaultvalue;
            return val;
        }

        public T GetValue<T>(string key)
        {
            object val = fDataProvider[key];
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
