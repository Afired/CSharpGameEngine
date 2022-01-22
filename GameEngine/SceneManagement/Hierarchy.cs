using System;
using System.Collections;
using System.Collections.Generic;
using GameEngine.Entities;

namespace GameEngine.SceneManagement; 

public class Hierarchy : IEnumerable {

    private static Hierarchy v_instance;
    public static Hierarchy Instance => v_instance ??= new Hierarchy();

    private List<Entity> _entities;
    
    
    public Hierarchy() {
        _entities = new List<Entity>();
    }

    public void Add(Entity entity) {
        _entities.Add(entity);
        entity.Awake();
    }
    
    public IEnumerator<Entity> GetEnumerator() {
        foreach(Entity entity in _entities)
            yield return entity;
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public int Count => _entities.Count;
    
    public Entity this[int index] {
        get {
            if(index > _entities.Count - 2)
                throw new ArgumentOutOfRangeException();
            return _entities[index];
        }
    }
    
}
