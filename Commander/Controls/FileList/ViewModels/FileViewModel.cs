using Microsoft.Practices.Prism.Mvvm;

namespace Commander.Controls.FileList.ViewModels
{
    public class FileViewModel : BindableBase
    {
        public string FileName { get; set; } = "File1";
    }
}
