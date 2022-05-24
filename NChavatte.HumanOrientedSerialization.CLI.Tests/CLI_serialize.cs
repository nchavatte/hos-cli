using NChavatte.HumanOrientedSerialization.CLI.Tests.Resources;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NChavatte.HumanOrientedSerialization.CLI.Tests
{
    [TestFixture]
    public class CLI_serialize
    {
        private SUTContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new SUTContext();
        }

        [TestCase("source.0.bin", "serial-form.0.txt")]
        public void Should_return_serial_form(string sourceName, string expectedSerialFormName)
        {
            // Arrange
            string sourcePath = _context.WriteResourceIntoFile(sourceName);
            _context.Arguments = $"serialize \"{sourcePath}\"";
            _context.Process.StartInfo.RedirectStandardOutput = true;

            // Act
            bool started = _context.StartProcessAndWait();

            // Assert
            //Assert.That(_context.Process.ExitCode, Is.Zero);
            Assert.IsTrue(started);
            string expectedSerialForm = ResourceProvider.GetResourceText(expectedSerialFormName).Trim();
            string actualSerialForm = _context.Process.StandardOutput.ReadToEnd().Trim();
            Assert.AreEqual(expectedSerialForm, actualSerialForm);
        }

        [TestCase("")]
        [TestCase("not-a-file.bin")]
        public void Should_return_error_code_if_no_source_file(string badSourceFilePath)
        {
            // Arrange
            _context.Arguments = $"serialize {badSourceFilePath}";
            _context.Process.StartInfo.RedirectStandardOutput = true;

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