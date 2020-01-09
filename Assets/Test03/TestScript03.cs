using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Test03
{
    public class TestScript03 : MonoBehaviour
    {
        private Stopwatch _stopwatch;

        private async void Start()
        {
            _stopwatch = Stopwatch.StartNew();
           
            //await TaskAwaitACoroutine();

            StartCoroutine(CoroutineAwaitATask());
        }

        private static IEnumerator TempCoroutine(IEnumerator coro, System.Action afterExecutionCallback)
        {
            yield return coro;
            afterExecutionCallback(); 
        }

        private async Task TaskAwaitACoroutine()
        {
            await TaskAsyncCountDown(2, "precoro");
            var tcs = new TaskCompletionSource<object>();
            StartCoroutine(TempCoroutine(CoroutineCountDown(3, "coro"), () => tcs.TrySetResult(null)));        
            await tcs.Task;
            await TaskAsyncCountDown(2, "postcoro");
        }
        
        
        public IEnumerator CoroutineAwaitATask()
        {
            yield return CoroutineCountDown(2, "pretask");
 
            var task = TaskAsyncCountDown(3, "task");
            yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled); 
            //Check task's return;
            if (task.IsCompleted)
            {
               LogHelper.LogToTUnityConsole("TDone", "task", _stopwatch.ElapsedMilliseconds);
            }
            yield return CoroutineCountDown(2, "posttask");
        }
        
        private async Task TaskAsyncCountDown(int count, string flag = "")
        {
            for (var i = count; i >= 0; i--)
            {
                LogHelper.LogToTUnityConsole(i, flag, _stopwatch.ElapsedMilliseconds);

                await Task.Delay(1000); //main thread
                //await Task.Delay(1000).ConfigureAwait(false);  //other thread
            }
        }
        
        private IEnumerator CoroutineCountDown(int count, string flag = "")
        {
            for (var i = count; i >=0; i--)
            {
                LogHelper.LogToTUnityConsole(i, flag, _stopwatch.ElapsedMilliseconds);
                yield return new WaitForSeconds(1);
            }
        }
        
    }
}
