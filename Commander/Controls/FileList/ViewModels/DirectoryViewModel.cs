using System.IO;

namespace Commander.Controls.FileList.ViewModels
{
    public class DirectoryViewModel : FileSystemEntityViewModel
    {
        public override FileSystemInfo FileSystemItem { get; }

        public DirectoryViewModel(string path)
        {
            FileSystemItem = new DirectoryInfo(path);
        }
    }
}
