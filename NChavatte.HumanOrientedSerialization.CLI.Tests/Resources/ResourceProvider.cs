using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace NChavatte.HumanOrientedSerialization.CLI.Tests.Resources
{
    internal static class ResourceProvider
    {
        public static string WriteResourceIntoFile(string resourceName, string filePath)
        {
            File.WriteAllBytes(filePath, GetResourceBytes(resourceName));
            return filePath;
        }
        public static string GetResourceText(string resourceName)
            => Encoding.ASCII.GetString(GetResourceBytes(resourceName));

        public static byte[] GetResourceBytes(string resourceName)
        {
            try
            {
                Type type = typeof(ResourceProvider);
                string resourceFullName = $"{type.Namespace}.{resourceName}";
                using (Stream stream = type.Assembly.GetManifestResourceStream(resourceFullName))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception)
            {
                TestContext.Error.WriteLine($"Cannot read test resource \"{resourceName}\"");
                throw;
            }
        }
    }
}
