using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.Encryption;
using altCrypt.Core.x86.FileSystem;

namespace altCrypt.Client.CommandLine
{
    static class Program
    {
        static void Main(string[] args)
        {
            //TODO - refactor this mess and stablise :-)
            //TODO - remove hardcoded AES

            Console.WriteLine($"altCrypt [Alpha] ({DateTime.Now.Year})");

            string intro = @"
Usage: altCrypt <command> <switches>

<Commands>
e : encrypt
d : decrypt

<Switches>
-d : directory
-f : file
-k : key

<Example>
altCrypt e -k ""Pass@w0rd1"" -d ""C:\temp""
";
            if (args.Length == 0)
            {
                Console.WriteLine(intro);
                return;
            }
            string command = args[0];
            string switch1Key = args[1];
            string switch1Value = args[2];

            string switch2Key = args[3];
            string switch3Value = args[4];

            IKey key = new Key(switch1Value);
            SymmetricAlgorithm encryptionProvider = new AesCryptoServiceProvider();
            StreamEncryptor encryptor = new StreamEncryptor(key, encryptionProvider);

            string path = switch3Value;
            switch (command)
            {
                case "e":
                    if (switch2Key == "-d")
                        EncryptDirectory(encryptor, path);
                    else
                        EncryptFile(encryptor, path);
                    break;
                case "d":
                    if (switch2Key == "-d")
                        DecryptDirectory(encryptor, path);
                    else
                        DecryptFile(encryptor, path);
                    break;
                default:
                    Console.WriteLine($"Error: Command '{command}' not recognised");
                    return;
            }
        }

        private static void EncryptFile(StreamEncryptor encryptor, string path)
        {
            IFile<Stream> file = new LocalFile(path);
            encryptor.Encrypt(file);
        }

        private static void DecryptFile(StreamEncryptor encryptor, string path)
        {
            IFile<Stream> file = new LocalFile(path);
            encryptor.Decrypt(file);
        }

        private static void EncryptDirectory(StreamEncryptor encryptor, string path)
        {
            IDirectory<Stream> directory = new LocalDirectory(path);
            IEnumerable<IFile<Stream>> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                encryptor.Encrypt(file);
        }

        private static void DecryptDirectory(StreamEncryptor encryptor, string path)
        {
            IDirectory<Stream> directory = new LocalDirectory(path);
            IEnumerable<IFile<Stream>> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                encryptor.Decrypt(file);
        }
    }
}
