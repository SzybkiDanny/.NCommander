using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileViewModel : FileSystemItemViewModel
    {
        public long? Size => (FileSystemItem as FileInfo)?.Length;
        public override FileSystemInfo FileSystemItem { get; }

        public FileViewModel(string path)
        {
            FileSystemItem = new FileInfo(path);
        }
    }
}
