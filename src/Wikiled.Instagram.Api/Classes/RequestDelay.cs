using System;

namespace Wikiled.Instagram.Api.Classes
{
    public class RequestDelay : IRequestDelay
    {
        private readonly int maxSeconds;

        private readonly int minSeconds;

        private readonly Random random;

        private bool isEnabled;

        private RequestDelay(int minSeconds, int maxSeconds)
        {
            this.minSeconds = minSeconds;
            this.maxSeconds = maxSeconds;
            random = new Random(DateTime.Now.Millisecond);
            isEnabled = true;
        }

        public bool Exist => isEnabled && minSeconds != 0 && maxSeconds != 0;

        public TimeSpan Value => Exist ? TimeSpan.FromSeconds(random.Next(minSeconds, maxSeconds)) : TimeSpan.Zero;

        public void Disable()
        {
            isEnabled = false;
        }

        public void Enable()
        {
            isEnabled = true;
        }

        public static IRequestDelay Empty()
        {
            return new RequestDelay(0, 0);
        }

        public static IRequestDelay FromSeconds(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("Value max should be bigger that value min");
            }

            if (max < 0)
            {
                throw new ArgumentException("Both min and max values should be bigger than 0");
            }

            return new RequestDelay(min, max);
        }
    }
}