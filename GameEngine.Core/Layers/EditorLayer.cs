using System.Numerics;
using GameEngine.Core.Core;
using ImGuiNET;

namespace GameEngine.Core.Layers; 

public class EditorLayer : Layer {
    
    public EditorLayer() {
        SwapBuffers = true;
    }
    
    protected override void OnAttach() {
        GlfwWindow.ImGuiController.Update(Time.DeltaTime);
        SetTheme();
        PushStyle();
    }

    private static void PushStyle() {
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 4);
    }

    private static void SetTheme() {
        RangeAccessor<Vector4> colors = ImGui.GetStyle().Colors;
        
        // Header
        colors[(int)ImGuiCol.Header] = new Vector4(0.21f, 0.21f, 0.21f, 1.0f);
        colors[(int)ImGuiCol.HeaderHovered] = new Vector4(0.23f, 0.23f, 0.23f, 1.0f);
        colors[(int)ImGuiCol.HeaderActive] = new Vector4(0.25f, 0.25f, 0.25f, 1.0f);
        
        // Button
        colors[(int)ImGuiCol.Button] = new Vector4(0.29f, 0.29f, 0.29f, 1.0f);
//        colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
//        colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        
        // Property Field BG
        colors[(int)ImGuiCol.FrameBg] = new Vector4(0.12f, 0.12f, 0.12f, 1.0f);
        colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.10f, 0.10f, 0.10f, 1.0f);
        colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.22f, 0.22f, 0.22f, 1.2f);
        
        // Tabs
        colors[(int)ImGuiCol.TabUnfocused] = new Vector4(0.18f, 0.18f, 0.18f, 1.0f);            // tabs which are in background
        colors[(int)ImGuiCol.Tab] = new Vector4(0.18f, 0.18f, 0.18f, 1.0f);
        colors[(int)ImGuiCol.TabActive] = new Vector4(0.30f, 0.30f, 0.30f, 1.0f);                  // active tab (always one)
        colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.25f, 0.25f, 0.25f, 1.0f);         // "default"
        colors[(int)ImGuiCol.TabHovered] = new Vector4(0.30f, 0.30f, 0.30f, 1.0f);                  // hover
        
        // Tab Title
        colors[(int)ImGuiCol.TitleBg] = new Vector4(0.14f, 0.14f, 0.14f, 1.0f);
        colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.14f, 0.14f, 0.14f, 1.0f);
        colors[(int)ImGuiCol.TitleBgCollapsed] = new Vector4(0.14f, 0.14f, 0.14f, 1.0f);
        
        // Separator between panels, columns etc.
        colors[(int)ImGuiCol.Separator] = new Vector4(0.10f, 0.10f, 0.10f, 1.0f);
        colors[(int)ImGuiCol.SeparatorHovered] = new Vector4(0.10f, 0.10f, 0.10f, 1.0f);
        colors[(int)ImGuiCol.SeparatorActive] = new Vector4(0.10f, 0.10f, 0.10f, 1.0f);
        
        // Border around main menu bar
        colors[(int)ImGuiCol.Border] = new Vector4(0.10f, 0.10f, 0.10f, 1.0f);
        
        colors[(int)ImGuiCol.Text] = new Vector4(0.80f, 0.80f, 0.80f, 1.0f);
        
        // docking preview highlights
        colors[(int)ImGuiCol.DockingPreview] = new Vector4(0.75f, 0.75f, 0.75f, 0.2f);
        // default docking bg
        colors[(int)ImGuiCol.DockingEmptyBg] = new Vector4(0.12f, 0.12f, 0.12f, 1.0f);
        
        
        colors[(int)ImGuiCol.MenuBarBg] = new Vector4(0.25f, 0.25f, 0.25f, 1.0f);
        
        colors[(int)ImGuiCol.WindowBg] = new Vector4(0.16f, 0.16f, 0.16f, 1.0f);
    }

    protected override void OnDetach() {
        GlfwWindow.ImGuiController.Render();
        ImGui.PopStyleVar();
    }
    
}
