using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace NChavatte.HumanOrientedSerialization.CLI.Tests
{
    [TestFixture]
    public class CLI_bad_command
    {
        private SUTContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new SUTContext();
        }

        [TestCase("")]
        [TestCase("command-not-found")]
        public void Shoud_return_error_exit_code_on_bad_command(string commandLineArgs)
        {
            // Arrange
            string sutPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "hos-cli.exe");
            _context.Process.StartInfo = new ProcessStartInfo(sutPath, commandLineArgs);

            // Act
            _context.StartProcessAndWait();

            // Assert
            TestContext.WriteLine($"Exit code: {_context.Process.ExitCode}");
            Assert.That(_context.Process.ExitCode, Is.EqualTo(1));
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }
    }
}