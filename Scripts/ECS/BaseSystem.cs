using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy.ECS
{
    // NOTE THAT SOME COMPONENTS RETURNED MIGHT BE QUEUED FOR POOLING!
    public class BaseSystem
    {
        // todo: instance
        public enum UpdateFormat
        {
            Update1Request1ComponentSet,
            Update1RequestNComponentSets,
            UpdateNRequestsNComponentSets
        }
        public UpdateFormat updateFormat;
        public List<Type[]> typesetList = new List<Type[]>() { new Type[0] };
        public bool isParallel = false;
        public Type[] typeset
        {
            set
            {
                typesetList[0] = value;
            }
            get
            {
                return typesetList[0];
            }
        }
        public IEnumerable<Entity> FindAllEntitiesWithTypes(Type[] types)
        {
            return Database.entities.Where(s => {
                if (s.queueForPooling) return false;
                foreach (var key in s.componentRefs.Keys) 
                    if (Database.components[key][s.componentRefs[key]].queueForPooling && FindAllEntitiesWithTypeAndIndex(key, s.componentRefs[key]).Count() <= 1) 
                        return false;
                var hasAll = true;
                foreach (var t in types)
                {
                    if (!s.componentRefs.ContainsKey(t))
                        hasAll = false;
                }
                return hasAll;
            });
        }
        public IEnumerable<Entity> FindAllEntitiesWithTypeAndIndex(Type type, int index)
        {
            return Database.entities.Where(s =>
            {
                if (!s.componentRefs.ContainsKey(type)) return false;
                return s.componentRefs[type] == index;
            });
        }
        public virtual void Initialize()
        {

        }
        public void UpdateAll()
        {
            switch (updateFormat)
            {
                case UpdateFormat.Update1Request1ComponentSet:
                    Entity[] entities = FindAllEntitiesWithTypes(typeset).ToArray();
                    if (isParallel)
                    {
                        Parallel.For(0, entities.Length, i =>
                        {
                            var entity = entities[i];
                            Dictionary<Type, int> componentSet = new Dictionary<Type, int>();
                            foreach (var type in typeset)
                            {
                                componentSet.Add(type, entity.componentRefs[type]);
                            }
                            Update1ComponentSet(componentSet);
                        });
                    }
                    else
                    {
                        for (int i = 0; i < entities.Length; i++)
                        {
                            var entity = entities[i];
                            Dictionary<Type, int> componentSet = new Dictionary<Type, int>();
                            foreach (var type in typeset)
                                componentSet.Add(type, entity.componentRefs[type]);
                            Update1ComponentSet(componentSet);
                        }
                    }
                    break;
                case UpdateFormat.Update1RequestNComponentSets:
                    UpdateNComponentSets(GetListDictionaryForTypeset(typeset));
                    break;
                case UpdateFormat.UpdateNRequestsNComponentSets:
                    var listOfComponentSets = new List<Dictionary<Type, List<int>>>();
                    foreach (var thisTypeset in typesetList)
                        listOfComponentSets.Add(GetListDictionaryForTypeset(thisTypeset));
                    UpdateNComponentSetsNRequests(listOfComponentSets);
                    break;
            }
        }
        public Dictionary<Type, List<int>> GetListDictionaryForTypeset(Type[] typeset)
        {
            var entities = FindAllEntitiesWithTypes(typeset).ToArray();
            var componentSets = new Dictionary<Type, List<int>>();
            foreach (var type in typeset)
            {
                componentSets.Add(type, new List<int>());
            }
            for (int i = 0; i < entities.Length; i++)
            {
                foreach (var type in typeset)
                {
                    componentSets[type].Add(entities[i].componentRefs[type]);
                }
            }
            return componentSets;
        }
        public virtual void Update1ComponentSet(Dictionary<Type, int> componentSet)
        {

        }

        public virtual void UpdateNComponentSets(Dictionary<Type, List<int>> componentSets)
        {

        }
        public virtual void UpdateNComponentSetsNRequests(List<Dictionary<Type, List<int>>> componentSetsList)
        {
            
        }
        public List<T> GetComponents<T>(int i, List<Dictionary<Type, List<int>>> componentSetsList) where T : Component
        {
            List<int> componentsIndices = componentSetsList[i][typeof(T)];
            List<T> components = new List<T>();
            foreach (var j in componentsIndices)
                components.Add((T)Database.components[typeof(T)][j]);
            return components;
        }
        public void SetComponents<T>(int i, List<Dictionary<Type, List<int>>> componentSetsList, List<T> components) where T : Component
        {
            List<int> componentsIndices = componentSetsList[i][typeof(T)];
            for (int j = 0; j < componentsIndices.Count; j++)
            {
                Database.components[typeof(T)][componentsIndices[j]] = components[j];
            }
        }
        public void SetComponents<T>(int i, List<Dictionary<Type, List<int>>> componentSetsList, List<T> components, List<int> componentsIndices) where T : Component
        {
            for (int j = 0; j < componentsIndices.Count; j++)
            {
                Database.components[typeof(T)][componentsIndices[j]] = components[j];
            }
        }
        public List<int> GetComponentsIndices<T>(int i, List<Dictionary<Type, List<int>>> componentSetsList) where T : Component
        {
            return componentSetsList[i][typeof(T)];
        }
    }
}