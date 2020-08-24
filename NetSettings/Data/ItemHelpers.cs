using NetSettings.Data;
using System;
using System.Collections.Generic;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetSettings
{
    internal class ItemHelpers
    {
        public static void BuildQualifiedNames(Dictionary<string, ItemTree> aQualifiedNames, ItemTree item, ItemTree parent)//TODO: Can this be changed to private?
        {
            var currentParent = (parent == null || parent.type == "root") ? null : parent;
            if (item.type != "root")
            {
                item.FullName = currentParent == null ? item.name : $"{currentParent.FullName}.{item.name}";
            }

            if (item.subItems != null)
            {
                foreach (var subItem in item.subItems)
                {
                    BuildQualifiedNames(aQualifiedNames, subItem, item);
                }
            }

            if (item.FullName != null)
            {
                aQualifiedNames.Add(item.FullName, item);
            }
        }

        public static void BuildQualifiedNames(ItemTree aRoot, out Dictionary<string, ItemTree> aQualifiedNames)
        {
            aQualifiedNames = null;
            if (aRoot.type == "root")
            {
                aQualifiedNames = new Dictionary<string, ItemTree>(StringComparer.OrdinalIgnoreCase);
                BuildQualifiedNames(aQualifiedNames, aRoot, null);
            }
        }

        //public static void NormalizeItemValue(ItemTree aItem)
        //{

        //}

        internal static void NormalizeItemData(ItemTree aItem, ref object obj) //TODO: Can the object be changed to Color?
        {
            switch (aItem.type)
            {
                case "color":
                    //TODO: Why do we need this culture lines?
                    var lastCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                    try
                    {
                        if (obj is string htmlColor)
                        {
                            obj = Color.FromHtml(htmlColor);
                        }
                        else if (!(obj is Color))
                        {
                            var color = ((JObject) obj);
                            //TODO: Choose how to handle a null case
                            var r = (color.GetValue("R") ?? throw new InvalidOperationException()).Value<byte>();
                            var g = color.GetValue("G")!.Value<byte>();
                            var b = color.GetValue("B")!.Value<byte>();
                            obj = Color.FromArgb(r,g,b);
                        }
                    }
                    finally
                    {
                        System.Threading.Thread.CurrentThread.CurrentCulture = lastCulture; //TODO: What is the purpose of this line?
                    }
                    break;
                case "number":
                    //normalize all our numbers to double data type
                    if (obj != null && obj is long num)
                    {
                        obj = (double) num;
                    }
                    break;
                //default:
                //    throw new NotImplementedException("aItem.type: " + aItem.type);
            }
        }
    }
}