namespace GameEngine.Layers; 

public class EditorLayer : Layer {

    public EditorLayer() {
        SwapBuffers = true;
    }

    protected override void OnAttach() {
        GlfwWindow.ImGuiController.Update(0.1f);
    }

    protected override void OnDetach() {
        GlfwWindow.ImGuiController.Render();
    }

}
