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
using System.Threading.Tasks;

namespace altCrypt.Client.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFileProcessor _fileProcessor;
        private bool _isProcessing;

        public string ApplicationName { get; } = "altCrypt Desktop [Alpha]";
        public ICommand OnSelectFolderCommand { get; }
        public ICommand EncryptCommand { get; }
        public ICommand DecryptCommand { get; }
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set { Set(nameof(IsProcessing), ref _isProcessing, value); }
        }

        public ObservableCollection<IFile> SelectedFiles { get; } = new ObservableCollection<IFile>();

        public MainViewModel(IFileProcessor fileProcessor)
        {
            if (fileProcessor == null)
                throw new ArgumentNullException(nameof(fileProcessor));

            _fileProcessor = fileProcessor;

            OnSelectFolderCommand = new RelayCommand(AddSelectedFilesToCollection);
            EncryptCommand = new RelayCommand(async () => await EncryptSelectedFilesAsync());
            DecryptCommand = new RelayCommand(async () => await DecryptSelectedFilesAsync());
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

        private async Task EncryptSelectedFilesAsync()
        {
            await PerformProcessing(_fileProcessor.ProcessAsync(SelectedFiles));
        }

        private async Task DecryptSelectedFilesAsync()
        {
            await PerformProcessing(_fileProcessor.ReverseProcessAsync(SelectedFiles));
        }

        private async Task PerformProcessing(Task task)
        {
            IsProcessing = true;

            await task;

            IsProcessing = false;
        }
    }
}