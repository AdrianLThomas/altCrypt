using altCrypt.Core.FileSystem;
using System;
using System.IO;
using System.Security.Cryptography;

namespace altCrypt.Core.x86
{
    public class FileEncryptor : IEncryptor
    {
        public Stream Encrypt(IFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var provider = Aes.Create();
            ICryptoTransform encryptor = provider.CreateEncryptor();

            var cryptoStream = new CryptoStream(file.Data, encryptor, CryptoStreamMode.Write);

            using (var fileStream = file.Data)
                fileStream.CopyTo(cryptoStream);

            return cryptoStream;
        }
    }
}
