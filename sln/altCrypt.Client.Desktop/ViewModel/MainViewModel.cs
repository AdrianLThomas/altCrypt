using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.FileSystem;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace altCrypt.Client.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string ApplicationName { get; } = "altCrypt Desktop [Alpha]";
        public ICommand OnSelectFolderCommand { get; }
        public ObservableCollection<IFile> SelectedFiles { get; } = new ObservableCollection<IFile>();

        public MainViewModel()
        {
            OnSelectFolderCommand = new RelayCommand(AddSelectedFilesToCollection);
        }

        private void AddSelectedFilesToCollection()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            CommonFileDialogResult result = dialog.ShowDialog();
            
            if (result == CommonFileDialogResult.Ok)
            {
                SelectedFiles.Clear();

                string folderPath = dialog.FileNames.Single();
                var directory = new LocalDirectory(folderPath);

                foreach (IFile file in directory.GetFilesIncludingSubdirectories())
                    SelectedFiles.Add(file);
            }
        }
    }
}