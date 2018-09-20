namespace DM.Models.ViewModels
{
    public class ImageCreation
    {
        public ImageCreation(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
    }
}
