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
using altCrypt.Business;

namespace altCrypt.Client.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFileProcessor _fileProcessor;
        public string ApplicationName { get; } = "altCrypt Desktop [Alpha]";
        public ICommand OnSelectFolderCommand { get; }
        public ICommand EncryptCommand { get; }
        public ICommand DecryptCommand { get; }

        public ObservableCollection<IFile<Stream>> SelectedFiles { get; } = new ObservableCollection<IFile<Stream>>();

        public MainViewModel(IFileProcessor fileProcessor)
        {
            if (fileProcessor == null)
                throw new ArgumentNullException(nameof(fileProcessor));

            _fileProcessor = fileProcessor;

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
            _fileProcessor.Process(SelectedFiles);
        }

        private void DecryptSelectedFiles()
        {
            _fileProcessor.ReverseProcess(SelectedFiles);
        }
    }
}