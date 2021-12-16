using GameEngine.Components;

namespace GameEngine; 

public class Component {

    public GameObject GameObject;

    public Component(GameObject gameObject) {
        GameObject = gameObject;
    }

}
