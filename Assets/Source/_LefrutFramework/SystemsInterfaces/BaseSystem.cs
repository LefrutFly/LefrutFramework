namespace Lefrut.Framework
{
    public abstract class BaseSystem
    {
        protected Entity Actor;

        public bool IsActive = true;

        public Property<MonoProvider> NeededProviders = new Property<MonoProvider>();

        public Property<MonoProvider> Providers = new Property<MonoProvider>();


        public virtual void Initialize(Property<MonoProvider> providers, Entity actor)
        {
            Providers = providers;
            Actor = actor;
        }

        public abstract void AddProviders();
    }
}