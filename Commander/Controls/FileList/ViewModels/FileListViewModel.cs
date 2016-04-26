using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileListViewModel : BindableBase
    {
        private string _currentPath = @"D:\";

        private ObservableCollection<FileSystemItemViewModel> _files =
            new ObservableCollection<FileSystemItemViewModel>();

        public FileListViewModel()
        {
            LoadPathCommand = new DelegateCommand<string>(GetPathFiles);
        }

        public string CurrentPath
        {
            get { return _currentPath; }
            set { SetProperty(ref _currentPath, value); }
        }

        public ObservableCollection<FileSystemItemViewModel> Files
        {
            get { return _files; }
            set { SetProperty(ref _files, value); }
        }

        public ICommand LoadPathCommand { get; private set; }

        private void GetPathFiles(string path)
        {
            Task.Run(() =>
            {
                Files =
                    new ObservableCollection<FileSystemItemViewModel>(
                        Directory.GetDirectories(path)
                            .Select(FileSystemItemViewModel.Create)
                            .Concat(Directory.GetFiles(path).Select(FileSystemItemViewModel.Create)));
                var newDirectory = new DirectoryInfo(path);
                if (newDirectory?.Parent != null)
                    Files.Insert(0, new DirectoryViewModel(newDirectory.Parent?.FullName) { DisplayName = ".." });
                CurrentPath = path;
            });
        }

        private static void UiInvoke(Action action)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
                Application.Current.Dispatcher.Invoke(action);
            else
                action();
        }
    }
}