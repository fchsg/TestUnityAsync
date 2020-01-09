```c

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public static class CoroutineTaskHelper
    {
        public static async Task StartCoroutineAsync(this MonoBehaviour monoBehavior, IEnumerator coroutine)
        {
            var tcs = new TaskCompletionSource<object>();
            monoBehavior.StartCoroutine(emptyCoroutine(coroutine, tcs));
            await tcs.Task;
        }

        public static async Task StartCoroutineAsync(this MonoBehaviour monoBehavior, YieldInstruction yieldInstruction)
        {
            var tcs = new TaskCompletionSource<object>();
            monoBehavior.StartCoroutine(emptyCoroutine(yieldInstruction, tcs));
            await tcs.Task;
        }

        public static CoroutineWithTask<object> AsCoroutine(this Task task)
        {
            //var coroutine = new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);
            return new CoroutineWithTask<object>(task);
        }

        public static CoroutineWithTask<T> AsCoroutine<T>(this Task<T> task)
        {
            return new CoroutineWithTask<T>(task);
        }

        private static IEnumerator emptyCoroutine(YieldInstruction coro, TaskCompletionSource<object> completion)
        {
            yield return coro;
            completion.TrySetResult(null);
        }

        private static IEnumerator emptyCoroutine(IEnumerator coro, TaskCompletionSource<object> completion)
        {
            yield return coro;
            completion.TrySetResult(null);
        }

        public struct CoroutineWithTask<T> : IEnumerator
        {
            private readonly IEnumerator _coreCoroutine;

            public CoroutineWithTask(Task<T> coreTask)
            {
                WrappedTask = coreTask;
                _coreCoroutine = new WaitUntil(() => coreTask.IsCompleted || coreTask.IsFaulted || coreTask.IsCanceled);
            }

            public CoroutineWithTask(Task coreTask)
            {
                WrappedTask = Task.Run(async () =>
                {
                    await coreTask;
                    return default(T);
                });
                _coreCoroutine = new WaitUntil(() => coreTask.IsCompleted || coreTask.IsFaulted || coreTask.IsCanceled);
            }

            private Task<T> WrappedTask { get; set; }
            
            public T Result => WrappedTask.Result;

            public object Current => _coreCoroutine.Current;

            public bool MoveNext()
            {
                return _coreCoroutine.MoveNext();
            }

            public void Reset()
            {
                _coreCoroutine.Reset();
            }
        }
    }
}


```










[msdn async]https://docs.microsoft.com/zh-cn/archive/blogs/appconsult/unity-coroutine-tap-zh-cn
