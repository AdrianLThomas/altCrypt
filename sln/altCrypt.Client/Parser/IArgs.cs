using System;
using altCrypt.Client.CommandLine.Input;

namespace altCrypt.Client.CommandLine.Parser
{
    public interface IArgs
    {
        bool IsError { get; }
        Command Command { get; }
        Switch Switch { get; }
        string Path { get; }
    }
}
