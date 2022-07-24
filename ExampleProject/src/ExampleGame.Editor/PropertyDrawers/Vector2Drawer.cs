using ExampleGame.Nodes;
using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

namespace ExampleGame.Editor.PropertyDrawers;

public class MyValueFloatDrawer : PropertyDrawer<MyValueFloat> {
    
    protected override void DrawProperty(ref MyValueFloat? value, Property property) {

        if(value is null) {
            if(ImGui.Button("Instantiate"))
                value = new MyValueFloat();
            return;
        }
        
        ImGui.Text($"MyValueFloat is {value.Value}");
    }
    
}
