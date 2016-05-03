using System.IO;
using System.Windows;
using System.Windows.Input;
using Commander.Controls.FileList.ViewModels;
using Microsoft.Practices.Prism.Commands;

namespace Commander.Controls.FileList
{
    public partial class FileList
    {
        public static readonly DependencyProperty FileSelectedProperty =
            DependencyProperty.Register(
                "FileSelected",
                typeof (ICommand),
                typeof (FileList),
                new UIPropertyMetadata(null));

        private static readonly DependencyProperty FileSelectedInternalProperty =
            DependencyProperty.Register(
                "FileSelectedInternal",
                typeof (ICommand),
                typeof (FileList),
                new UIPropertyMetadata(null));

        public ICommand FileSelected
        {
            get { return (ICommand)GetValue(FileSelectedProperty); }
            set { SetValue(FileSelectedProperty, value); }
        }

        private ICommand FileSelectedInternal
        {
            get { return (ICommand)GetValue(FileSelectedInternalProperty); }
            set { SetValue(FileSelectedInternalProperty, value); }
        }

        public FileList()
        {
            InitializeComponent();
            FileSelectedInternal = new DelegateCommand<FileSystemInfo>(HandleEntitySelection);
        }

        private void HandleEntitySelection(FileSystemInfo fileSystemInfo)
        {
            FileSelected?.Execute(fileSystemInfo);
            if (!File.GetAttributes(fileSystemInfo.FullName).HasFlag(FileAttributes.Directory)) return;

            (DataContext as FileListViewModel)?.LoadPathCommand.Execute(fileSystemInfo.FullName);
        }
    }
}