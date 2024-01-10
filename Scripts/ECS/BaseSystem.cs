using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGalaxy.ECS
{
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
                var hasAll = true;
                foreach (var t in types)
                {
                    if (!s.componentRefs.ContainsKey(t))
                        hasAll = false;
                }
                return hasAll;
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
                                componentSet.Add(type, entity.componentRefs[type]);
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
        
}
}