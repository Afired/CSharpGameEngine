namespace GameEngine.Core.Layers; 

public class EditorLayer : Layer {

    public EditorLayer() {
        SwapBuffers = true;
    }

    protected override void OnAttach() {
        GlfwWindow.ImGuiController.Update(0.001f);
    }

    protected override void OnDetach() {
        GlfwWindow.ImGuiController.Render();
    }

}
