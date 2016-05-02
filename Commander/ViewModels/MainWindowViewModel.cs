using System;
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
            FileClick = new DelegateCommand<FileSystemInfo>(FileClicked);
            ChangeLanguage = new DelegateCommand<string>(LanguageChanged);
        }

        private void LanguageChanged(string language)
        {
            throw new NotImplementedException();
        }

        private void FileClicked(FileSystemInfo o)
        {
            o.ToString();
        }

        public ICommand FileClick { get; private set; }
        public ICommand ChangeLanguage { get; private set; }
    }
}
