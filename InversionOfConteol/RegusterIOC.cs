using System;

namespace Webapi.InversionOfControl
{
    public sealed class RegusterIOC
    {
        /// <summary>
        /// Provide Inject Entry
        /// </summary>
        /// <param name="registerAction">Inject Mapping Function</param>
        /// <param name="assemblyFilter">Specify the range for DI</param>
        public void DependencyInjection(Action<Type, Type, IocType> registerAction, string assemblyFilter = "")
        {
            var helper = new AssemblyHelper(assemblyFilter);
            var settingList = helper.GetAttributeSetting(helper.GetAssemblies());
            settingList.ForEach(r => registerAction(r.Item1, r.Item2, r.Item3));
        }
    }
}