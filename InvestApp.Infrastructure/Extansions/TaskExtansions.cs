using System;
using System.Threading.Tasks;

namespace InvestApp.Infrastructure.Extansions
{
    public static class TaskExtansions
    {
        public static async void Await(this Task task, Action complitedCallback = null, Action<Exception> errorCallback = null)
        {
            try
            {
                await task;
                complitedCallback?.Invoke();
            }
            catch (Exception e)
            {
                errorCallback?.Invoke(e);
            }
        }

        public static async Task Await<T>(this Task<T> task, Action<T> complitedCallback = null, Action<Exception> errorCallback = null)
        {
            try
            {
                T result = await task;
                complitedCallback?.Invoke(result);
            }
            catch (Exception e)
            {
                errorCallback?.Invoke(e);
            }
        }
    }
}