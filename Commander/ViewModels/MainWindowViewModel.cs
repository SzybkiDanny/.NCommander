using System;
using System.IO;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Commander.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            FileClick = new DelegateCommand<FileSystemInfo>(FileClicked);
            ChangeLanguage = new DelegateCommand<string>(LanguageChanged);
        }

        public ICommand FileClick { get; private set; }
        public ICommand ChangeLanguage { get; private set; }

        private void LanguageChanged(string language)
        {
            throw new NotImplementedException();
        }

        private void FileClicked(FileSystemInfo o)
        {
            o.ToString();
        }
    }
}