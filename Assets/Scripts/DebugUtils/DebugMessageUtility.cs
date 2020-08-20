using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Minecraft.DebugUtils
{
    public static class DebugMessageUtility
    {
        private const string Condition = "UNITY_EDITOR";
        
        [Conditional(Condition)]
        public static void Log(this IDebugMessageSender sender, params object[] messages)
        {
            Log(sender, null, messages);
        }

        [Conditional(Condition)]
        public static void Log(this IDebugMessageSender sender, Object context, params object[] messages)
        {
            if (!sender.DisableLog)
            {
                Debug.Log(Format(sender.DisplayName, messages), context);
            }
        }

        [Conditional(Condition)]
        public static void LogWarning(this IDebugMessageSender sender, params object[] messages)
        {
            LogWarning(sender, null, messages);
        }

        [Conditional(Condition)]
        public static void LogWarning(this IDebugMessageSender sender, Object context, params object[] messages)
        {
            if (!sender.DisableLog)
            {
                Debug.LogWarning(Format(sender.DisplayName, messages), context);
            }
        }

        [Conditional(Condition)]
        public static void LogError(this IDebugMessageSender sender, params object[] messages)
        {
            LogError(sender, null, messages);
        }

        [Conditional(Condition)]
        public static void LogError(this IDebugMessageSender sender, Object context, params object[] messages)
        {
            if (!sender.DisableLog)
            {
                Debug.LogError(Format(sender.DisplayName, messages), context);
            }
        }

        private static string Format(string senderName, object[] messages)
        {
            return $"<color=blue><b>[{senderName}]</b></color> {string.Concat(messages)}";
        }
    }
}