using System.Globalization;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using WPFLocalizeExtension.Engine;

namespace Commander.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            CurrentCulture = CultureInfo.CurrentUICulture;
            ChangeLanguage = new DelegateCommand<string>(LanguageChanged);
        }

        public CultureInfo CurrentCulture
        {
            get { return LocalizeDictionary.Instance.Culture; }
            set
            {
                if (LocalizeDictionary.Instance.Culture == value)
                    return;
                LocalizeDictionary.Instance.Culture = value;
                OnPropertyChanged(nameof(CurrentCulture));
            }
        }

        public ICommand ChangeLanguage { get; private set; }

        private void LanguageChanged(string language)
        {
            CurrentCulture = new CultureInfo(language);
        }
    }
}