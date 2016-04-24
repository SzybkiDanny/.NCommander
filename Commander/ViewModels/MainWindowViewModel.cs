using System.IO;
using System.Windows.Input;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;

namespace Commander.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            FileClicked = new DelegateCommand<FileSystemInfo>(ExecuteMethod);
        }

        private void ExecuteMethod(FileSystemInfo o)
        {
            o.ToString();
        }

        public ICommand FileClicked { get; private set; }
    }
}
