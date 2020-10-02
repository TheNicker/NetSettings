using System.Collections.Generic;
using Avalonia.Collections;
using Avalonia.LogicalTree;
using NetSettings.Common.Interfaces;

namespace NetSettings.Avalonia.AvaloniaControls
{
    class LogicalControls : List<IControl>
    {
        public LogicalControls(IAvaloniaList<ILogical> logicalChildren)
        {
        }
    }
}
