using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public class ItemHelpers
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
            if (item.subitems != null)
                foreach (ItemTree subItem in item.subitems)
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

        internal static void NormalizeItemData(ItemTree aItem, ref object obj)
        {
            if (aItem.type == "color")
            {
                if (obj != null && obj is string)
                    obj = System.Drawing.ColorTranslator.FromHtml(obj as string);

                //if (aItem.value != null && aItem.value is string)
                //    aItem.value = System.Drawing.ColorTranslator.FromHtml(aItem.value as string);
            }

            if (aItem.type == "number")
            {
                if (obj == null)
                    obj = 0.0;
                else
                    if (obj is Int64)
                        obj = (double)(Int64)obj;

            }
        }
    }
}
