using System.Collections;
using NetSettingsCore.Common.Interfaces;

namespace NetSettingsCore.Common
{
    public interface IControlContainer
    {
        void StartUpdate();
        void Reset();
        void EndUpdate();
        //IList Controls { get; }
        void AddControl(IControl control);
        void ResetPosition();
    }
}