using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy.ECS
{
    public static class Database
    {
        public static List<Entity> entities = new List<Entity>();
        public static Dictionary<Type, List<Component>> components = new Dictionary<Type, List<Component>>();
        public static int AddComponent(Type type, Component component)
        {
            if (!components.ContainsKey(type)) components[type] = new List<Component>();
            var list = components[type];
            return Pooling.Add(component, ref list);
        }
        public static void RemoveComponent(Type type, int index)
        {
            var list = components[type];
            Pooling.Remove(index, ref list);
        }
        public static int AddEntity(Entity entity)
        {
            return Pooling.Add(entity, ref entities);
        }
        public static void RemoveEntity(int index)
        {
            Pooling.Remove(index, ref entities);
            foreach (var componentType in entities[index].componentRefs.Keys)
            {
                if (componentType != null)
                {
                    var list = components[componentType];
                    Pooling.Remove(entities[index].componentRefs[componentType], ref list);
                }
            }
        }
        /*public static int FindEntity(Entity e)
        {
            return entities.FindIndex(e2 => e == e2)
        }*/
    }
}