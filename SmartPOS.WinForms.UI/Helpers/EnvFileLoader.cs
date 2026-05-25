using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartPOS.WinForms.UI.Helpers
{
    internal static class EnvFileLoader
    {
        private static readonly string[] EnvFileNames =
        {
            ".env",
            ".env.local"
        };

        public static void Load()
        {
            foreach (string filePath in GetEnvFiles())
            {
                LoadFile(filePath);
            }
        }

        private static IEnumerable<string> GetEnvFiles()
        {
            var directories = GetCandidateDirectories()
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Reverse()
                .ToList();

            foreach (string directory in directories)
            {
                foreach (string fileName in EnvFileNames)
                {
                    string filePath = Path.Combine(directory, fileName);
                    if (File.Exists(filePath))
                    {
                        yield return filePath;
                    }
                }
            }
        }

        private static IEnumerable<string> GetCandidateDirectories()
        {
            string current = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < 8 && !string.IsNullOrWhiteSpace(current); i++)
            {
                yield return current;

                DirectoryInfo parent = Directory.GetParent(current);
                if (parent == null)
                {
                    yield break;
                }

                current = parent.FullName;
            }
        }

        private static void LoadFile(string filePath)
        {
            foreach (string rawLine in File.ReadAllLines(filePath))
            {
                if (!TryParseLine(rawLine, out string key, out string value))
                {
                    continue;
                }

                Environment.SetEnvironmentVariable(key, value, EnvironmentVariableTarget.Process);
            }
        }

        private static bool TryParseLine(string rawLine, out string key, out string value)
        {
            key = null;
            value = null;

            if (string.IsNullOrWhiteSpace(rawLine))
            {
                return false;
            }

            string line = rawLine.Trim();
            if (line.StartsWith("#", StringComparison.Ordinal))
            {
                return false;
            }

            if (line.StartsWith("export ", StringComparison.OrdinalIgnoreCase))
            {
                line = line.Substring("export ".Length).TrimStart();
            }

            int separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
            {
                return false;
            }

            key = line.Substring(0, separatorIndex).Trim();
            value = line.Substring(separatorIndex + 1).Trim();

            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            value = Unquote(value);
            return true;
        }

        private static string Unquote(string value)
        {
            if (value == null || value.Length < 2)
            {
                return value;
            }

            char first = value[0];
            char last = value[value.Length - 1];
            if ((first == '"' && last == '"') || (first == '\'' && last == '\''))
            {
                return value.Substring(1, value.Length - 2);
            }

            return value;
        }
    }
}
