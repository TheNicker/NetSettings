using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetSettings.Data
{
    [Serializable]
    public class ItemTree// : IItemTree
    {
        //[JsonProperty(Order = 1)]
        //Json fields
        public string type;
        public string name;
        public string displayName;
        public string description;
        public object defaultValue;
        public string values;
        public ItemTree[] subItems;
        [JsonIgnore]
        public string FullName;

        private void RootVerify()
        {
            if (this.type != "root")
                throw new Exception("Operation valid only for root item");
        }

        public static ItemTree FromFile(string aFileName)
        {
            var text = File.ReadAllText(aFileName);
            var root = (ItemTree)JsonConvert.DeserializeObject(text, typeof(ItemTree));
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
            ItemHelpers.NormalizeItemData(this, ref this.defaultValue);
        }
    }
}
