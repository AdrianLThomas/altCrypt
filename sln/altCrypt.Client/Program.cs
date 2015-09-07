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
        private static IArgs _args;

        static void Main(string[] args)
        {
            Console.WriteLine($"altCrypt [Alpha] ({DateTime.Now.Year})");
            _args = new ArgsParser(args);

            string instructions = GetInstructions();
            if (_args.IsError)
            {
                Console.WriteLine(instructions);
                return;
            }

            Console.WriteLine($"Parameters:\r\n{_args.ToString()}");
            Console.WriteLine($"Started: {DateTime.Now}");

            var encryptor = new StreamEncryptor(new Key(_args.Key), _args.Algorithm);

            switch (_args.Command)
            {
                case Command.Encrypt:
                    if (_args.Switches.HasFlag(Switch.Directory))
                        EncryptDirectory(encryptor);
                    else
                        EncryptFile(encryptor);
                    break;
                case Command.Decrypt:
                    if (_args.Switches.HasFlag(Switch.Directory))
                        DecryptDirectory(encryptor);
                    else
                        DecryptFile(encryptor);
                    break;
            }

            Console.WriteLine($"Finished: {DateTime.Now}");
            if (System.Diagnostics.Debugger.IsAttached) Console.ReadLine();
        }

        private static void EncryptFile(StreamEncryptor encryptor)
        {
            IFile<Stream> file = new LocalFile(_args.Path);
            encryptor.Encrypt(file);
        }

        private static void DecryptFile(StreamEncryptor encryptor)
        {
            IFile<Stream> file = new LocalFile(_args.Path);
            encryptor.Decrypt(file);
        }

        private static void EncryptDirectory(StreamEncryptor encryptor)
        {
            IDirectory<Stream> directory = new LocalDirectory(_args.Path);
            IEnumerable<IFile<Stream>> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                encryptor.Encrypt(file);
        }

        private static void DecryptDirectory(StreamEncryptor encryptor)
        {
            IDirectory<Stream> directory = new LocalDirectory(_args.Path);
            IEnumerable<IFile<Stream>> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                encryptor.Decrypt(file);
        }

        private static string GetInstructions()
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
