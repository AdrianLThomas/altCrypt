using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;
using System.Threading.Tasks;
using System.Linq;

namespace altCrypt.Core.x86.Encryption
{
    public class FileEncryptor : StreamEncryptor, IEncryptFile, IEncryptFiles
    {
        public FileEncryptor(IKey key, IIV iv, SymmetricAlgorithm encryptionProvider)
            : base(key, iv, encryptionProvider)
        {
        }

        public async Task EncryptAsync(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string tempFilename = $"{file.FilePath}.temp";
            using (var stream = new FileStream(tempFilename, FileMode.OpenOrCreate))
            {
                this.EncryptToStream(file, stream);
                await file.WriteAsync(stream);
            }

            File.Delete(tempFilename);
        }

        public async Task EncryptAsync(IEnumerable<IFile> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            foreach (var file in files)
            {
                await EncryptAsync(file);
            }
        }

        public async Task DecryptAsync(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string tempFilename = $"{file.FilePath}.temp";
            using (var stream = new FileStream(tempFilename, FileMode.OpenOrCreate))
            {
                this.DecryptToStream(file, stream);
                await file.WriteAsync(stream);
            }

            File.Delete(tempFilename);
        }

        public async Task DecryptAsync(IEnumerable<IFile> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            foreach (var file in files)
            {
                await DecryptAsync(file);
            }
        }
    }
}
