using GameEngine.Components;

namespace GameEngine; 

public class Component {

    public GameObject GameObject { get; }

    public Component(GameObject gameObject) {
        GameObject = gameObject;
    }

}
