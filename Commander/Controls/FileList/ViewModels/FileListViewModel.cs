using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.VisualBasic.FileIO;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileListViewModel : BindableBase, IDisposable, IDropTarget
    {
        private const string ParentDirectoryDisplayName = "..";
        private readonly FileSystemWatcher _fileSystemWatcher = new FileSystemWatcher();
        private string _currentPath;

        private ObservableCollection<FileSystemEntityViewModel> _files =
            new ObservableCollection<FileSystemEntityViewModel>();

        private CollectionViewSource _filesDataView = new CollectionViewSource();
        private bool _isDisposed;
        private DriveInfo _selectedDrive;
        private string _sortColumn;
        private ListSortDirection _sortDirection;

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

        public ObservableCollection<FileSystemEntityViewModel> Files
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
        public ICommand LoadParentDirectoryCommand { get; private set; }
        public ListCollectionView FilesDataView => _filesDataView.View as ListCollectionView;
        public DriveInfo[] AvailableDrives => DriveInfo.GetDrives();

        public DriveInfo SelectedDrive
        {
            get { return _selectedDrive ?? AvailableDrives.FirstOrDefault(); }
            set
            {
                SetProperty(ref _selectedDrive, value);
                CurrentPath = _selectedDrive.Name;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _fileSystemWatcher.Dispose();
            _isDisposed = true;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is FileSystemEntityViewModel || dropInfo.Data is IList<FileSystemEntityViewModel>))
                return;
            if (dropInfo.TargetItem is FileViewModel)
                return;
            dropInfo.Effects = (dropInfo.KeyStates & DragDropKeyStates.ShiftKey) != 0
                ? DragDropEffects.Copy
                : DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var destinationPath = CurrentPath;
            if (dropInfo.TargetItem is DirectoryViewModel)
                destinationPath = ((DirectoryViewModel) dropInfo.TargetItem).FileSystemItem.FullName;

            var fileSystemAction = (dropInfo.KeyStates & DragDropKeyStates.ShiftKey) != 0
                ? (Action<IList<FileSystemEntityViewModel>, string>) CopyItems
                : MoveItems;

            if (dropInfo.Data is FileSystemEntityViewModel)
                fileSystemAction(new[] {(FileSystemEntityViewModel) dropInfo.Data}, destinationPath);
            else if (dropInfo.Data is IList<FileSystemEntityViewModel>)
                fileSystemAction((IList<FileSystemEntityViewModel>) dropInfo.Data, destinationPath);
        }

        private void LoadPathFiles(string path)
        {
            if (!Directory.Exists(path))
                return;
            Task.Run(() =>
            {
                var rootDirectory = new DirectoryInfo(path);
                UiInvoke(() =>
                {
                    Files =
                        new ObservableCollection<FileSystemEntityViewModel>(
                            rootDirectory.EnumerateFileSystemInfos()
                                .Where(f => (f.Attributes & (FileAttributes.System | FileAttributes.Hidden)) == 0)
                                .Select(f => FileSystemEntityViewModel.Create(f.FullName)));
                });
                if (rootDirectory.Parent != null)
                    UiInvoke(
                        () =>
                            Files.Insert(0,
                                new DirectoryViewModel(rootDirectory.Parent?.FullName)
                                {
                                    DisplayName = ParentDirectoryDisplayName
                                }));
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
                action?.Invoke();
        }

        private void SetUpFileWatcher(string path)
        {
            FileSystemEventHandler fileSystemChangeHandler =
                delegate { LoadPathFiles(path); };

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Path = path;
            _fileSystemWatcher.Changed += fileSystemChangeHandler;
            _fileSystemWatcher.Created += fileSystemChangeHandler;
            _fileSystemWatcher.Deleted += fileSystemChangeHandler;
            _fileSystemWatcher.Renamed += (sender, args) => LoadPathFiles(path);
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void CopyItems(IList<FileSystemEntityViewModel> fileSystemEntityViewModels,
            string destinationPath)
        {
            foreach (var file in fileSystemEntityViewModels.OfType<FileViewModel>())
                FileSystem.CopyFile(file.FileSystemItem.FullName, $"{destinationPath}/{file.FileSystemItem.Name}");
            foreach (var directory in fileSystemEntityViewModels.OfType<DirectoryViewModel>())
                FileSystem.CopyDirectory(directory.FileSystemItem.FullName,
                    $"{destinationPath}/{directory.FileSystemItem.Name}");
        }

        private static void MoveItems(IList<FileSystemEntityViewModel> fileSystemEntityViewModels,
            string destinationPath)
        {
            foreach (var file in fileSystemEntityViewModels.OfType<FileViewModel>())
                FileSystem.MoveFile(file.FileSystemItem.FullName, $"{destinationPath}/{file.FileSystemItem.Name}");
            foreach (var directory in fileSystemEntityViewModels.OfType<DirectoryViewModel>())
                FileSystem.MoveDirectory(directory.FileSystemItem.FullName,
                    $"{destinationPath}/{directory.FileSystemItem.Name}");
        }
    }
}
