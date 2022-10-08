using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcnesTools
{
    public static class Debug
    {
        public static void Log(object text, Color color)
        {
            #if UNITY_EDITOR
            UnityEngine.Debug.Log($"<color={color}>{text}</color>");
            #endif
        }

        public static void Log(object text, string colorHex)
        {
            #if UNITY_EDITOR
            if (ColorUtility.TryParseHtmlString(colorHex, out var color))
            {
                UnityEngine.Debug.Log($"<color={color}>{text}</color>");
            }
            #endif
        }

        public static void Log(object text)
        {
            #if UNITY_EDITOR
            UnityEngine.Debug.Log(text);
            #endif
        }

        public static void LogWarning(object text)
        {
            #if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(text);
            #endif
        }

        public static void LogError(object text)
        {
            #if UNITY_EDITOR
            UnityEngine.Debug.LogError(text);
            #endif
        }
    }
}