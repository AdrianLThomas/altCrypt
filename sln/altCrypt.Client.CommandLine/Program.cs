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
        private static FileEncryptor _encryptor;

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

            _encryptor = new FileEncryptor(new Key(_args.Key), new RandomIV(),  _args.Algorithm);

            switch (_args.Command)
            {
                case Command.Encrypt:
                    if (_args.Switches.HasFlag(Switch.Directory))
                        EncryptDirectory();
                    else
                        EncryptFile();
                    break;
                case Command.Decrypt:
                    if (_args.Switches.HasFlag(Switch.Directory))
                        DecryptDirectory();
                    else
                        DecryptFile();
                    break;
            }

            Console.WriteLine($"Finished: {DateTime.Now}");
            if (System.Diagnostics.Debugger.IsAttached) Console.ReadLine();
        }

        private static void EncryptFile()
        {
            IFile file = new LocalFile(_args.Path);
            _encryptor.Encrypt(file);
        }

        private static void DecryptFile()
        {
            IFile file = new LocalFile(_args.Path);
            _encryptor.Decrypt(file);
        }

        private static void EncryptDirectory()
        {
            IDirectory directory = new LocalDirectory(_args.Path);
            IEnumerable<IFile> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                _encryptor.Encrypt(file);
        }

        private static void DecryptDirectory()
        {
            IDirectory directory = new LocalDirectory(_args.Path);
            IEnumerable<IFile> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                _encryptor.Decrypt(file);
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
