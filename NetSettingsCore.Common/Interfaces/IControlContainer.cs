namespace NetSettings.Common.Interfaces
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