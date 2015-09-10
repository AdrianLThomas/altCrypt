using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;

namespace altCrypt.Core.x86.Encryption
{
    public class FileEncryptor : StreamEncryptor, IEncryptFile, IEncryptFiles
    {
        public FileEncryptor(IKey key, IIV iv, SymmetricAlgorithm encryptionProvider)
            : base(key, iv, encryptionProvider)
        {
        }

        public void Encrypt(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            Encrypt(new[] { file });
        }

        public void Encrypt(IEnumerable<IFile> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            foreach (var file in files)
            {
                string tempFilename = $"{file.FilePath}.temp";
                using (var stream = new FileStream(tempFilename, FileMode.OpenOrCreate))
                {
                    this.EncryptToStream(file, stream);
                    file.Write(stream);
                }

                File.Delete(tempFilename);
            }
        }

        public void Decrypt(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            Decrypt(new[] { file });
        }

        public void Decrypt(IEnumerable<IFile> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            foreach (var file in files)
            {
                string tempFilename = $"{file.FilePath}.temp";
                using (var stream = new FileStream(tempFilename, FileMode.OpenOrCreate))
                {
                    this.DecryptToStream(file, stream);
                    file.Write(stream);
                }

                File.Delete(tempFilename);
            }
        }
    }
}
