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
            OrderCommand = new DelegateCommand<string>(ExecuteMethod);
        }

        private void ExecuteMethod(string s)
        {
            throw new NotImplementedException();
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
        public ICommand OrderCommand { get; private set; }

        private void GetPathFiles(string path)
        {
            Task.Run(() =>
            {
                var rootDirectory = new DirectoryInfo(path);
                Files =
                    new ObservableCollection<FileSystemItemViewModel>(
                        rootDirectory.EnumerateFileSystemInfos()
                            .Where(f => (f.Attributes & (FileAttributes.System | FileAttributes.Hidden)) == 0)
                            .Select(f => FileSystemItemViewModel.Create(f.FullName)));

                if (rootDirectory.Parent != null)
                    Files.Insert(0, new DirectoryViewModel(rootDirectory.Parent?.FullName) {DisplayName = ".."});
                CurrentPath = path;
            });
        }

        //private static void UiInvoke(Action action)
        //{
        //    if (!Application.Current.Dispatcher.CheckAccess())
        //        Application.Current.Dispatcher.Invoke(action);
        //    else
        //        action();
        //}
    }
}