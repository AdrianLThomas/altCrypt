using System;
using System.Collections.Generic;
using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.Encryption;
using altCrypt.Core.x86.FileSystem;
using System.IO;
using System.Linq;
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

            //ManipulateFile(encryptor);
            //ManipulateDirectory(encryptor);

            //var memStream = ReadFromFile(@"C:\temp\Picture.jpg");
            //WriteToFile(memStream, @"C:\temp\New_Picture.jpg");
        }

        private static void WriteToFile(MemoryStream memStream, string path)
        {
            LocalFile file = new LocalFile(path);
            file.Write(memStream);
        }

        private static MemoryStream ReadFromFile(string path)
        {
            LocalFile file = new LocalFile(path);
            var memStream = new MemoryStream();
            using (FileStream fileHandle = file.Read())
            {
                byte[] buffer = new byte[fileHandle.Length];
                fileHandle.Read(buffer, 0, buffer.Length);
                fileHandle.Seek(0, SeekOrigin.Begin);

                memStream.Write(buffer, 0, buffer.Length);
                memStream.Flush();
            }

            return memStream;
        }

        private static void ManipulateFile(IEncryptor encryptor)
        {
            IFile<Stream> file = new LocalFile(@"C:\temp\Picture.jpg");
            encryptor.Encrypt(file);

            Console.WriteLine("Please check the file has been encrypted. Hit any key to decrypt...");
            Console.ReadKey();

            encryptor.Decrypt(file);
        }

        private static void ManipulateDirectory(IEncryptor encryptor)
        {
            IDirectory<Stream> directory = new LocalDirectory(@"C:\temp");
            IEnumerable<IFile<Stream>> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                encryptor.Encrypt(file);

            Console.WriteLine("Directory encrypted. Hit any key to decrypt");
            Console.ReadKey();

            foreach (var file in files)
                encryptor.Decrypt(file);
        }
    }
}
