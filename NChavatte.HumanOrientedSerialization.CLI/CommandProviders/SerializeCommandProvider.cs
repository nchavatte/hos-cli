using NChavatte.HumanOrientedSerialization.CLI.Business;
using System.CommandLine;
using System.IO;

namespace NChavatte.HumanOrientedSerialization.CLI.CommandProviders
{
    internal class SerializeCommandProvider : ICommandProvider
    {
        public Command GetCommand()
        {
            Command serializeCommand = new Command("serialize", "Returns serial form of a file to standard output");

            Argument<FileInfo> sourceArgument = new Argument<FileInfo>("source", "Source file path");
            serializeCommand.AddArgument(sourceArgument);

            serializeCommand.SetHandler<FileInfo>(
                FileSerializer.SerializeFileAsync
                , sourceArgument);

            return serializeCommand;
        }
    }
}
