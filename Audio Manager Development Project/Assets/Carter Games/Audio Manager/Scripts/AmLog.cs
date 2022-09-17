// ----------------------------------------------------------------------------
// AmLog.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 09/06/2022
// ----------------------------------------------------------------------------

using UnityEngine;

namespace CarterGames.Assets.AudioManager
{
    /// <summary>
    /// Used for sending logs formatted with the asset...
    /// </summary>
    public static class AmLog
    {
        private const string LogPrefix = "<color=#E77A7A><b>Audio Manager</b></color> | ";
        private const string WarningPrefix = "<color=#D6BA64><b>Warning</b></color> | ";
        private const string ErrorPrefix = "<color=#E77A7A><b>Error</b></color> | ";
        private static string WarningCodePrefix(int codeNumber) => $"<color=#D6BA64><b>Warning Code {codeNumber}</b></color> | ";
        private static string ErrorCodePrefix(int codeNumber) => $"<color=#E77A7A><b>Error Code {codeNumber}</b></color> | ";
        
        public static void Normal(string message) => Debug.Log($"{LogPrefix}{message}");
        public static void Normal(string message, Object ctx) => Debug.Log($"{LogPrefix}{message}", ctx);
        public static void Warning(string message) => Debug.Log($"{LogPrefix}{WarningPrefix}{message}");
        public static void WarningWithCode(int code, string message) => Debug.Log($"{LogPrefix}{WarningCodePrefix(code)}{message}");
        public static void Warning(string message, Object ctx) => Debug.Log($"{LogPrefix}{WarningPrefix}{message}", ctx);
        public static void WarningWithCode(int code, string message, Object ctx) => Debug.Log($"{LogPrefix}{WarningCodePrefix(code)}{message}", ctx);
        public static void Error(string message) => Debug.Log($"{LogPrefix}{ErrorPrefix}{message}");
        public static void ErrorWithCode(int code, string message) => Debug.Log($"{LogPrefix}{ErrorCodePrefix(code)}{message}");
        public static void Error(string message, Object ctx) => Debug.Log($"{LogPrefix}{ErrorPrefix}{message}", ctx);
        public static void ErrorWithCode(int code, string message, Object ctx) => Debug.Log($"{LogPrefix}{ErrorCodePrefix(code)}{message}", ctx);
    }
}