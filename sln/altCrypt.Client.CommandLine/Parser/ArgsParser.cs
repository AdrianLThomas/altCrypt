using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Schema;
using altCrypt.Client.CommandLine.Input;
using Switch = altCrypt.Client.CommandLine.Input.Switch;

namespace altCrypt.Client.CommandLine.Parser
{
    public class ArgsParser : IArgs
    {
        private readonly string[] _args;

        public bool IsError { get; private set; }
        public Command Command { get; private set; } = Command.None;
        public Switch Switches { get; private set; } = Switch.None;
        public string Path { get; private set; }
        public string Key { get; private set; }
        public int KeySize { get; private set; } = 128;
        public SymmetricAlgorithm Algorithm { get; private set; } = Aes.Create();

        public ArgsParser(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            //TODO - refactor

            _args = args;

            //Validate Length
            {
                if (args.Length <= 0)
                {
                    IsError = true;
                    return;
                }
            }

            //Validate Command
            {
                string command = args[0];
                if (command.Equals(InputConstants.EncryptCommand, StringComparison.OrdinalIgnoreCase))
                {
                    Command = Command.Encrypt;
                }
                else if (command.Equals(InputConstants.DecryptCommand, StringComparison.OrdinalIgnoreCase))
                {
                    Command = Command.Decrypt;
                }
                else
                {
                    IsError = true;
                    return;
                }
            }

            //Validate Switches
            {
                //Key
                int passwordSwitchIndex = Array.IndexOf(args, InputConstants.KeySwitch);
                if (passwordSwitchIndex > 0)
                {
                    Key = args[passwordSwitchIndex + 1];
                    Switches = Switches | Switch.Key;
                }
                else
                {
                    IsError = true;
                    return;
                }

                //Directory / File
                int directorySwitchIndex = Array.IndexOf(args, InputConstants.DirectorySwitch);
                int fileSwitchIndex = Array.IndexOf(args, InputConstants.FileSwitch);
                if (directorySwitchIndex > 0)
                {
                    Path = args[directorySwitchIndex + 1];
                    Switches = Switches | Switch.Directory;
                }
                else if (fileSwitchIndex > 0)
                {
                    Path = args[fileSwitchIndex + 1];
                    Switches = Switches | Switch.File;
                }
                else
                {
                    IsError = true;
                    return;
                }

                //Algorithm
                int algorithmSwitchIndex = Array.IndexOf(args, InputConstants.AlgorithmSwitch);
                if (algorithmSwitchIndex > 0)
                {
                    string algorithmName = args[algorithmSwitchIndex + 1];

                    Algorithm = SymmetricAlgorithm.Create(algorithmName);

                    Switches = Switches | Switch.Algorithm;
                }

                //Key Size
                int keySizeSwitchIndex = Array.IndexOf(args, InputConstants.KeySizeSwitch);
                if (keySizeSwitchIndex > 0)
                {
                    int keySize = int.Parse(args[keySizeSwitchIndex + 1]);
                    KeySize = keySize;
                    Algorithm.KeySize = keySize;

                    Switches = Switches | Switch.KeySize;
                }
            }
        }

        public override string ToString()
        {
            return $"Command: {Command}\r\nSwitches: {Switches}\r\nPath: {Path}\r\nKey: {new string('*', Key.Length)}\r\nKey Size: {KeySize}\r\n";
        }
    }
}
