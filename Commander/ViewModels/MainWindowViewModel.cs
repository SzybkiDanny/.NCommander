using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public FileListViewModel LeftListViewModel { get; set; } = new FileListViewModel();
        public FileListViewModel RightListViewModel { get; set; } = new FileListViewModel();
        
        public ObservableCollection<string> CurrentDir { get; set; } = new ObservableCollection<string>() {"Item1", "Item2"};
    }
}
