using GameEngine.Core.Rendering;

namespace GameEngine.Core.Layers; 

public delegate void OnDraw();

public abstract class Layer {
    
    protected bool SwapBuffers = false;

    internal void Attach() {
        if(SwapBuffers)
            Renderer.SwapActiveFrameBuffer();
        OnAttach();
    }
    
    public virtual void Draw() {
        OnDraw?.Invoke();
    }

    internal void Detach() {
        OnDetach();
    }

    public event OnDraw OnDraw;
    protected virtual void OnAttach() { }
    protected virtual void OnDetach() { }
}
