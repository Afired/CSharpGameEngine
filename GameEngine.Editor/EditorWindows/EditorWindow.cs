using GameEngine.Core.Input;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public abstract class EditorWindow {
    
    protected string Title = "Title";
    private readonly int _id;

    protected EditorWindow() {
        // ReSharper disable once VirtualMemberCallInConstructor
        _id = GetHashCode();
    }
    
    internal void DrawWindow() {
        bool opened = true;
        
        PreDraw();
        
        // push id doesn't work with windows since it can't be handled with the id stack, use ## or ### to set an identifier
        ImGui.Begin(Title + "###" + _id, ref opened, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.MenuBar);
        
        DrawToolbar();
        
        Draw();
//        if(ImGui.IsWindowFocused() && !ImGui.IsWindowAppearing() && Input.IsKeyDown(KeyCode.LeftControl) && Input.IsKeyDown(KeyCode.W)) {
//            opened = false;
//        }
        ImGui.End();
        
        PostDraw();
        
        if(!opened)
            Destroy();
    }
    
    public static void Create<T>() where T : EditorWindow, new() {
        T newWindow = new T();
        EditorGui.Instance.AddWindow(newWindow);
    }
    
    public void Destroy() {
        EditorGui.Instance.RemoveWindow(this);
    }
    
    protected virtual void PreDraw() { }
    protected virtual void PostDraw() { }
    
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
