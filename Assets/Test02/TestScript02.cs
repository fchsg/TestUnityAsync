using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Test02
{
    public class TestScript02 : MonoBehaviour
    {
        private Stopwatch _stopwatch;

        async void Start()
        {
            _stopwatch = Stopwatch.StartNew();
            
            // main thread execute
            //CPUBigHead(150);
            
            // other thread execute
            await CPUBigHeadAsync(150);
        }

        private void CPUBigHead(int n)
        {
            double result = 1;
            LogHelper.LogToTUnityConsole(result, $"n!(n={n}) start", _stopwatch.ElapsedMilliseconds);
            for (var i = 1; i <= n; i++)
            {
                result *= i;
                Thread.Sleep(10);
            }
            LogHelper.LogToTUnityConsole(result, $"n!(n={n}) stop", _stopwatch.ElapsedMilliseconds);
            Thread.Sleep(1000);
            
            //Texture change logic puts here
            LogHelper.LogToTUnityConsole(result, $"n!(n={n}) output", _stopwatch.ElapsedMilliseconds);
        }


        private async Task CPUBigHeadAsync(int n)
        {
            double result = 1;
            await Task.Run(() => 
            {
                LogHelper.LogToTUnityConsole(result, $"n!(n={n}) start", _stopwatch.ElapsedMilliseconds);
                for (var i = 1; i <= n; i++)
                {
                    result *= i;
                    Thread.Sleep(10);
                }
                LogHelper.LogToTUnityConsole(result, $"n!(n={n}) stop", _stopwatch.ElapsedMilliseconds);
            });  //Scheduler will contiue execution in main thread here 
 
            await Task.Delay(1000);
            //Texture change logic puts here
            LogHelper.LogToTUnityConsole(result, $"n!(n={n}) output", _stopwatch.ElapsedMilliseconds);
        }
    }
}
