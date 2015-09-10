using System;
using System.IO;
using altCrypt.Core.FileSystem;
using altCrypt.Core.Encryption;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace altCrypt.Business
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IEncryptFiles _fileEncryptor;
        private readonly string _processedExtension;

        public FileProcessor(string processedExtension, IEncryptFiles fileEncryptor)
        {
            if (string.IsNullOrEmpty(processedExtension))
                throw new ArgumentNullException(nameof(processedExtension));
            if (fileEncryptor == null)
                throw new ArgumentNullException(nameof(fileEncryptor));

            _processedExtension = processedExtension;
            _fileEncryptor = fileEncryptor;
        }

        public async Task ProcessAsync(IEnumerable<IFile> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            await _fileEncryptor.EncryptAsync(files);

            foreach (var file in files)
            {
                string currentFilename = Path.GetFileName(file.Name);
                string newFilenameWithExtension = string.Concat(currentFilename, _processedExtension);

                file.Rename(newFilenameWithExtension);
            }
        }

        public async Task ReverseProcessAsync(IEnumerable<IFile> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            await _fileEncryptor.DecryptAsync(files);

            foreach (var file in files)
            {
                string currentFilename = Path.GetFileName(file.Name);
                string originalFilename = Path.GetFileNameWithoutExtension(currentFilename);

                file.Rename(originalFilename);
            }
        }
    }
}
