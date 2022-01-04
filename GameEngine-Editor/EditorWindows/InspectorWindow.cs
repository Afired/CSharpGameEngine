using GameEngine.Components;
using GameEngine.Entities;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows;

public class InspectorWindow : EditorWindow {
    
    private Entity v_selected;
    public Entity Selected {
        get => v_selected;
        private set {
            v_selected = value;
            // todo: needs a unique identifier, because if not provided it takes the name
            //Title = $"Inspector - {value.GetType().ToString()}";
        }
    }
    
    
    public InspectorWindow() {
        Title = "Inspector";
        HierarchyWindow.OnSelect += entity => Selected = entity;
    }
    
    protected override void Draw() {
        if(Selected == null) {
            ImGui.Text("select an entity");
            return;
        }
        if(Selected is ITransform transform)
            DrawTransform(transform.Transform);
    }

    private void DrawTransform(Transform transform) {
        ImGui.Text("Transform");
        ImGui.Text($"Position: {transform.Position.X} | {transform.Position.Y} | {transform.Position.Z}");
        ImGui.Text($"Rotation: {transform.Rotation}");
        ImGui.Text($"Scale: {transform.Scale.X} | {transform.Scale.Y} | {transform.Scale.Z}");
    }
    
}
