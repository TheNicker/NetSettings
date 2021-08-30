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

namespace NetSettings.Data
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
        public string values;
        public ItemTree[] subitems;
        [JsonIgnore]
        public string FullName;

        private void RootVerify()
        {
            if (this.type != "root")
                throw new Exception("Operation valid only for root item");
        }

        public static ItemTree FromFile(string aFileName)
        {
            string text = File.ReadAllText(aFileName);
            ItemTree root = (ItemTree)Newtonsoft.Json.JsonConvert.DeserializeObject(text, typeof(ItemTree));
            return root;
        }

        public void ToFile(string aFileName)
        {

            string text = JsonConvert.SerializeObject(this,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                }
                );

            File.WriteAllText(aFileName, text);
        }

      [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        ItemHelpers.NormalizeItemData(this, ref this.defaultvalue);
    }

   

    }

    
}
