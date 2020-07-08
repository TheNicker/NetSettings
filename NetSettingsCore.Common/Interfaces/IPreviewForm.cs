namespace NetSettingsCore.Common
{
    public interface IPreviewForm
    {
        void Show();
        string ImageName { get; set; }
        void Hide();
    }
}