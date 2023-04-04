using GameEngine.Core.Rendering;
using GameEngine.Numerics;

namespace GameEngine.Core.Nodes; 

public interface ICamera {
    
    public Color BackgroundColor { get; }
    public abstract Matrix<float> ViewMatrix { get; }
    public abstract Matrix<float> ProjectionMatrix { get; }
    public abstract FrameBuffer FrameBuffer { get; }
    
}
