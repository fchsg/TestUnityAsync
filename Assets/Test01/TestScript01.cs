using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Test01
{
    public class TestScript01 : MonoBehaviour
    {
        private Stopwatch _stopwatch;

        private async void Start()
        {
            _stopwatch = new Stopwatch(); 
            _stopwatch.Start(); 
            
            // coroutine single
            //StartCoroutine(CoroutineCountDown(3, "BasicCoCall"));
            
            // coroutine parallel
            //StartCoroutine(CoroutineCountDown(3, "BasicCoCallA"));
            //StartCoroutine(CoroutineCountDown(3, "BasicCoCallB")); 
            
            // coroutine sequence
            //StartCoroutine(CoroutineCountDownSeq(3, "BasicCoCallA", "BasicCoCallB")); 
            
            // await single
            //await TaskAsyncCountDown(3,"BasicCoCallA");
            
            // await parallel
            //var a = TaskAsyncCountDown(3, "BasicCoCallA");
            //var b = TaskAsyncCountDown(3, "BasicCoCallB");
            
            // await sequence
            //await TaskAsyncCountDownSeq(3, "BasicCoCallA", "BasicCoCallB");
            
            // await other thread
            await Task.Run(() => TaskAsyncCountDown(3, "BasicCoCallA"));

        }
        
        public async Task TaskAsyncCountDownSeq(int countDown, params string[] flags)
        {
            foreach (var flag in flags)
            {
                await TaskAsyncCountDown(3, flag);
            }
        }
        
        private async Task TaskAsyncCountDown(int count, string flag = "")
        {
            for (var i = count; i >= 0; i--)
            {
                LogHelper.LogToTUnityConsole(i, flag, _stopwatch.ElapsedMilliseconds);

                //await Task.Delay(1000); //main thread
                await Task.Delay(1000).ConfigureAwait(false);  //other thread
            }
        }
        
        public IEnumerator CoroutineCountDownSeq(int countDown, params string[] flags)
        {
            foreach (var flag in flags)
            {
                yield return StartCoroutine(CoroutineCountDown(3, flag)); 
            }
        }
        
        private IEnumerator CoroutineCountDown(int count, string flag = "")
        {
            for (var i = count; i >=0 ; i--)
            {
                LogHelper.LogToTUnityConsole(i, flag, _stopwatch.ElapsedMilliseconds);
                yield return new WaitForSeconds(1);
            }
        }
        
      
        
    }
}
