using NChavatte.HumanOrientedSerialization.Common;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NChavatte.HumanOrientedSerialization.CLI.Business
{
    internal static class FileDeserializer
    {
        public static Task<int> DeserializeFileAsync(FileInfo serialFormFile, FileInfo outputFile)
            => Task.Factory.StartNew(() => DeserializeFile(serialFormFile, outputFile));

        private static int DeserializeFile(FileInfo serialFormFile, FileInfo outputFile)
        {
            if (!TryReadSerialFormFile(serialFormFile, out string serialForm))
                return -3;

            if (!TryConvertSerialFormToBytes(serialForm, out byte[] bytes))
                return -4;

            if (!TryWriteOutputFile(bytes, outputFile))
                return -5;

            return 0;
        }

        private static bool TryReadSerialFormFile(FileInfo serialFormFile, out string serialForm)
        {
            try
            {
                using (Stream fileStream = serialFormFile.OpenRead())
                using (TextReader reader = new StreamReader(fileStream))
                {
                    serialForm = reader.ReadToEnd();
                    return true;
                }
            }
            catch (Exception)
            {
                serialForm = null;
                Console.Error.WriteLine("Cannot read serial form file");
                return false;
            }
        }

        private static bool TryConvertSerialFormToBytes(string serialForm, out byte[] bytes)
        {
            bytes = null;

            try
            {
                DeserializationResult result = HOS.Deserialize(serialForm);
                if (!result.IsError)
                {
                    bytes = result.Content;
                    return true;
                }

                DisplayDeserializationError(result.Error);

                return false;
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine("Unmanaged error");
                Console.Error.WriteLine(exc);
                return false;
            }
        }

        private static void DisplayDeserializationError(DeserializationError error)
        {
            switch (error.ErrorType)
            {
                case DeserializationErrorType.WordMalformed:
                    Console.Error.WriteLine(
                        $"Line {error.LineNumber}: word {error.WordNumber} malformed");
                    break;
                case DeserializationErrorType.ParityBitDoesNotMatch:
                    Console.Error.WriteLine(
                        $"Line {error.LineNumber}: word {error.WordNumber} parity bit does not match");
                    break;
                case DeserializationErrorType.LineCheckSumDoesNotMatch:
                    Console.Error.WriteLine(
                        $"Line {error.LineNumber} check-sum does not match");
                    break;
                case DeserializationErrorType.LineCheckSumMissing:
                    Console.Error.WriteLine(
                        $"Line {error.LineNumber} check-sum is missing");
                    break;
                case DeserializationErrorType.ContentLengthDoesNotMatch:
                    Console.Error.WriteLine(
                        "Content length does not match");
                    break;
                case DeserializationErrorType.ContentLengthMissing:
                    Console.Error.WriteLine(
                        "Content length is missing");
                    break;
                case DeserializationErrorType.OpeningLineMissing:
                    Console.Error.WriteLine(
                        "Opening line is missing");
                    break;
                case DeserializationErrorType.ClosingLineMissing:
                    Console.Error.WriteLine(
                        "Closing line is missing");
                    break;
                default:
                    throw new NotSupportedException(error.ErrorType.ToString());
            }
        }

        private static bool TryWriteOutputFile(byte[] bytes, FileInfo outputFile)
        {
            try
            {
                if (outputFile.Exists)
                    outputFile.Delete();

                using (Stream fileStream = outputFile.Create())
                {
                    fileStream.Write(bytes, 0, bytes.Length);
                    return true;
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Cannot write output file");
                return false;
            }
        }
    }
}
