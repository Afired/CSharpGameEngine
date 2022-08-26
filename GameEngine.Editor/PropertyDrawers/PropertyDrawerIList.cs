//using GameEngine.Core.Nodes;
//using ImGuiNET;
//
//namespace GameEngine.Editor.PropertyDrawers; 
//
//public class PropertyDrawerIList : PropertyDrawer<IMany> {
//    
//    protected override void DrawProperty(ref IMany? value, Property property) {
//
//        if(value is null) {
//            ImGui.Text("null");
//            return;
//        }
//
//        foreach(object valueObject in value.objects) {
//            if(valueObject is string myString) {
//                ImGui.Text(myString);
//            }
//        }
//        
//    }
//    
//}
//
//public class PropertyDrawerList<T> : PropertyDrawer<List<T>> {
//    
//    protected override void DrawProperty(ref List<T>? value, Property property) {
//        throw new NotImplementedException();
//    }
//    
//}
