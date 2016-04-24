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
            Task.Run(() =>
            {
                UiInvoke(() => Files.Clear());
            var newDirectory = new DirectoryInfo(CurrentPath);
            if (newDirectory?.Parent != null)
                UiInvoke(() => Files.Add(new DirectoryViewModel(newDirectory.Parent?.FullName) { DisplayName = ".." }));
            foreach (var directory in Directory.GetDirectories(CurrentPath).Select(FileSystemItemViewModel.Create))
                    UiInvoke(() => Files.Add(directory));
                foreach (var file in Directory.GetFiles(CurrentPath).Select(FileSystemItemViewModel.Create))
                    UiInvoke(() => Files.Add(file));
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