namespace GameEngine.Components;

public class Component {
    
    public GameObject GameObject { get; }
    
    
    public Component(GameObject gameObject) {
        GameObject = gameObject;
    }
    
}
