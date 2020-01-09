using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Test04
{
    public class TestScript04 : MonoBehaviour
    {
        private Stopwatch _stopwatch;
        private async void Start()
        {
            _stopwatch = Stopwatch.StartNew();

            //await TaskAwaitACoroutineWithEx();

            StartCoroutine(CoroutineAwaitATaskWithEx());
        }
        
        private async Task TaskAwaitACoroutineWithEx()
        {
            await TaskAsyncCountDown(2, "precoro");
            await this.StartCoroutineAsync(CoroutineCountDown(3, "coro")); 
            await TaskAsyncCountDown(2, "postcoro");
        }
        
        public IEnumerator CoroutineAwaitATaskWithEx()
        {
            yield return CoroutineCountDown(2, "pretask");
            var task = TaskAsyncCountDown(3, "task");
            yield return task.AsCoroutine(); 
            LogHelper.LogToTUnityConsole("TDone", "task", _stopwatch.ElapsedMilliseconds);
            yield return CoroutineCountDown(2, "posttask");
        }
        
        
        private IEnumerator CoroutineCountDown(int count, string flag = "")
        {
            for (var i = count; i >=0; i--)
            {
                LogHelper.LogToTUnityConsole(i, flag, _stopwatch.ElapsedMilliseconds);
                yield return new WaitForSeconds(1);
            }
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
    }
}
