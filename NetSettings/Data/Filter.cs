using System;

namespace NetSettings.Data
{
    public class Filter
    {
        public string IncludeName;

        internal bool IsEmpty()
        {
            return String.IsNullOrWhiteSpace(IncludeName);
        }
    }
}
