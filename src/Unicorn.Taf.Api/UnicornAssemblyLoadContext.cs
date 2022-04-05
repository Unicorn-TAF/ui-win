#if NETCOREAPP || NET
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Unicorn.Taf.Api
{
    /// <summary>
    /// Provides with ability to manipulate with Unicorn test assembly in dedicated <see cref="AssemblyLoadContext"/>
    /// </summary>
    public class UnicornAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly string _assemblyDirectory;
        private readonly List<Assembly> _loadedAssemblies;
        private readonly Dictionary<string, Assembly> _sharedAssemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnicornAssemblyLoadContext"/> class 
        /// based on specified tests assembly directory and types shared across load contexts.
        /// </summary>
        /// <param name="contextDirectory">tests assembly directory</param>
        public UnicornAssemblyLoadContext(string contextDirectory)
        {
            _assemblyDirectory = contextDirectory;
            _loadedAssemblies = new List<Assembly>();
            _sharedAssemblies = new Dictionary<string, Assembly>();
        }

        /// <summary>
        /// Loads all assemblies from tests assembly directory except assemblies containing shared types.
        /// </summary>
        /// <param name="sharedTypes">types shared across load contexts (usually types from Taf.Api)</param>
        public UnicornAssemblyLoadContext Initialize(params Type[] sharedTypes)
        {
            foreach (Type sharedType in sharedTypes)
            {
                _sharedAssemblies[Path.GetFileName(sharedType.Assembly.Location)] = sharedType.Assembly;
            }

            foreach (string dll in Directory.EnumerateFiles(_assemblyDirectory, "*.dll"))
            {
                if (!_sharedAssemblies.ContainsKey(Path.GetFileName(dll)))
                {
                    _loadedAssemblies.Add(LoadFromAssemblyPath(dll));
                }
            }

            return this;
        }

        /// <summary>
        /// Gets all implementations of the specified shared type or interface 
        /// located in all assemblies loaded into context.
        /// </summary>
        /// <typeparam name="T">base shared type</typeparam>
        /// <returns>all shared type implementations</returns>
        public IEnumerable<T> GetImplementations<T>() =>
            _loadedAssemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(T).IsAssignableFrom(t))
            .Select(t => Activator.CreateInstance(t))
            .Cast<T>();

        /// <summary>
        /// Gets <see cref="Assembly"/> containing specified type from the load context.
        /// </summary>
        /// <param name="type">type belonging to desired assembly</param>
        /// <returns><see cref="Assembly"/> containing the type</returns>
        public Assembly GetAssemblyContainingType(Type type) =>
            _loadedAssemblies
            .First(a => a.GetName().Name.Equals(type.Assembly.GetName().Name, StringComparison.InvariantCulture));

        /// <summary>
        /// Gets <see cref="Assembly"/> by its <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyName">assembly name</param>
        /// <returns><see cref="Assembly"/> located at path</returns>
        public Assembly GetAssembly(AssemblyName assemblyName) =>
            _loadedAssemblies
            .First(a => a.GetName().FullName.Equals(assemblyName.FullName, StringComparison.InvariantCulture));

        /// <summary>
        /// Loads an assembly from specified path. 
        /// </summary>
        /// <param name="assemblyPath">path of assembly to load</param>
        public void LoadAssemblyFrom(string assemblyPath) =>
            _loadedAssemblies.Add(LoadFromAssemblyPath(assemblyPath));

        /// <summary>
        /// Loads an assembly with specified <see cref="AssemblyName"/>. 
        /// If assembly is in shared assemblies it's returned.
        /// </summary>
        /// <param name="assemblyName">assembly name</param>
        /// <returns><see cref="Assembly"/> instance</returns>
        protected override Assembly Load(AssemblyName assemblyName)
        {
            string fileName = $"{assemblyName.Name}.dll";

            if (_sharedAssemblies.ContainsKey(fileName))
            {
                return _sharedAssemblies[fileName];
            }

            return Assembly.Load(assemblyName);
        }
    }
}
#endif