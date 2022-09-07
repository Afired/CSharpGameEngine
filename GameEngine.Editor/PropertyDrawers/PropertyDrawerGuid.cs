using System.Runtime.InteropServices;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerGuid : PropertyDrawer<Guid> {
    
    protected override void DrawProperty(ref Guid guid, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        
        ImGui.Text(guid.ToString());
        if(ImGui.BeginDragDropTarget()) {
            ImGuiPayloadPtr payload = ImGui.AcceptDragDropPayload(typeof(Guid).FullName);
            unsafe {
                if(payload.NativePtr is not null) {
                    object? data = Marshal.PtrToStructure(payload.Data, typeof(Guid));
                    if(data is not null) {
                        Guid receivedGuid = (Guid) data;
                        guid = receivedGuid;
                    }
                }
            }
            ImGui.EndDragDropTarget();
        }
        
        ImGui.PopID();
        ImGui.PopItemWidth();
        ImGui.Columns(1);
    }
    
}
