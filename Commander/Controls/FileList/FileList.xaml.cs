using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            FileSelectedInternal= new DelegateCommand<FileSystemInfo>(ExecuteMethod);
        }

        private void ExecuteMethod(FileSystemInfo fileSystemInfo)
        {
            FileSelected?.Execute(fileSystemInfo);
            if (!File.GetAttributes(fileSystemInfo.FullName).HasFlag(FileAttributes.Directory)) return;

            (DataContext as FileListViewModel)?.LoadPathCommand.Execute(fileSystemInfo.FullName);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            var dragSource = sender as ListView;
            if (dragSource != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(dragSource,
                                     dragSource.SelectedItems,
                                     DragDropEffects.Move);
            }
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(e.Data.GetFormats().FirstOrDefault()) || sender == e.Source)
                e.Effects = DragDropEffects.None;
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            //dynamic droppedFiles = e.Data.GetData(e.Data.GetFormats().FirstOrDefault());
            //if (droppedFiles != null)
            //    File.Move(droppedFiles.FileSystemItem.FullName, CurrentPathTextBox.Text + "/" + droppedFiles[0].FileSystemItem.Name);
            
        }
    }
}