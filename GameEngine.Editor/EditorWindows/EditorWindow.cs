using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorWindow {

    protected string Title = "Title";
    private readonly int _id;
    
    public EditorWindow() {
        EditorApplication.Instance.EditorLayer.OnDraw += DrawWindow;
        _id = GetHashCode();
    }
    
    private void DrawWindow() {
        bool opened = true;
        // push id doesnt work with windows since it cant be handled with the id stack, c++ uses ## or ### to set an identifier
        ImGui.Begin(Title + "##" + _id, ref opened, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.MenuBar);

        DrawToolbar();
        
        Draw();
        ImGui.End();
        if(!opened)
            EditorApplication.Instance.EditorLayer.OnDraw -= DrawWindow;
    }
    
    private void DrawToolbar() {
//        ImGui.PushID(Title + "Menubar");
//        if(ImGui.BeginMenuBar()) {
//            if(ImGui.BeginMenu("MyMenu")) {
//                if(ImGui.MenuItem("MyItem")) {
//                    
//                }
//                ImGui.EndMenu();
//            }
//            ImGui.EndMenuBar();
//        }
//        ImGui.PopID();
        
        /*
         * if (ImGui::Begin("StatusBar", nullptr, flags)) {
  if (ImGui::BeginMenuBar()) {
    ImGui::Text("%s", state.c_str());
    ImGui::EndMenuBar();
  }
  ImGui::End();
}
         */
    }
    
    protected virtual void Draw() { }
    
}
