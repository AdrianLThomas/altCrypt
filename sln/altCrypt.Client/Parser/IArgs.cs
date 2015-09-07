using System;
using System.Security.Cryptography;
using altCrypt.Client.CommandLine.Input;

namespace altCrypt.Client.CommandLine.Parser
{
    public interface IArgs
    {
        bool IsError { get; }
        Command Command { get; }
        Switch Switches { get; }
        string Path { get; }
        string Key { get; }
        int KeySize { get; }
        SymmetricAlgorithm Algorithm { get; }
    }
}
