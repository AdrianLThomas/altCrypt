using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.Encryption;
using altCrypt.Core.x86.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace altCrypt.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IKey key = new Key("Pass@w0rd1");
            SymmetricAlgorithm encryptionProvider = new AesCryptoServiceProvider();
            IEncryptor encryptor = new StreamEncryptor(key, encryptionProvider);

            ////Encrypt
            //{
            //    IFile file = new LocalFile(@"C:\temp\Test.txt");
            //    using (Stream encryptedContent = encryptor.EncryptToStream(file))
            //    {
            //        using (var newFile = File.Create(@"C:\temp\Test.encrypted.txt"))
            //        {
            //            var mem = (MemoryStream)encryptedContent;
            //            byte[] bytes = mem.ToArray();

            //            newFile.Write(bytes, 0, bytes.Length);
            //        }
            //    }
            //}

            ////Decrypt
            //{
            //    IFile file = new LocalFile(@"C:\temp\Test.encrypted.txt");
            //    using (Stream decryptedContent = encryptor.DecryptToStream(file))
            //    {
            //        using (var newFile = File.Create(@"C:\temp\Test.decrypted.txt"))
            //        {
            //            var mem = (MemoryStream)decryptedContent;
            //            byte[] bytes = mem.ToArray();

            //            newFile.Write(bytes, 0, bytes.Length);
            //        }
            //    }
            //}

            //Ideal usage:
            {
                IFile<Stream> file = new LocalFile(@"C:\temp\Test.txt");
                encryptor.Encrypt(file);
            }
        }
    }
}
