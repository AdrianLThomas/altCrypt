using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace altCrypt.Client.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog {IsFolderPicker = true};
            CommonFileDialogResult result = dialog.ShowDialog();
        }
    }
}
