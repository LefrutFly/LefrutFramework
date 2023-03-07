using Lefrut.Extensions;
using UnityEngine;

namespace Lefrut.Framework
{
    public class GlobalSceneData : DIExtension<GlobalSceneData>
    {
        private const int COUNT_PROPERTIES = 10000;


        public Property<MonoProvider>[] Properties { get; private set; } = new Property<MonoProvider>[COUNT_PROPERTIES];

        private int lastIndex = -1;


        public int AddEntityProviders(Property<MonoProvider> property)
        {
            int index = lastIndex;

            if (index < COUNT_PROPERTIES)
            {
                index++;

                Properties[index] = property;

                lastIndex = index;
            }
            else
            {
                Debug.LogError("!GlobalSceneData! Properties is full! Try increasing the COUNT_POPERTIES value.");
            }
            return index;
        }
    }
}