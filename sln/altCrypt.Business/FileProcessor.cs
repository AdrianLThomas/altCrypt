using System;
using System.IO;
using altCrypt.Core.FileSystem;
using altCrypt.Core.Encryption;

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

        public void Process(IFile<Stream> file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string newFilenameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
            string newFilenameWithExtension = Path.Combine(newFilenameWithoutExtension, _processedExtension);

            _fileEncryptor.Encrypt(file);
            file.Rename(newFilenameWithExtension);
        }
    }
}
