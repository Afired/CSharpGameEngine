using System.Runtime.InteropServices;
using GameEngine.Core.AssetManagement;
using ImGuiNET;

namespace GameEngine.Editor.PropertyDrawers; 

public class PropertyDrawerAsset<T> : PropertyDrawer<AssetRef<T>> where T : class, IAsset {
    
    protected override void DrawProperty(ref AssetRef<T> assetRefRef, Property property) {
        ImGui.Columns(2);
        ImGui.Text(property.Name);
        ImGui.NextColumn();
        
        ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
        ImGui.PushID(property.Name);
        
        ImGui.Text(assetRefRef.Guid.ToString());
        if(ImGui.BeginDragDropTarget()) {
            ImGuiPayloadPtr payload = ImGui.AcceptDragDropPayload(typeof(Guid).FullName);
            unsafe {
                if(payload.NativePtr is not null) {
                    object? data = Marshal.PtrToStructure(payload.Data, typeof(Guid));
                    if(data is not null) {
                        Guid receivedGuid = (Guid) data;
                        assetRefRef = new AssetRef<T>(receivedGuid);
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
