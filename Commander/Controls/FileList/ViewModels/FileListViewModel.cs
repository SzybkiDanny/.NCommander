using System.Collections.ObjectModel;
using System.Windows.Input;
using Commander.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileListViewModel : BindableBase
    {
        public string CurrentPath { get; set; }
        public FileListViewModel()
        {
            LoadPathCommand = new DelegateCommand(GetPathFiles, CanSubmit);
        }

        public ObservableCollection<FileSystemItemViewModel> Files { get; set; } =
            new ObservableCollection<FileSystemItemViewModel>();

        public ICommand LoadPathCommand { get; private set; }

        private void GetPathFiles()
        {
            Files.Add(FileSystemItemViewModel.Create(CurrentPath));
        }

        private static bool CanSubmit() => true;
    }
}