using System;
using System.IO;

namespace Commander.Controls.FileList.ViewModels
{
    public class DirectoryViewModel : FileSystemItemViewModel
    {
        public override string Name => _directory.Name;
        public override string Extension => _directory.Extension;
        public override long? Size => null;
        public override DateTime ModificationDate => _directory.LastWriteTime;
        public override string Attributes => _directory.Attributes.ToString();

        private DirectoryInfo _directory;

        public DirectoryViewModel(string path) : base(path)
        {
            _directory = new DirectoryInfo(path);
        }
    }
}
