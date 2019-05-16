using System;

namespace Wikiled.Instagram.Api.Classes
{
    public interface IRequestDelay
    {
        bool Exist { get; }

        TimeSpan Value { get; }

        void Disable();

        void Enable();
    }
}