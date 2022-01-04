namespace GameEngine.Layers; 


public delegate void OnDraw();

public abstract class Layer {
    
    public static event OnDraw OnDraw;
    
    public virtual void OnAttach() { }
    public virtual void OnDetach() { }
    public virtual void Draw() {
        OnDraw?.Invoke();
    }
    
}
