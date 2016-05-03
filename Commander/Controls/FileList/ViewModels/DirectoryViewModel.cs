using System.IO;

namespace Commander.Controls.FileList.ViewModels
{
    public class DirectoryViewModel : FileSystemEntityViewModel
    {
        public DirectoryViewModel(string path)
        {
            FileSystemItem = new DirectoryInfo(path);
        }

        public override FileSystemInfo FileSystemItem { get; }
    }
}