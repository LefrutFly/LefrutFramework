using NaughtyAttributes;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Lefrut.Framework
{
    public abstract class MonoProvider : MonoBehaviour 
    {
        public abstract IData Data { get; }

        [HideInInspector] public Entity Entity;

        [Button] 
        public void Delete()
        {
            Entity?.MonoProvidersOnEntity.Remove(this);
            DestroyImmediate(this);
        }
    }
}