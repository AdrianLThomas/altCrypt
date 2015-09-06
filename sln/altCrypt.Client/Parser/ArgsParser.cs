using System;
using System.Linq;
using System.Xml.Schema;
using altCrypt.Client.CommandLine.Input;

namespace altCrypt.Client.CommandLine.Parser
{
    public class ArgsParser : IArgs
    {
        private readonly string[] _args;

        public bool IsError { get; private set; } = false;
        public Command Command { get; private set; } = Command.None;
        public Switch Switch { get; private set; } = Switch.None;
        public string Path { get; private set; } = null;
        public string Password { get; private set; } = null;

        public ArgsParser(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

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
                //Password
                int passwordSwitchIndex = Array.IndexOf(args, InputConstants.KeySwitch);
                if (passwordSwitchIndex > 0)
                {
                    Password = args[passwordSwitchIndex + 1];
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
                    Switch = Switch.Directory;
                    Path = args[directorySwitchIndex + 1];
                }
                else if (fileSwitchIndex > 0)
                {
                    Switch = Switch.File;
                    Path = args[fileSwitchIndex + 1];
                }
                else
                {
                    IsError = true;
                    return;
                }
            }
        }
    }
}
