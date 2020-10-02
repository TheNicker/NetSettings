namespace NetSettings.Common.Interfaces
{
    public interface IPreviewForm
    {
        void Show();
        string ImageName { get; set; }
        void Hide();
    }
}