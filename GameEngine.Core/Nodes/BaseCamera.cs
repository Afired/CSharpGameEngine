using System;
using GameEngine.Core.Rendering;
using GameEngine.Core.Serialization;
using GameEngine.Numerics;

namespace GameEngine.Core.Nodes; 

//TODO: proper support for abstract classes: require component attribute without the generation of the component interface but partial extension class
public abstract partial class BaseCamera : Transform3D, ICamera {
    
    [Serialized] public bool IsMainCamera = true;
    [Serialized] public Color BackgroundColor { get; set; } = Application.Instance.Config.DefaultBackgroundColor;
    public FrameBuffer FrameBuffer { get; private set; }
    
    public abstract Matrix<float> ViewMatrix { get; }
    public abstract Matrix<float> ProjectionMatrix { get; }

//    public BaseCamera() {
//        Application.Instance.InGameFrameBuffers.Add(new WeakReference<FrameBuffer>(FrameBuffer));
//    }
    
    protected override void OnAwake() {
//        FrameBuffer = new FrameBuffer(
//            Application.Instance.Renderer.MainWindow.Gl,
//            Application.Instance.Renderer,
//            (uint) Application.Instance.InGameResolution.X,
//            (uint) Application.Instance.InGameResolution.Y,
//            false
//        );
        base.OnAwake();
        Application.Instance.Renderer.SetActiveCamera(this);
    }
    
    protected override void OnUpdate() {
        base.OnUpdate();
        if(IsMainCamera)
            Application.Instance.Renderer.SetActiveCamera(this);
    }
    
    ~BaseCamera() {
//        FrameBuffer.Dispose();
    }
    
}
