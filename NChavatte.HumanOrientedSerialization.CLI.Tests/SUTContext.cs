﻿using NChavatte.HumanOrientedSerialization.CLI.Tests.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace NChavatte.HumanOrientedSerialization.CLI.Tests
{
    internal class SUTContext : IDisposable
    {
        private readonly List<string> _tempFilePaths;

        public SUTContext()
        {
            _tempFilePaths = new List<string>();
            Process = new Process();
        }

        public Process Process { get; }

        public string Arguments
        {
            get => Process.StartInfo.Arguments;
            set => Process.StartInfo.Arguments = $"{DllPath} {value}";
        }

        private string DllPath
            => $"\"{Path.Combine(TestContext.CurrentContext.TestDirectory, "hos-cli.dll")}\"";

        public string WriteResourceIntoFile(string resourceName)
        {
            string path = GetTempFile();
            ResourceProvider.WriteResourceIntoFile(resourceName, path);
            return path;
        }

        public string GetTempFile()
        {
            string path = Path.GetTempFileName();
            _tempFilePaths.Add(path);
            return path;
        }

        public bool StartProcessAndWait()
        {
            string sutPath = "dotnet";
            Process.StartInfo.FileName = sutPath;
            Process.StartInfo.UseShellExecute = false;
            bool isStarted = Process.Start();
            if (isStarted)
                Process.WaitForExit();
            else
                TestContext.Error.WriteLine("SUT process did not start");
            return isStarted;
        }

        public void Dispose()
        {
            Process?.Dispose();

            foreach (string path in _tempFilePaths)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}