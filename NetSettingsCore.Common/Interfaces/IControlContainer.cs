using System.Collections;

namespace NetSettingsCore.Common
{
    public interface IControlContainer
    {
        void StartUpdate();
        void Reset();
        void EndUpdate();
        IList Controls { get; }
        void ResetPosition();
    }
}