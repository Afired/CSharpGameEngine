using System.Numerics;
using GameEngine.Numerics;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerQuaternion<T> : PropertyDrawer<GameEngine.Numerics.Quaternion<T>> where T : struct, IFloatingPointIeee754<T> {
    
//    protected override void DrawProperty(ref Quaternion quaternion, Property property) {
//        ImGui.Columns(2);
//        ImGui.Text(property.Name);
//        ImGui.NextColumn();
//
//        float x = quaternion.X;
//        float y = quaternion.Y;
//        float z = quaternion.Z;
//        float w = quaternion.W;
//        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 4 - 20);
//        ImGui.PushID(property.Name);
//        ImGui.Text("X");
//        ImGui.SameLine();
//        ImGui.DragFloat($"##X", ref x);
//        ImGui.SameLine();
//        ImGui.Text("Y");
//        ImGui.SameLine();
//        ImGui.DragFloat($"##Y", ref y);
//        ImGui.SameLine();
//        ImGui.Text("Z");
//        ImGui.SameLine();
//        ImGui.DragFloat($"##Z", ref z);
//        ImGui.SameLine();
//        ImGui.Text("W");
//        ImGui.SameLine();
//        ImGui.DragFloat($"##W", ref w);
//        
//        quaternion = new Quaternion(x, y, z, w);
//        
//        ImGui.PopID();
//        ImGui.PopItemWidth();
//        ImGui.Columns(1);
//    }
    
    protected override void DrawProperty(ref GameEngine.Numerics.Quaternion<T> quat, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        float x = float.CreateChecked(quat.X);
        float y = float.CreateChecked(quat.Y);
        float z = float.CreateChecked(quat.Z);
        float w = float.CreateChecked(quat.W);
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X / 4 - 20);
        ImGui.PushID(property.Name);
        ImGui.Text("X");
        ImGui.SameLine();
        ImGui.DragFloat($"##X", ref x);
        ImGui.SameLine();
        ImGui.Text("Y");
        ImGui.SameLine();
        ImGui.DragFloat($"##Y", ref y);
        ImGui.SameLine();
        ImGui.Text("Z");
        ImGui.SameLine();
        ImGui.DragFloat($"##Z", ref z);
        ImGui.SameLine();
        ImGui.Text("W");
        ImGui.SameLine();
        ImGui.DragFloat($"##W", ref w);
        
        quat = new GameEngine.Numerics.Quaternion<T>(T.CreateChecked(x), T.CreateChecked(y), T.CreateChecked(z), T.CreateChecked(w));
        
//        GameEngine.Numerics.Vector3 eulerAngles = quat.ToEulerAngles();
//        float eulerX = (float) ((180 / Math.PI) * eulerAngles.X);
//        float eulerY = (float) ((180 / Math.PI) * eulerAngles.Y);
//        float eulerZ = (float) ((180 / Math.PI) * eulerAngles.Z);
//        
//        System.Numerics.Vector2 size = new(ImGui.GetContentRegionAvail().X / 3 - 20, 15);
//        ImGui.Text("X");
//        ImGui.SameLine();
//        ImGui.Selectable(eulerX.ToString(), false, ImGuiSelectableFlags.None, size);
//        ImGui.SameLine();
//        ImGui.Text("Y");
//        ImGui.SameLine();
//        ImGui.Selectable(eulerY.ToString(), false, ImGuiSelectableFlags.None, size);
//        ImGui.SameLine();
//        ImGui.Text("Z");
//        ImGui.SameLine();
//        ImGui.Selectable(eulerZ.ToString(), false, ImGuiSelectableFlags.None, size);
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
