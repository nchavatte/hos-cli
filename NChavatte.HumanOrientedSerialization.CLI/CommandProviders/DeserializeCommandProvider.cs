using NChavatte.HumanOrientedSerialization.CLI.Business;
using System.CommandLine;
using System.IO;

namespace NChavatte.HumanOrientedSerialization.CLI.CommandProviders
{
    internal class DeserializeCommandProvider : ICommandProvider
    {
        public Command GetCommand()
        {
            Command deserializeCommand = new Command("deserialize", "Converts a serial form to binary and writes it into a file");

            Argument<FileInfo> serialformArgument = new Argument<FileInfo>("serial form", "Serial form file path");
            deserializeCommand.AddArgument(serialformArgument);

            Argument<FileInfo> outputArgument = new Argument<FileInfo>("output", "Output file path");
            deserializeCommand.AddArgument(outputArgument);

            deserializeCommand.SetHandler<FileInfo, FileInfo>(
                FileDeserializer.DeserializeFileAsync
                , serialformArgument
                , outputArgument);
            
            return deserializeCommand;
        }
    }
}
