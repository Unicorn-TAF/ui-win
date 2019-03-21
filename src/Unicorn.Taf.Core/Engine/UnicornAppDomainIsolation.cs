using System;

namespace Unicorn.Taf.Core.Engine
{
    public sealed class UnicornAppDomainIsolation<T> : IDisposable where T : MarshalByRefObject
    {
        private readonly T instance;
        private AppDomain domain;

        public UnicornAppDomainIsolation(string assemblyDirectory)
        {
            var setup = new AppDomainSetup
            {
                ShadowCopyFiles = "true",
                ApplicationBase = assemblyDirectory
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
