using GameEngine.Components;
using GameEngine.Entities;
using GameEngine.Numerics;
using ImGuiNET;
using Vector2 = System.Numerics.Vector2;

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
        ImGui.Indent();
        {
            System.Numerics.Vector3 transformPosition = new System.Numerics.Vector3(transform.Position.X, transform.Position.Y, transform.Position.Z);
            ImGui.InputFloat3("Position", ref transformPosition);
            transform.Position = new Vector3(transformPosition.X, transformPosition.Y, transformPosition.Z);
            
            float transformRotation = transform.Rotation;
            ImGui.InputFloat("Rotation", ref transformRotation);
            transform.Rotation = transformRotation;
            
            System.Numerics.Vector3 transformScale = new System.Numerics.Vector3(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
            ImGui.InputFloat3("Scale", ref transformScale);
            transform.Scale = new Vector3(transformScale.X, transformScale.Y, transformScale.Z);
        }
        ImGui.Unindent();
    }
    
}
