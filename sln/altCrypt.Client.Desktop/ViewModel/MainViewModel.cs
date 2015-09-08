using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.FileSystem;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace altCrypt.Client.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEncryptFiles _fileEncryptor;
        public string ApplicationName { get; } = "altCrypt Desktop [Alpha]";
        public ICommand OnSelectFolderCommand { get; }
        public ICommand EncryptCommand { get; }
        public ICommand DecryptCommand { get; }

        public ObservableCollection<IFile<Stream>> SelectedFiles { get; } = new ObservableCollection<IFile<Stream>>();


        public MainViewModel(IEncryptFiles fileEncryptor)
        {
            if (fileEncryptor == null)
                throw new ArgumentNullException(nameof(fileEncryptor));

            _fileEncryptor = fileEncryptor;

            OnSelectFolderCommand = new RelayCommand(AddSelectedFilesToCollection);
            EncryptCommand = new RelayCommand(EncryptSelectedFiles);
            DecryptCommand = new RelayCommand(DecryptSelectedFiles);
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

                foreach (IFile<Stream> file in directory.GetFilesIncludingSubdirectories())
                    SelectedFiles.Add(file);
            }
        }

        private void EncryptSelectedFiles()
        {
            _fileEncryptor.Encrypt(SelectedFiles);
        }

        private void DecryptSelectedFiles()
        {
            _fileEncryptor.Decrypt(SelectedFiles);
        }
    }
}