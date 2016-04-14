using System;
using System.IO;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileViewModel : FileSystemItemViewModel
    {
        public override string Name => _file.Name;
        public override string Extension => _file.Extension;
        public override long? Size => _file.Length;
        public override DateTime ModificationDate => _file.LastWriteTime;
        public override string Attributes => _file.Attributes.ToString();

        private readonly FileInfo _file;

        public FileViewModel(string path) : base(path)
        {
            _file = new FileInfo(Path);
        }
    }
}
