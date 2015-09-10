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
using System.Threading.Tasks;
using System.Threading;

namespace altCrypt.Client.CommandLine
{
    static class Program
    {
        private static IArgs _args;
        private static FileEncryptor _encryptor;

        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            MainAsync(args, cts.Token).Wait();
        }

        static async Task MainAsync(string[] args, CancellationToken token)
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

            _encryptor = new FileEncryptor(new Key(_args.Key), new RandomIV(), _args.Algorithm);

            switch (_args.Command)
            {
                case Command.Encrypt:
                    if (_args.Switches.HasFlag(Switch.Directory))
                        await EncryptDirectoryAsync();
                    else
                        await EncryptFileAsync();
                    break;
                case Command.Decrypt:
                    if (_args.Switches.HasFlag(Switch.Directory))
                        await DecryptDirectoryAsync();
                    else
                        await DecryptFileAsync();
                    break;
            }

            Console.WriteLine($"Finished: {DateTime.Now}");
            if (System.Diagnostics.Debugger.IsAttached) Console.ReadLine();
        }

        private static async Task EncryptFileAsync()
        {
            IFile file = new LocalFile(_args.Path);
            await _encryptor.EncryptAsync(file);
        }

        private static async Task DecryptFileAsync()
        {
            IFile file = new LocalFile(_args.Path);
            await _encryptor.DecryptAsync(file);
        }

        private static async Task EncryptDirectoryAsync()
        {
            IDirectory directory = new LocalDirectory(_args.Path);
            IEnumerable<IFile> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                await _encryptor.EncryptAsync(file);
        }

        private static async Task DecryptDirectoryAsync()
        {
            IDirectory directory = new LocalDirectory(_args.Path);
            IEnumerable<IFile> files = directory.GetFilesIncludingSubdirectories();
            foreach (var file in files)
                await _encryptor.DecryptAsync(file);
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
