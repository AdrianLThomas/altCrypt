using System;
using System.IO;
using altCrypt.Core.FileSystem;
using altCrypt.Core.Encryption;
using System.Collections.Generic;

namespace altCrypt.Business
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IEncryptFile _fileEncryptor;
        private readonly string _processedExtension;

        public FileProcessor(string processedExtension, IEncryptFile fileEncryptor)
        {
            if (string.IsNullOrEmpty(processedExtension))
                throw new ArgumentNullException(nameof(processedExtension));
            if (fileEncryptor == null)
                throw new ArgumentNullException(nameof(fileEncryptor));

            _processedExtension = processedExtension;
            _fileEncryptor = fileEncryptor;
        }

        public void Process(IEnumerable<IFile<Stream>> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            foreach (var file in files)
            {
                string currentFilename = Path.GetFileName(file.Name);
                string newFilenameWithExtension = Path.Combine(currentFilename, _processedExtension);

                _fileEncryptor.Encrypt(file);
                file.Rename(newFilenameWithExtension);
            }
        }

        public void ReverseProcess(IEnumerable<IFile<Stream>> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            foreach (var file in files)
            {
                string currentFilename = Path.GetFileName(file.Name);
                string originalFilename = Path.GetFileNameWithoutExtension(currentFilename);

                _fileEncryptor.Decrypt(file);
                file.Rename(originalFilename);
            }
        }
    }
}
