using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSettings
{
   public class ItemControlsGroup
        {
            public System.Windows.Forms.Label label;
            public System.Windows.Forms.Control parentContainer;
            public System.Windows.Forms.Control control;
            public System.Windows.Forms.Button defaultButton;

            public bool Visible 
            {
                set
                {
                    label.Visible = parentContainer.Visible = control.Visible = value;
                    if (defaultButton != null)
                        defaultButton .Visible= value;
                }
            }
        }
}
