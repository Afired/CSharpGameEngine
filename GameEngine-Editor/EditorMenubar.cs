using GameEngine.Core;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorMenubar {
    
    public EditorMenubar() {
        OnImGui += Draw;
    }

    private void Draw() {
        if(ImGui.BeginMainMenuBar()) {
            if(ImGui.BeginMenu("Application")) {
                
                if(ImGui.MenuItem("Quit")) {
                    Application.Terminate();
                }
                
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();   
        }
    }
    
}
