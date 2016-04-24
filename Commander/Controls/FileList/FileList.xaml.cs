using System.Windows;
using System.Windows.Input;

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

        public ICommand FileSelected
        {
            get { return (ICommand)GetValue(FileSelectedProperty); }
            set { SetValue(FileSelectedProperty, value); }
        }

        public FileList()
        {
            InitializeComponent();
        }
    }
}