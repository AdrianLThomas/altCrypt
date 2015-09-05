using System;
using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.Encryption;
using altCrypt.Core.x86.FileSystem;
using System.IO;
using System.Security.Cryptography;

namespace altCrypt.Client
{
    static class Program
    {
        static void Main(string[] args)
        {
            IKey key = new Key("Pass@w0rd1");
            SymmetricAlgorithm encryptionProvider = new AesCryptoServiceProvider();
            IEncryptor encryptor = new StreamEncryptor(key, encryptionProvider);

            IFile<Stream> file = new LocalFile(@"C:\temp\Test.txt");
            encryptor.Encrypt(file);

            Console.WriteLine("Please check the file has been encrypted. Hit any key to decrypt...");
            Console.ReadKey();

            encryptor.Decrypt(file);
        }
    }
}
