using GameEngine.Core.Rendering;

namespace GameEngine.Core.Layers; 

public delegate void OnDraw(Renderer renderer);

public abstract class Layer {
    
    protected bool SwapBuffers = false;

    internal void Attach(Renderer renderer) {
//        if(SwapBuffers)
//            renderer.SwapActiveFrameBuffer();
        OnAttach(renderer);
    }
    
    public virtual void Draw(Renderer renderer) {
        OnDraw?.Invoke(renderer);
    }

    internal void Detach(Renderer renderer) {
        OnDetach(renderer);
    }

    public event OnDraw? OnDraw;
    protected virtual void OnAttach(Renderer renderer) { }
    protected virtual void OnDetach(Renderer renderer) { }
}
