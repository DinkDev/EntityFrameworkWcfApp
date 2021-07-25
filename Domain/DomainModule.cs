namespace Domain
{
    using System.Reflection;
    using Autofac;
    using Module = Autofac.Module;

    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var thisAssembly = Assembly.GetAssembly(GetType());

            builder.RegisterAssemblyTypes(thisAssembly)
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}
