using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Commander.Controls.FileList.Win32;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public abstract class FileSystemItemViewModel : BindableBase
    {
        private string _displayName;

        public string DisplayName
        {
            get { return _displayName ?? FileSystemItem.Name; }
            set { _displayName = value; }
        }
        public string Extension => FileSystemItem.Extension;
        public DateTime ModificationDate => FileSystemItem.LastWriteTime;
        public string Attributes => FileSystemItem.Attributes.ToString();
        public abstract FileSystemInfo FileSystemItem { get; }
        public Stream Ico
        {
            get
            {
                var shinfo = new SHFILEINFO();
                Win32.Win32.SHGetFileInfo(FileSystemItem.FullName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.Win32.SHGFI_ICON | Win32.Win32.SHGFI_SMALLICON);
                var resultStream = new MemoryStream();
                var bitmap = Icon.FromHandle(shinfo.hIcon).ToBitmap();
                bitmap.Save(resultStream, ImageFormat.Png);
                return resultStream;
            }
        }

        public static FileSystemItemViewModel Create(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory) ? (FileSystemItemViewModel) new DirectoryViewModel(path) : new FileViewModel(path);
        }
    }
}
