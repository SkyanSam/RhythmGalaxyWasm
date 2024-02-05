using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy.ECS
{
    public static class Database
    {
        public static List<Entity> entities = new List<Entity>();
        public static Dictionary<Type, List<Component>> components = new Dictionary<Type, List<Component>>();

        public static void Reset()
        {
            entities = new List<Entity>();
            components = new Dictionary<Type, List<Component>>();
        }
        public static int AddComponent<T>(T component) where T : Component
        {
            if (!components.ContainsKey(typeof(T))) components[typeof(T)] = new List<Component>();
            var list = components[typeof(T)];
            var index = Pooling.Add(component, ref list);
            components[typeof(T)] = list;
            if (typeof(T) == typeof(Bullet)) Console.WriteLine($"Adding bComponent @ {index}");
            return index;
        }
        public static T GetComponent<T>(int index) where T : Component
        {
            return (T)components[typeof(T)][index];
        }
        public static T GetComponent<T>(this Entity e) where T : Component
        {
            return (T)components[typeof(T)][e.componentRefs[typeof(T)]];
        }
        public static void SetComponent<T>(int index, T value) where T : Component
        {
            components[typeof(T)][index] = value;
        }
        public static void SetComponent<T>(this Entity e, T value) where T : Component
        {
            components[typeof(T)][e.componentRefs[typeof(T)]] = value;
        }
        public static void RemoveComponent<T>(this Entity e) where T : Component
        {
            RemoveComponent<T>(e.componentRefs[typeof(T)]);
        }
        public static void RemoveComponent<T>(int index) where T : Component
        {
            /*if (typeof(T) == typeof(Bullet) || typeof(T) == typeof(SpriteComponent) || typeof(T) == typeof(HitboxComponent))
            {
                components[typeof(T)].RemoveAt(index);
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].componentRefs.ContainsKey(typeof(T))) {
                        var entity = entities[i];
                        var eIndex = entity.componentRefs[typeof(T)];
                        if (eIndex > index) entity.componentRefs[typeof(T)] = eIndex - 1;
                        // consider adding case if eIndex == index
                        entities[i] = entity;
                    }
                }
            }*/
            
                var list = components[typeof(T)];
                Pooling.Remove(index, ref list);
                components[typeof(T)] = list;
        }
        public static int AddEntity(Entity entity)
        {
            return Pooling.Add(entity, ref entities);
        }
        public static void RemoveEntity(int index)
        {
            var entity = entities[index];
            Pooling.Remove(index, ref entities);
            foreach (var componentType in entity.componentRefs.Keys)
            {
                if (componentType != null)
                {
                    var list = components[componentType];
                    Pooling.Remove(entity.componentRefs[componentType], ref list);
                    components[componentType] = list;
                }
            }
        }
        public static int FindEntity(Type[] componentTypes, int[] componentIndices)
        {
            return entities.FindIndex((Entity e) =>
            {
                for (int i = 0; i < componentTypes.Length; i++)
                {
                    if (!e.componentRefs.ContainsKey(componentTypes[i]) || e.componentRefs[componentTypes[i]] != componentIndices[i])
                        return false;
                }
                return true;
            });
        }
        /*public static int FindEntity(Entity e)
        {
            return entities.FindIndex(e2 => e == e2)
        }*/

        // CONSIDER MAKING A FUNCTION THAT TAKES COMPONENT AS INPUT AND THEN FINDS ENTITY THAT MATCHES WITH THAT COMPONENT
    }
}