using NetSettings.Data;
using System;
using System.Collections.Generic;
using NetSettingsCore.Common;
using NetSettingsCore.Common.Classes;

namespace NetSettings
{
    internal class ItemHelpers
    {
        public static void BuildQualifiedNames(Dictionary<string, ItemTree> aQualifiedNames, ItemTree item, ItemTree parent)
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

            if (item.subItems != null)
                foreach (ItemTree subItem in item.subItems)
                {
                    BuildQualifiedNames(aQualifiedNames, subItem, item);

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
                    }
                    finally
                    {
                        System.Threading.Thread.CurrentThread.CurrentCulture =
                            lastCulture; //TODO: What is the purpose of this line?
                    }


                    //TODO: Why do we need this lines?
                    //Make sure all the colors are created from (R,G,B) and not known names
                    var color = (Color)obj;
                    obj = Color.FromArgb(color.R, color.G, color.B);

                    //TODO: Delete this lines? if no remove the System.Drawing
                    //if (aItem.value != null && aItem.value is string)
                    //    aItem.value = System.Drawing.ColorTranslator.FromHtml(aItem.value as string);
                    break;
                case "number":
                    //normalize all our numbers to double data type
                    if (obj != null && obj is long num)
                        obj = (double)num;
                    break;
            }
        }
    }
}