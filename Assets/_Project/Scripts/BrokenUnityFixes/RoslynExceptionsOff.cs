using System.IO;
using System.Text.RegularExpressions;
//using UnityEditor;
//using UnityEditor;
using UnityEngine;

//[InitializeOnLoad]
public static class RoslynExceptionsOff
{
#if UNITY_EDITOR
    //static RoslynExceptionsOff() => Application.logMessageReceived += OnLogMessageReceived;

    //private static void OnLogMessageReceived(string message, string _, LogType logType)
    //{
    //    if (logType != LogType.Exception)
    //        return;

    //    const string pattern =
    //                @"^DirectoryNotFoundException: Could not find " +
    //                @"a part of the path ('|"")Temp(\\|/)RoslynAnalysisRunner";

    //    if (Regex.IsMatch(message, pattern))
    //    {
    //        Directory.CreateDirectory("Temp/RoslynAnalysisRunner");
    //    }
    //}
#endif
}
