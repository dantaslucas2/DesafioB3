namespace DesafioB3.Test
{
    public static class WaitHelper
    {
        public static async Task<T> WaitUntilAsync<T>(Func<Task<T>> action, TimeSpan timeout, TimeSpan? interval)
        {
            var deadline = DateTime.UtcNow + timeout;

            while (DateTime.UtcNow < deadline)
            {
                var result = await action();
                if (result is not null)
                {
                    return result;
                }
                await Task.Delay(interval);
            }

            throw new TimeoutException("Timeout waiting for e-mail");
        }
    }
}