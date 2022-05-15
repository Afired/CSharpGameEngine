using GameEngine.Core.Core;

namespace GameEngine.Core.Layers; 

public class EditorLayer : Layer {
    
    public EditorLayer() {
        SwapBuffers = true;
    }
    
    protected override void OnAttach() {
        GlfwWindow.ImGuiController.Update(Time.DeltaTime);
    }
    
    protected override void OnDetach() {
        GlfwWindow.ImGuiController.Render();
    }
    
}
