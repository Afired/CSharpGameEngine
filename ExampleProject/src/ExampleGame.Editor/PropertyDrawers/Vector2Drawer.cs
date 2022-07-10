using ExampleGame.Nodes;
using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

namespace ExampleGame.Editor.PropertyDrawers;

public class MyValueFloatDrawer : PropertyDrawer<MyValueFloat> {
    
    protected override void DrawProperty(ref MyValueFloat value, Property property) {
        ImGui.Text("MyValueFloat");
    }
    
}
