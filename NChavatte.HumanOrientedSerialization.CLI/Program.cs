using NChavatte.HumanOrientedSerialization.CLI.CommandProviders;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

namespace NChavatte.HumanOrientedSerialization.CLI
{
    internal class Program
    {
        static int Main(string[] args)
        {

            RootCommand rootCommand = new RootCommand("Serializes/deserializes files");
            GetCommands().ToList().ForEach(rootCommand.AddCommand);
            return rootCommand.Invoke(args);
        }

        private static IEnumerable<Command> GetCommands()
        {
            Type providerAbstractType = typeof(ICommandProvider);
            return providerAbstractType.Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(providerAbstractType))
                .Select(t => (ICommandProvider)Activator.CreateInstance(t))
                .Select(p => p.GetCommand());
        }
    }
}
