using System.Reflection;
using GameEngine.Core.Nodes;
using GameEngine.Editor.PropertyDrawers;

namespace GameEngine.Editor.NodeDrawers; 

public class RendererDrawer : NodeDrawer<SpriteRenderer> {
    
    protected override void DrawNode(SpriteRenderer node) {
        MemberInfo[] properties = GetSerializedMembersNotBeingHiddenWithBaseTypesIncluded(node.GetType()).ToArray();//.First(m => m.Name == "Position");
        
        PropertyDrawer.Draw(node, properties.First(p => p.Name == "Texture") as PropertyInfo);
        PropertyDrawer.Draw(node, properties.First(p => p.Name == "Shader") as PropertyInfo);
        PropertyDrawer.Draw(node, properties.First(p => p.Name == "Geometry") as PropertyInfo);
        
    }
    
}
