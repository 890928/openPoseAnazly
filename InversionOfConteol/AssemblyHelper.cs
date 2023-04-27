using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Webapi.InversionOfControl
{
    public class AssemblyHelper
    {
        private string _assemblyFilter = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyHelper"/> class.
        /// </summary>
        /// <param name="assemblyFilter">filter assembly name</param>
        public AssemblyHelper(string assemblyFilter)
        {
            _assemblyFilter = assemblyFilter;
        }

        /// <summary>
        /// Use assembly name get one assembly which project refer to
        /// </summary>
        /// <param name="assemblyName">Assembly Name</param>
        /// <returns></returns>
        public static Assembly GetAssembly(string assemblyName) {
            var rLibrary = DependencyContext.Default.RuntimeLibraries;
            foreach (var dll in rLibrary)
            {
                if (dll.Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase))
                {
                    var assembly = Assembly.Load(new AssemblyName(dll.Name));
                    return assembly;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Assemblies with fiter
        /// </summary>
        /// <returns></returns>
        public List<Assembly> GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var rLibrary = DependencyContext.Default.RuntimeLibraries;
            foreach (var dll in rLibrary)
            {
                if (dll.Name.IndexOf(_assemblyFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var assembly = Assembly.Load(new AssemblyName(dll.Name));
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }

        /// <summary>
        /// Use assembly name get assemblies which project refer to
        /// </summary>
        /// <param name="assemblyNameList">Assembly Name List</param>
        /// <returns></returns>
        public List<Assembly> GetAssemblies(List<string> assemblyNameList)
        {
            var assemblies = new List<Assembly>();
            foreach (var assemblyName in assemblyNameList)
            {
                var assembly = GetAssembly(assemblyName);
                if (assembly != null)
                {
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }

        /// <summary>
        /// Get IOC Attribute Setting
        /// </summary>
        /// <param name="assemblies">assemblies</param>
        /// <returns></returns>
        public List<(Type, Type, IocType)> GetAttributeSetting(List<Assembly> assemblies)
        {
            var registerList = new List<(Type, Type, IocType)>();

            assemblies.ForEach(assembly =>
            {
                // 取得非公開項目
                // foreach (var classType in assembly.GetExportedTypes())
                foreach (var classType in assembly.GetTypes())
                {
                    var att = classType.GetTypeInfo().GetCustomAttribute<RegisterIOCAttribute>();
                    if (att != null)
                    {
                        var interfaceType = att.TInterface;
                        var implementType = att.TImplementation;

                        if (implementType == null)
                        {
                            implementType = classType;
                        }

                        if (interfaceType == null)
                        {
                            var interfaceArray = classType.GetInterfaces();
                            if (interfaceArray.Count() == 1)
                            {
                                interfaceType = interfaceArray.Single();
                            }
                            else
                            {
                                interfaceType = classType.GetInterfaces()
                                                .FirstOrDefault(i => i.FullName.IndexOf(_assemblyFilter, StringComparison.OrdinalIgnoreCase) >= 0);
                            }

                            if (interfaceType == null)
                            {
                                interfaceType = classType;
                            }
                        }

                        registerList.Add((interfaceType, implementType, att.LifeCycle));
                    }
                }
            });

            return registerList;
        }
    }
}