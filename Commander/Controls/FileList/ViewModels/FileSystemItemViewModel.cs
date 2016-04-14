using System;
using System.IO;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public abstract class FileSystemItemViewModel : BindableBase
    {
        public virtual string Name { get; }
        public virtual string Extension { get; }
        public virtual long? Size { get; }
        public virtual DateTime ModificationDate { get; }
        public virtual string Attributes { get; }
        protected readonly string Path;

        protected FileSystemItemViewModel(string path)
        {
            Path = path;
        }

        public static FileSystemItemViewModel Create(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory) ? (FileSystemItemViewModel) new DirectoryViewModel(path) : new FileViewModel(path);
        }
    }
}
