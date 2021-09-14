using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;


namespace NetSettings.Data
{
    using NameToJsonObject = Dictionary<string, JProperty>;

    public class UserDataSerializer
    {
        static public void SaveToFile(ItemTree root, Dictionary<string, object> dataBindings,string filePath)
        {
            //Build json object from dataBindings
            var rootJsonObject = new JObject();
            NameToJsonObject mapObjects = new NameToJsonObject();
            root.FullName = "";
            BuildTree(root, rootJsonObject, mapObjects);

            //update json object with data in databindings.
            foreach (var pair in dataBindings)
            {
                if (dataBindings[pair.Key] == null)
                    mapObjects[pair.Key].Value = null;
                else
                    mapObjects[pair.Key].Value = JToken.FromObject(dataBindings[pair.Key]);
            }

            //Serialize json 
            string text = JsonConvert.SerializeObject(rootJsonObject, new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

            //write serialized json to disk
            File.WriteAllText(filePath, text);
        }


       public static void LoadDataTree(string address, Dictionary<string, object> data, JObject root)
       {
            foreach (var element in root)
            {
                string currentAddress = String.IsNullOrWhiteSpace(address) ? element.Key : address + ItemHelpers.QualifiedNameSeperator + element.Key;

                if (element.Value.Type == JTokenType.Object)
                    LoadDataTree(currentAddress, data, element.Value as JObject);
                else
                {
                    data.Add(currentAddress, element.Value.ToObject(typeof(Object)));
                }
            }
        }


       public static Dictionary<string, object> LoadFromFile(string filePath)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            JObject jsonTree = JObject.Parse(File.ReadAllText(filePath));
            LoadDataTree("", data, jsonTree);
            return data;
        }


        static private void BuildTree(ItemTree root, Newtonsoft.Json.Linq.JObject rootObject, NameToJsonObject mapObjects)
        {
            if (root.type == "root" || root.type == "menu")
            {
                if (root.type == "menu")
                {
                    JObject childObject = new JObject();
                    rootObject.Add(root.name, childObject);
                    rootObject = childObject;
                }
                    

                foreach (ItemTree subitem in root.subitems)
                    BuildTree(subitem, rootObject, mapObjects);
            }
            else
            {
                JProperty prop = new JProperty(root.name, null);
                rootObject.Add(prop);
                mapObjects.Add(root.FullName, prop);
            }
        }
    }
}
