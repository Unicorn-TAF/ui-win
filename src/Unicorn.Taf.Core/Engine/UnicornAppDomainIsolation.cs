using System;
using System.IO;
using System.Reflection;

namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides with ability to manipulate with Unicorn test assembly in dedicated <see cref="AppDomain"/>
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of worker on tests assembly</typeparam>
    [Serializable]
    public sealed class UnicornAppDomainIsolation<T> : IDisposable where T : MarshalByRefObject
    {
        [NonSerialized]
        private AppDomain _domain;
        private readonly string _assemblyDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnicornAppDomainIsolation{T}"/> class based on specified tests assembly directory.
        /// </summary>
        /// <param name="assemblyDirectory">path to tests assembly directory</param>
        public UnicornAppDomainIsolation(string assemblyDirectory)
        {
            _assemblyDirectory = assemblyDirectory;

            var setup = new AppDomainSetup
            {
                ShadowCopyFiles = "true",
                ApplicationBase = assemblyDirectory
            };

            _domain = AppDomain.CreateDomain("UnicornAppDomain:" + Guid.NewGuid(), null, setup);

            _domain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var type = typeof(T);
            Instance = (T)_domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        /// <summary>
        /// Gets assembly worker instance
        /// </summary>
        public T Instance { get; }

        /// <summary>
        /// Unloads current <see cref="AppDomain"/>
        /// </summary>
        public void Dispose()
        {
            if (_domain != null)
            {
                AppDomain.Unload(_domain);
                _domain = null;
            }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var shortAssemblyName = args.Name.Substring(0, args.Name.IndexOf(','));
            var fileName = Path.Combine(_assemblyDirectory, shortAssemblyName + ".dll");

            if (File.Exists(fileName))
            {
                var bytes = File.ReadAllBytes(fileName);
                var assessment = Assembly.Load(bytes);
                return assessment;
            }
            else
            {
                return Assembly.GetExecutingAssembly().FullName == args.Name ? Assembly.GetExecutingAssembly() : null;
            }
        }
    }
}
