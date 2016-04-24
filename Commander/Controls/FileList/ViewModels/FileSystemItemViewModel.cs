using System;
using System.IO;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public abstract class FileSystemItemViewModel : BindableBase
    {
        public string Name => FileSystemItem.Name;
        public string Extension => FileSystemItem.Extension;
        public DateTime ModificationDate => FileSystemItem.LastWriteTime;
        public string Attributes => FileSystemItem.Attributes.ToString();
        public abstract FileSystemInfo FileSystemItem { get; }

        public static FileSystemItemViewModel Create(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory) ? (FileSystemItemViewModel) new DirectoryViewModel(path) : new FileViewModel(path);
        }
    }
}
