using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace altCrypt.Client.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string ApplicationName { get; } = "altCrypt Desktop";
        public ICommand OnSelectFolderCommand { get; }

        public MainViewModel()
        {
            OnSelectFolderCommand = new RelayCommand(OnSelectFolder);
        }

        private void OnSelectFolder()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            CommonFileDialogResult result = dialog.ShowDialog();
        }
    }
}