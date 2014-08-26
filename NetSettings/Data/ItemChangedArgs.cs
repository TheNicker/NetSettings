using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
    public enum ItemChangedMode { None, OnTheFly, UserConfirmed }
    public class ItemChangedArgs
    {
        public string Key { get; set; }
        public object Val { get; set; }
        public ItemChangedMode ChangedMode { get; set; }

        public object sender;

    }
}
