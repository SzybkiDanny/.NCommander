using System.IO;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileViewModel : FileSystemEntityViewModel
    {
        public FileViewModel(string path)
        {
            FileSystemItem = new FileInfo(path);
        }

        public long? Size => (FileSystemItem as FileInfo)?.Length;
        public string Extension => FileSystemItem.Extension;
        public override FileSystemInfo FileSystemItem { get; }
    }
}