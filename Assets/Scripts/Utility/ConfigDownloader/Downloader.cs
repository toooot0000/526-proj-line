using System;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Directory = System.IO.Directory;

namespace Utility.ConfigDownloader{
#if UNITY_EDITOR
    public static class Downloader{
        private static string _pyDir = "";
        
        [MenuItem("CustomMethods/Download ^#d")]
        public static void DownloadConfig(){
            var process = new Process();
#if UNITY_EDITOR_WIN
            var args = $"\"{Directory.GetCurrentDirectory()}\\Assets\\Scripts\\Utility\\ConfigDownloader\\DownloadConfig.py\"";
#elif UNITY_EDITOR_OSX
            var args = $"\"{Directory.GetCurrentDirectory()}/Assets/Scripts/Utility/ConfigDownloader/DownloadConfig.py\"";
#endif
            process.StartInfo = new(){
                FileName = GetPythonPath(),
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
#if UNITY_EDITOR_WIN
                WorkingDirectory = $"{Directory.GetCurrentDirectory()}\\Assets\\Scripts\\Utility\\ConfigDownloader\\"
#elif UNITY_EDITOR_OSX
                WorkingDirectory = $"{Directory.GetCurrentDirectory()}/Assets/Scripts/Utility/ConfigDownloader/"
#endif
            };
            process.Start();
            Debug.Log(process.StandardOutput.ReadToEnd());
            Debug.Log(process.StandardError.ReadToEnd());
            process.WaitForExit();
        }

        private static string GetPythonPath(){
            if (_pyDir != ""){
                return _pyDir;
            }
#if UNITY_EDITOR_WIN
            var process = new Process();
            process.StartInfo = new(){
                FileName = "where.exe",
                Arguments = "python",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            process.Start();
            _pyDir = process.StandardOutput.ReadToEnd().Split("\n")[0];
            _pyDir = _pyDir.Substring(0, _pyDir.Length - 1);
            process.WaitForExit();
            return _pyDir;
#elif UNITY_EDITOR_OSX
            var process = new Process();
            process.StartInfo = new(){
                FileName = "which",
                Arguments = "python3",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            process.Start();
            _pyDir = process.StandardOutput.ReadLine();
            process.WaitForExit();
            return _pyDir;
#endif
        }
    }
#endif
}