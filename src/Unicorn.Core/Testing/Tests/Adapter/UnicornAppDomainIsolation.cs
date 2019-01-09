using System;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public sealed class UnicornAppDomainIsolation<T> : IDisposable where T : MarshalByRefObject
    {
        private AppDomain domain;
        private readonly T instance;

        public UnicornAppDomainIsolation(string assemblyDirectory)
        {
            var setup = new AppDomainSetup
            {
                ShadowCopyFiles = "true",
            };

            domain = AppDomain.CreateDomain("UnicornAppDomain:" + Guid.NewGuid(), null, setup);

            var type = typeof(T);
            instance = (T)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T Instance => this.instance;

        public void Dispose()
        {
            if (domain != null)
            {
                AppDomain.Unload(domain);
                domain = null;
            }
        }
    }
}
