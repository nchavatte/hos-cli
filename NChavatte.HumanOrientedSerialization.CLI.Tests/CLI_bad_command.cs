using NUnit.Framework;

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
            _context.Process.StartInfo.Arguments = commandLineArgs;

            // Act
            bool started = _context.StartProcessAndWait();

            // Assert
            Assert.IsTrue(started);
            Assert.That(_context.Process.ExitCode, Is.Not.Zero);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }
    }
}