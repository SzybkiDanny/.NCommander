using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileListViewModel : BindableBase
    {
        public string CurrentPath { get; set; }
        public ObservableCollection<FileSystemItemViewModel> Files { get; set; } =
            new ObservableCollection<FileSystemItemViewModel>();
        public ICommand LoadPathCommand { get; private set; }

        public FileListViewModel()
        {
            LoadPathCommand = new DelegateCommand(GetPathFiles);
        }

        private void GetPathFiles()
        {
            Files.Clear();
            foreach (var directory in Directory.GetDirectories(CurrentPath).Select(FileSystemItemViewModel.Create))
                Files.Add(directory);
            foreach (var file in Directory.GetFiles(CurrentPath).Select(FileSystemItemViewModel.Create))
                Files.Add(file);
        }
    }
}