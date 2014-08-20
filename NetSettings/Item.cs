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

        
        public object this [string key]
        {
            get
            {

                RootVerify();
                if (QualifiedNames == null)
                    ItemTree.BuildQualifiedNames(this);

                ItemTree item;
                if (QualifiedNames.TryGetValue(key, out item))
                {
                    return item.currentValue;
                }
                else
                    return null;
                
                
            }
            set
            {
                RootVerify();


                if (QualifiedNames == null)
                    ItemTree.BuildQualifiedNames(this);

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
        public Dictionary<string, ItemTree> QualifiedNames;
        
        [NonSerialized]
        [JsonIgnore]
        public System.Windows.Forms.Control control;

        public void RefreshQualifiedNames()
        {
            if (QualifiedNames == null)
                ItemTree.BuildQualifiedNames(this);
        }


        public static ItemTree FromFile(string aFileName)
        {
            string text = File.ReadAllText(aFileName);
            ItemTree root = (ItemTree)Newtonsoft.Json.JsonConvert.DeserializeObject(text, typeof(ItemTree));
            return root;
        }
        
        private static void BuildQualifiedNames(Dictionary<string,ItemTree> aQualifiedNames, ItemTree item,ItemTree parent)
        {
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
                    BuildQualifiedNames(aQualifiedNames, subItem, item);

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
                aRoot.QualifiedNames = new Dictionary<string, ItemTree>(StringComparer.OrdinalIgnoreCase);
                BuildQualifiedNames(aRoot.QualifiedNames, aRoot, null);
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
          if (this.type == "color")
          {
              if (this.defaultvalue != null && this.defaultvalue is string)
                  this.defaultvalue = System.Drawing.ColorTranslator.FromHtml(this.defaultvalue as string);

              if (this.value != null && this.value is string)
                  this.value = System.Drawing.ColorTranslator.FromHtml(this.value as string);
                  //this.value = System.Drawing.Color.FromName(;
          }

          if (this.type == "number")
          {
              if (this.defaultvalue == null)
                  this.defaultvalue = 0.0;
              else
                  if (defaultvalue is Int64)
                      defaultvalue = (double)(Int64)defaultvalue;

          }
    }


      public List<string> GetSettingsNames()
      {
          RootVerify();

          if (QualifiedNames == null)
              BuildQualifiedNames(this);
          return QualifiedNames.Keys.ToList();
      }
    }

    
}
