using System.Numerics;
using System.Reflection;
using GameEngine.Core.Nodes;
using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

namespace GameEngine.Editor.NodeDrawers; 

public class RendererDrawer : NodeDrawer<Renderer> {
    
    protected override void DrawNode(Renderer node) {
        MemberInfo[] properties = GetSerializedMembersNotBeingHidden(node.GetType()).ToArray();//.First(m => m.Name == "Position");
        
        
        if(ImGui.CollapsingHeader("Transform", ImGuiTreeNodeFlags.DefaultOpen)) {
            PropertyDrawer.Draw(node, properties.First(p => p.Name == "LocalPosition") as PropertyInfo);
            PropertyDrawer.Draw(node, properties.First(p => p.Name == "Scale") as PropertyInfo);
            PropertyDrawer.Draw(node, properties.First(p => p.Name == "Rotation") as PropertyInfo);
        }
        ImGui.Spacing();
        if(ImGui.CollapsingHeader("Renderer", ImGuiTreeNodeFlags.DefaultOpen)) {
            PropertyDrawer.Draw(node, properties.First(p => p.Name == "Texture") as PropertyInfo);
            PropertyDrawer.Draw(node, properties.First(p => p.Name == "Shader") as PropertyInfo);
            PropertyDrawer.Draw(node, properties.First(p => p.Name == "Geometry") as PropertyInfo);
        }
        
    }
    
}
