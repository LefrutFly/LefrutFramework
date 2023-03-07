using Lefrut.Extensions;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Lefrut.Framework
{
    [RequireComponent(typeof(EntityUpdatesAdder))]
    public abstract class Entity : MonoBehaviour, IRun
    {
        [SerializeField] private List<MonoProvider> monoProvidersOnEntity = new List<MonoProvider>();
        [SerializeField] private int index;

        public List<MonoProvider> MonoProvidersOnEntity => monoProvidersOnEntity;
        public Property<MonoProvider> Providers => providers;
        public Property<MonoProvider> NeededProviders => neededProviders;
        public int Index => index;

        private Property<MonoProvider> providers = new Property<MonoProvider>();
        private Property<MonoProvider> neededProviders = new Property<MonoProvider>();

        protected GlobalSystemStorage globalSystemStorage;


        [Button]
        public void AddAllProviders()
        {
            InitData();

            foreach (var item in NeededProviders.GetValuesArray())
            {
                if (providers.Has(item) == true) continue;

                bool isContinue = false;
                foreach (var provider in monoProvidersOnEntity)
                {
                    if (provider.GetType() == item.GetType())
                    {
                        isContinue = true;
                        break;
                    }
                }
                if (isContinue)
                {
                    continue;
                }

                var addedProvider = gameObject.AddComponent(item.GetType()) as MonoProvider;
                addedProvider.Entity = this;

                monoProvidersOnEntity.Add(addedProvider);
            }
        }

        [Button]
        public void RemoveAllProviders()
        {
            foreach (var provider in monoProvidersOnEntity)
            {
                DestroyImmediate(provider);
            }

            monoProvidersOnEntity.Clear();
            neededProviders.ClearAll();
        }


        private void Awake()
        {
            TakeMonoProviders();
            InitGlobalSystemStorage();
            InitSystems();
        }

        private void OnEnable()
        {
            var enableSystems = globalSystemStorage.enableSystems[index];

            foreach (var system in enableSystems)
            {
                system.Enable();
            }
        }

        private void Start()
        {
            var startableSystems = globalSystemStorage.startableSystems[index];

            foreach (var system in startableSystems)
            {
                system.Start();
            }
        }

        public void Run()
        {
            var updatableSystems = globalSystemStorage.updatableSystems[index];

            foreach (var system in updatableSystems)
            {
                system.Update();
            }
        }

        public void FixedRun()
        {
            var fixedUpdatableSystems = globalSystemStorage.fixedUpdatableSystems[index];

            foreach (var system in fixedUpdatableSystems)
            {
                system.FixedUpdate();
            }
        }

        public void LateRun()
        {
            var lateUpdatableSystems = globalSystemStorage.lateUpdatableSystems[index];

            foreach (var system in lateUpdatableSystems)
            {
                system.LateUpdate();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var triggerableSystems = globalSystemStorage.triggerableSystems[index];

            foreach (var system in triggerableSystems)
            {
                system.TriggetEnter(collision);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            var triggerableSystems = globalSystemStorage.triggerableSystems[index];

            foreach (var system in triggerableSystems)
            {
                system.TriggetStay(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var triggerableSystems = globalSystemStorage.triggerableSystems[index];

            foreach (var system in triggerableSystems)
            {
                system.TriggetExit(collision);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collidableSystems = globalSystemStorage.collidableSystems[index];

            foreach (var system in collidableSystems)
            {
                system.CollideEnter(collision);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            var collidableSystems = globalSystemStorage.collidableSystems[index];

            foreach (var system in collidableSystems)
            {
                system.CollideStay(collision);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var collidableSystems = globalSystemStorage.collidableSystems[index];

            foreach (var system in collidableSystems)
            {
                system.CollideExit(collision);
            }
        }

        private void OnDisable()
        {
            var disableSystems = globalSystemStorage.disableSystems[index];

            foreach (var system in disableSystems)
            {
                system.Disable();
            }
        }

        private void OnDestroy()
        {
            var destroySystems = globalSystemStorage.destroySystems[index];

            foreach (var system in destroySystems)
            {
                system.Destroy();
            }
        }

        private void TakeMonoProviders()
        {
            providers.TakeListBack(monoProvidersOnEntity);
        }

        private void InitGlobalSystemStorage()
        {
            globalSystemStorage = GlobalSystemStorage.GetInstance();
            index = globalSystemStorage.GetIndex();
        }

        protected void AddSystem(BaseSystem system)
        {
            if (system == null) return;

            system.Initialize(providers, this);

            
            if (system is IEnableSystem)
            {
                globalSystemStorage.AddEnableSystem(index, system as  IEnableSystem);
            }       
            if (system is IStartableSystem)
            {
                globalSystemStorage.AddStartableSystem(index, system as IStartableSystem);
            }   
            if (system is IUpdatableSystem)
            {
                globalSystemStorage.AddUpdatableSystem(index, system as IUpdatableSystem);
            }
            if (system is IFixedUpdatableSystem)
            {
                globalSystemStorage.AddFixedUpdatableSystem(index, system as IFixedUpdatableSystem);
            }
            if (system is ILateUpdatableSystem)
            {
                globalSystemStorage.AddLateUpdatableSystem(index, system as ILateUpdatableSystem);
            }
            if (system is ITriggerableSystem)
            {
                globalSystemStorage.AddTriggerableSystem(index, system as ITriggerableSystem);
            }
            if (system is ICollidableSystem)
            {
                globalSystemStorage.AddCollidableSystem(index, system as ICollidableSystem);
            }
            if (system is IDisableSystem)
            {
                globalSystemStorage.AddDisableSystem(index, system as IDisableSystem);
            }
            if (system is IDestroySystem)
            {
                globalSystemStorage.AddDestroySystem(index, system as IDestroySystem);
            }
        }

        protected void RemoveSystem(BaseSystem system)
        {
            globalSystemStorage.RemoveSystem(index, system);
        }

        protected void AddData(BaseSystem system)
        {
            system.AddProviders();
            NeededProviders.AddPropertyElements(system.NeededProviders);
        }

        protected abstract void InitSystems();

        protected abstract void InitData();
    }
}
