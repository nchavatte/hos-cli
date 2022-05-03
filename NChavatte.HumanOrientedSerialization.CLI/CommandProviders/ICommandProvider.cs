using System.CommandLine;

namespace NChavatte.HumanOrientedSerialization.CLI.CommandProviders
{
    internal interface ICommandProvider
    {
        Command GetCommand();
    }
}
