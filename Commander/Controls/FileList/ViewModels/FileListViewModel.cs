using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileListViewModel : BindableBase, IDisposable
    {
        private string _currentPath = @"D:\";
        private ObservableCollection<FileSystemItemViewModel> _files =
            new ObservableCollection<FileSystemItemViewModel>();
        private CollectionViewSource _filesDataView = new CollectionViewSource();
        private string _sortColumn;
        private ListSortDirection _sortDirection;
        private FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher();
        private bool _isDisposed;

        public FileListViewModel()
        {
            LoadPathCommand = new DelegateCommand<string>(LoadPathFiles);
            OrderCommand = new DelegateCommand<string>(OrderFiles);
        }

        public string CurrentPath
        {
            get { return _currentPath; }
            set { SetProperty(ref _currentPath, value); }
        }

        public ObservableCollection<FileSystemItemViewModel> Files
        {
            get { return _files; }
            set
            {
                SetProperty(ref _files, value);
                _filesDataView = new CollectionViewSource {Source = value};
                OnPropertyChanged(nameof(FilesDataView));
            }
        }

        public ICommand LoadPathCommand { get; private set; }
        public ICommand OrderCommand { get; private set; }
        public ListCollectionView FilesDataView => _filesDataView.View as ListCollectionView;

        private void LoadPathFiles(string path)
        {
            Task.Run(() =>
            {
                var rootDirectory = new DirectoryInfo(path);
                UiInvoke(() =>
                    Files =
                        new ObservableCollection<FileSystemItemViewModel>(
                            rootDirectory.EnumerateFileSystemInfos()
                                .Where(f => (f.Attributes & (FileAttributes.System | FileAttributes.Hidden)) == 0)
                                .Select(f => FileSystemItemViewModel.Create(f.FullName)))
                    );
                if (rootDirectory.Parent != null)
                    UiInvoke(() => Files.Insert(0, new DirectoryViewModel(rootDirectory.Parent?.FullName) {DisplayName = ".."}));
                CurrentPath = path;
                SetUpFileWatcher(path);
            });
        }

        private void OrderFiles(string column)
        {
            if (_sortColumn != column)
            {
                _sortColumn = column;
                _sortDirection = ListSortDirection.Ascending;
            }
            else
                _sortDirection = _sortDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;

            _filesDataView.SortDescriptions.Clear();
            _filesDataView.SortDescriptions.Add(
                new SortDescription(_sortColumn, _sortDirection));
        }

        private static void UiInvoke(Action action)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
                Application.Current.Dispatcher.Invoke(action);
            else
                action();
        }

        private void SetUpFileWatcher(string path)
        {
            FileSystemEventHandler fileSystemChangeHandler =
                delegate {
                    LoadPathFiles(path);
                };

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Path = path;
            _fileSystemWatcher.Changed += fileSystemChangeHandler;
            _fileSystemWatcher.Created += fileSystemChangeHandler;
            _fileSystemWatcher.Deleted += fileSystemChangeHandler;
            _fileSystemWatcher.Renamed += (sender, args) => LoadPathFiles(path);
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _fileSystemWatcher.Dispose();
            _isDisposed = true;
        }
    }
}