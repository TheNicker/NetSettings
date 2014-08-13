using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class Item
    {
        //Json fields
        public string type;
        public Item[] subitems;
        public string name;
        public string displayname;
        public string description;
        public object defaultvalue;
        public object value;

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

        public static Item FromFile(string aFileName)
        {
            string text = File.ReadAllText(aFileName);
            return (Item)Newtonsoft.Json.JsonConvert.DeserializeObject(text, typeof(Item));
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(rootItem, Newtonsoft.Json.Formatting.Indented);

        }
    }
}
