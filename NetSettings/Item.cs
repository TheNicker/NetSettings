using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class ControlsGroup
    {
        public System.Windows.Forms.Label label;
        public System.Windows.Forms.Control parentContainer;
        public System.Windows.Forms.Control control;
        public System.Windows.Forms.Button defaultButton;
    }

    [Serializable]
    public class ItemTree
    {
        //[JsonProperty(Order = 1)]
        //Json fields
        public string type;
        public string name;
        public string displayname;
        public string description;
        public object defaultvalue;
        public object value;
        public string values;
        public ItemTree[] subitems;


        private void RootVerify()
        {
            if (this.type != "root")
                throw new Exception("Operation valid only for root item");
        }

        //TODO: move this to another class, it should confirm to the view model paradigm
        //This class is the model MenuSettings is the view 
        [JsonIgnore]
        public bool IsVisible;

        [JsonIgnore]
        ItemTree root;
        
        public T GetValue<T>(string key)
        {
            object val = root[key];
            if (val != null)
            {
                if (val is T)
                    return (T)val;
                else
                    throw new Exception("Bad type");
            }
            throw new Exception("Default value is not set");
        }

        public object this [string key]
        {
            get
            {
                if (root.QualifiedNames == null)
                    ItemTree.BuildQualifiedNames(root);

                ItemTree item;
                if (root.QualifiedNames.TryGetValue(key, out item))
                {
                    return item.currentValue;
                }
                else
                    return null;
                
                
            }
            set
            {
                if (root.QualifiedNames == null)
                    ItemTree.BuildQualifiedNames(root);

                ItemTree item;
                if (QualifiedNames.TryGetValue(key, out item))
                {
                    item.value = value;
                }
            }
        }
        
        [JsonIgnore]
        public string FullName;
        [JsonIgnore]
        public object currentValue
        {
            get
            {
                return value != null ? value : defaultvalue;
            }
        }

        [JsonIgnore]
        public Dictionary<string, ItemTree> fQualifiedNames;

        [JsonIgnore]
        public Dictionary<string, ItemTree> QualifiedNames
        {
            get
            {
                return root.fQualifiedNames;
            }

            set
            {
                root.fQualifiedNames = value;
            }
        }
        
        [NonSerialized]
        [JsonIgnore]
        public ControlsGroup controlsGroup;

        public void RefreshQualifiedNames()
        {
            if (root.QualifiedNames == null)
                ItemTree.BuildQualifiedNames(root);
        }


        public static ItemTree FromFile(string aFileName)
        {
            string text = File.ReadAllText(aFileName);
            ItemTree root = (ItemTree)Newtonsoft.Json.JsonConvert.DeserializeObject(text, typeof(ItemTree));
            BuildQualifiedNames(root);
            return root;
        }
        
        private static void BuildQualifiedNames(ItemTree aRoot, Dictionary<string,ItemTree> aQualifiedNames, ItemTree item,ItemTree parent)
        {
            item.root = aRoot;
            ItemTree currentParent = parent == null || parent.type == "root" ? null : parent;
            if (item.type != "root")
            {
                if (currentParent == null)
                {
                    item.FullName = item.name;
                }
                else
                {
                    item.FullName = String.Format("{0}.{1}", currentParent.FullName, item.name);
                }
            }
            if (item.subitems != null)
                foreach (ItemTree subItem in item.subitems)
                {
                    BuildQualifiedNames(aRoot, aQualifiedNames, subItem, item);

                }

            if (item.FullName != null)
            {
                aQualifiedNames.Add(item.FullName,item);
            }
        }

        private static void BuildQualifiedNames(ItemTree aRoot)
        {
            if (aRoot.type == "root")
            {
                aRoot.fQualifiedNames = new Dictionary<string, ItemTree>(StringComparer.OrdinalIgnoreCase);
                BuildQualifiedNames(aRoot, aRoot.fQualifiedNames, aRoot, null);
            }
        }

        public void SetValue(object val)
        {
            this.value = val;
            if (DataProvider != null)
                DataProvider[FullName] = val;
        }

        [JsonIgnore]
        private Dictionary<string, object> fDataProvider;
        [JsonIgnore]
        public Dictionary<string, object> DataProvider
        {
            get
            {
                return root.fDataProvider;
            }
            set
            {
                root.fDataProvider = value;
                root.RefresOverrideData();
            }
        }

        private void SetAlltoDefault()
        {
            foreach (string key in QualifiedNames.Keys)
                QualifiedNames[key].ResetToDefault();

        }

        public Dictionary<string,object> GenerateDefaultOptionsSet()
        {
            Dictionary<string,object> result = new Dictionary<string,object>();
            foreach (KeyValuePair<string, ItemTree> pair in root.QualifiedNames)
                result.Add(pair.Key, pair.Value.defaultvalue);

            return result;
        }

        private void ResetToDefault()
        {
            value = defaultvalue != null ? defaultvalue : null;
        }

        private void RefresOverrideData()
        {
            if (DataProvider != null)
                foreach (string key in DataProvider.Keys)
                {
                    ItemTree item;

                    if (root.QualifiedNames.TryGetValue(key, out item))
                    {
                        item.value = DataProvider[key];
                        NormalizeItemValue(item);
                    }
                }
        }
      

        public void ToFile(string aFileName)
        {
            JsonSerializer d = JsonSerializer.Create();

            string text = JsonConvert.SerializeObject(this,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                }
                );

            File.WriteAllText(aFileName, text);
        }

        public ItemTree DeepClone(ItemTree obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (ItemTree)formatter.Deserialize(ms);
            }
        }

      [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        NormalizeItemValue(this);
    }

      private void NormalizeItemValue(ItemTree aItem)
      {
          if (aItem.type == "color")
          {
              if (aItem.defaultvalue != null && aItem.defaultvalue is string)
                  aItem.defaultvalue = System.Drawing.ColorTranslator.FromHtml(aItem.defaultvalue as string);

              if (aItem.value != null && aItem.value is string)
                  aItem.value = System.Drawing.ColorTranslator.FromHtml(aItem.value as string);
          }

          if (aItem.type == "number")
          {
              if (aItem.defaultvalue == null)
                  aItem.defaultvalue = 0.0;
              else
                  if (defaultvalue is Int64)
                      defaultvalue = (double)(Int64)defaultvalue;

          }
      }


      public List<string> GetSettingsNames()
      {
          //RootVerify();

          if (root.QualifiedNames == null)
              BuildQualifiedNames(root);
          return root.QualifiedNames.Keys.ToList();
      }
    }

    
}
