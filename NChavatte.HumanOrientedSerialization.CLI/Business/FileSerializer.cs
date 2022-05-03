using NChavatte.HumanOrientedSerialization.Common;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NChavatte.HumanOrientedSerialization.CLI.Business
{
    internal static class FileSerializer
    {
        public static Task<int> SerializeFileAsync(FileInfo sourceFile)
            => Task.Factory.StartNew(() => SerializeFile(sourceFile));

        private static int SerializeFile(FileInfo sourceFile)
        {
            if (!TryReadSourceFile(sourceFile, out byte[] source))
                return -1;

            if (!TrySerializeBytes(source))
                return -2;

            return 0;
        }

        private static bool TryReadSourceFile(FileInfo sourceFile, out byte[] content)
        {
            try
            {
                using (Stream fileStream = sourceFile.OpenRead())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    content = memoryStream.ToArray();
                    return true;
                }
            }
            catch (Exception)
            {
                content = null;
                Console.Error.WriteLine("Cannot read source file");
                return false;
            }
        }

        private static bool TrySerializeBytes(byte[] bytes)
        {
            try
            {
                string serialForm = HOS.Serialize(bytes);
                Console.Out.WriteLine(serialForm);
                return true;
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine("Unmanaged error");
                Console.Error.WriteLine(exc);
                return false;
            }
        }
    }
}
