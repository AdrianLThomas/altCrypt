using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using altCrypt.Core.Encryption;
using altCrypt.Core.FileSystem;
using altCrypt.Core.x86.Encryption;
using altCrypt.Core.x86.FileSystem;
using System.Linq.Expressions;
using altCrypt.Client.CommandLine.Input;
using altCrypt.Client.CommandLine.Parser;

namespace altCrypt.Client.CommandLine
{
    static class Program
    {
        static void Main(string[] args)
        {
            //TODO - refactor
            //TODO - remove hardcoded AES

            Console.WriteLine($"altCrypt [Alpha] ({DateTime.Now.Year})");
            var argsParser = new ArgsParser(args);

            string intro = GetIntro();
            if (argsParser.IsError)
            {
                Console.WriteLine(intro);
                return;
            }

            Console.WriteLine($"Parameters: {argsParser.ToString()}");
            Console.WriteLine($"Started: {DateTime.Now}");

            IKey key = new Key(argsParser.Key);
            SymmetricAlgorithm encryptionProvider = new AesCryptoServiceProvider();
            StreamEncryptor encryptor = new StreamEncryptor(key, encryptionProvider);

            string path = argsParser.Path;
            switch (argsParser.Command)
            {
                case Command.Encrypt:
                    if (argsParser.Switches.HasFlag(Switch.Directory))
                        EncryptDirectory(encryptor, path);
                    else
                        EncryptFile(encryptor, path);
                    break;
                case Command.Decrypt:
                    if (argsParser.Switches.HasFlag(Switch.Directory))
                        DecryptDirectory(encryptor, path);
                    else
                        DecryptFile(encryptor, path);
                    break;
            }

            Console.WriteLine($"Finished: {DateTime.Now}");
            if (System.Diagnostics.Debugger.IsAttached) Console.ReadLine();
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

        private static string GetIntro()
        {
            return @"
Usage: altCrypt <command> <switches>

<Commands>
e : encrypt
d : decrypt

<Switches>
-k : key (required)
-s : key size
-d : directory
-f : file
-a : algorithm (AES, DES, RC2, Rijndael, TripleDES)

<Example>
altCrypt e -k ""Pass@w0rd1"" -s 128 -d ""C:\temp"" -a AES
";
        }
    }
}
