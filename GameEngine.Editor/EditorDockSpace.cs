using System.Numerics;
using ImGuiNET;

namespace GameEngine.Editor; 

internal class EditorDockSpace {
    
    private uint _id;
    
    internal unsafe void Draw() {
        
        ImGuiViewport* viewport = ImGui.GetMainViewport();
        ImGui.SetNextWindowPos(viewport->Pos + new Vector2(0, 29));
        ImGui.SetNextWindowSize(viewport->Size - new Vector2(0, 29));
        ImGui.SetNextWindowViewport(viewport->ID);
        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoDocking
                                       | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove
                                       | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        ImGui.Begin("##DockSpace", windowFlags);
        ImGui.PopStyleVar(3);
        
        _id = ImGui.GetID("MainDockSpace");
        ImGui.DockSpace(_id, Vector2.Zero, ImGuiDockNodeFlags.PassthruCentralNode);
        
        ImGui.End();
    }
    
}
