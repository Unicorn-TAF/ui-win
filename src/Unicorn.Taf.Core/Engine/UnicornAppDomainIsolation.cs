using System;

namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides with ability to manipulate with Unicorn test assembly in dedicated <see cref="AppDomain"/>
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of worker on tests assembly</typeparam>
    public sealed class UnicornAppDomainIsolation<T> : IDisposable where T : MarshalByRefObject
    {
        private readonly T instance;
        private AppDomain domain;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnicornAppDomainIsolation{T}"/> based on specified tests assembly directory.
        /// </summary>
        /// <param name="assemblyDirectory">path to tests assemly directory</param>
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

        /// <summary>
        /// Gets assembly worker instance
        /// </summary>
        public T Instance => this.instance;

        /// <summary>
        /// Unloads current <see cref="AppDomain"/>
        /// </summary>
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
