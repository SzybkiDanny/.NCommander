using System.Collections.ObjectModel;
using System.Windows.Input;
using Commander.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileListViewModel : BindableBase
    {
        public string CurrentPath { get; set; } = @"D:\Music";
        public FileListViewModel()
        {
            LoadPathCommand = new DelegateCommand<string>(GetPathFiles, CanSubmit);
        }

        public ObservableCollection<FileViewModel> Files { get; set; } =
            new ObservableCollection<FileViewModel> {new FileViewModel(), new FileViewModel()};

        public ICommand LoadPathCommand { get; private set; }

        private void GetPathFiles(string path)
        {
            Files.Add(new FileViewModel());
        }

        private static bool CanSubmit(string path) => true;
    }
}