using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcnesTools
{
    public static class Debug
    {
        public static void Log(string text, Color color)
        {
            #if UNITY_EDITOR
            UnityEngine.Debug.Log($"<color={color}>{text}</color>");
            #endif
        }

        public static void Log(string text, string colorHex)
        {
            #if UNITY_EDITOR
            if (ColorUtility.TryParseHtmlString(colorHex, out var color))
            {
                UnityEngine.Debug.Log($"<color={color}>{text}</color>");
            }
            #endif
        }

        public static void Log(string text)
        {
            #if UNITY_EDITOR
            UnityEngine.Debug.Log(text);
            #endif
        }

        public static void LogWarning(string text)
        {
            #if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(text);
            #endif
        }

        public static void LogError(string text)
        {
            #if UNITY_EDITOR
            UnityEngine.Debug.LogError(text);
            #endif
        }
    }
}