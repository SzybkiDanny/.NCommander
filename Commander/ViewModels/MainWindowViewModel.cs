using System.Globalization;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using WPFLocalizeExtension.Engine;

namespace Commander.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            ChangeLanguage = new DelegateCommand<string>(LanguageChanged);
        }

        public ICommand ChangeLanguage { get; private set; }

        private void LanguageChanged(string language)
        {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = new CultureInfo(language);
        }
    }
}