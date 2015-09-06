using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace altCrypt.Client.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string ApplicationName { get; } = "altCrypt Desktop";
        public ICommand OnSelectFolderCommand { get; }
    }
}