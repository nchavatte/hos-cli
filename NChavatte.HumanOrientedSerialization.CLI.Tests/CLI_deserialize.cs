using NChavatte.HumanOrientedSerialization.CLI.Tests.Resources;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace NChavatte.HumanOrientedSerialization.CLI.Tests
{
    [TestFixture]
    public class CLI_deserialize
    {
        private SUTContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new SUTContext();
        }

        [TestCase("serial-form.0.txt", "source.0.bin")]
        public void Should_write_deserialized_form_into_file(string serialFormName, string expectedBinaryName)
        {
            // Arrange
            string serialFormPath = _context.WriteResourceIntoFile(serialFormName);
            string actualBinaryPath = _context.GetTempFile();
            string sutPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "hos-cli.exe");
            _context.Process.StartInfo = new ProcessStartInfo(sutPath, $"deserialize \"{serialFormPath}\" \"{actualBinaryPath}\"");

            // Act
            _context.StartProcessAndWait();

            // Assert
            Assert.That(_context.Process.ExitCode, Is.Zero);
            Assert.IsTrue(File.Exists(actualBinaryPath));
            byte[] expectedBinary = ResourceProvider.GetResourceBytes(expectedBinaryName);
            byte[] actualBinary = File.ReadAllBytes(actualBinaryPath);
            CollectionAssert.AreEqual(expectedBinary, actualBinary);
        }

        [Test]
        public void Should_return_error_code_if_input_file_does_not_exist()
        {
            // Arrange
            string serialFormPath = "not-serial-form-file.txt";
            string actualBinaryPath = _context.GetTempFile();
            string sutPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "hos-cli.exe");
            _context.Process.StartInfo = new ProcessStartInfo(sutPath, $"deserialize \"{serialFormPath}\" \"{actualBinaryPath}\"");

            // Act
            _context.StartProcessAndWait();

            // Assert
            Assert.That(_context.Process.ExitCode, Is.Not.Zero);
        }

        [Test]
        public void Should_return_error_code_if_no_output_file_path()
        {
            // Arrange
            string serialFormPath = _context.WriteResourceIntoFile("serial-form.0.txt");
            string sutPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "hos-cli.exe");
            _context.Process.StartInfo = new ProcessStartInfo(sutPath, $"deserialize \"{serialFormPath}\"");

            // Act
            _context.StartProcessAndWait();

            // Assert
            Assert.That(_context.Process.ExitCode, Is.Not.Zero);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }
    }
}