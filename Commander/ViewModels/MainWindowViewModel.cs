using System.Collections.ObjectModel;
using Commander.Controls.FileList.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public FileListViewModel LeftListViewModel { get; set; } = new FileListViewModel();
        public FileListViewModel RightListViewModel { get; set; } = new FileListViewModel();
    }
}
