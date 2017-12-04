using System;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace TidyDesktopMonster.AppHelper
{
    internal sealed class SingleInstanceGuard : IDisposable
    {
        public bool IsPrimaryInstance { get; }

        Mutex _mutex;

        public SingleInstanceGuard(string applicationId, Scope scope = Scope.LocalMachine)
        {
            _mutex = new Mutex(initiallyOwned: true, name: GetMutexName(applicationId, scope));
            IsPrimaryInstance = _mutex.WaitOne(TimeSpan.Zero, exitContext: true);
        }

        static string GetMutexName(string applicationId, Scope scope)
        {
            switch (scope)
            {
                case Scope.CurrentUser:
                    var userName = WindowsIdentity.GetCurrent().Name;
                    return $"{applicationId}:{EncodeId(userName)}";

                case Scope.LocalMachine:
                    return applicationId;

                default:
                    throw new ArgumentException("Unhandled value", "scope");
            }
        }

        static string EncodeId(string id)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(id))
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public void Dispose()
        {
            if (IsPrimaryInstance)
                _mutex.ReleaseMutex();

            _mutex.Dispose();
        }

        public enum Scope
        {
            LocalMachine,
            CurrentUser,
        }
    }
}
