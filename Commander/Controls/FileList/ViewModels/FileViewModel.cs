using System.IO;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileViewModel : FileSystemItemViewModel
    {
        public long? Size => (FileSystemItem as FileInfo)?.Length;
        public string Extension => FileSystemItem.Extension;
        public override FileSystemInfo FileSystemItem { get; }

        public FileViewModel(string path)
        {
            FileSystemItem = new FileInfo(path);
        }
    }
}
