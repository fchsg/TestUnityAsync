using System.Runtime.CompilerServices;
using System.Threading;


namespace Utils
{
    public static class LogHelper
    {
        public static void LogToTUnityConsole(object content, string flag, float milliseconds,  [CallerMemberName] string callerName = null)
        {
            UnityEngine.Debug.Log($"{callerName}：\t{flag} {content}\t at {milliseconds} \t in thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
