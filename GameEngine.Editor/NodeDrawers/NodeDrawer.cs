using System.Reflection;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Serialization;
using ImGuiNET;

namespace GameEngine.Editor.NodeDrawers;

public abstract class NodeDrawer {
    
    private static Dictionary<Type, NodeDrawer>? _nodeDrawerLookup;
    protected internal abstract Type NodeType { get; }
    protected internal abstract void DrawNodeInternal(Node node);
    
    internal static void Draw(Node node) {
        _nodeDrawerLookup ??= BuildNodeDrawerLookup();
        
        if(_nodeDrawerLookup.TryGetValue(node.GetType(), out NodeDrawer? nodeDrawer)) {
            nodeDrawer.DrawNodeInternal(node);
        } else {
            DrawDefaultHeader(node);
            DrawDefaultDrawers(node);
        }
    }
    
    private static Dictionary<Type, NodeDrawer> BuildNodeDrawerLookup() {
        Dictionary<Type, NodeDrawer> nodeDrawerLookup = new();
        List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(NodeDrawer<>), typeof(NodeDrawer<>).Assembly);
        foreach(Type type in derivedTypes) {
            NodeDrawer nodeDrawer = Activator.CreateInstance(type) as NodeDrawer ?? throw new NullReferenceException();
            if(!nodeDrawerLookup.TryAdd(nodeDrawer.NodeType, nodeDrawer))
                Console.LogWarning($"Failed to register Node Drawer of type {nodeDrawer.NodeType}");
        }
        return nodeDrawerLookup;
    }
    
    protected static void DrawDefaultHeader(Node node) {
//        ImGui.CollapsingHeader(node.GetType().ToString());
        ImGui.Text(node.GetType().ToString());
        ImGui.Spacing();
    }
    
    protected static void DrawDefaultDrawers(Node node) {
        foreach(MemberInfo memberInfo in GetSerializedMembers(node.GetType())) {
            if(memberInfo is PropertyInfo propertyInfo) {
                DrawProperty(node, propertyInfo);
            } else if(memberInfo is FieldInfo fieldInfo) {
                DrawField(node, fieldInfo);
            }
        }
    }
    
    private static void DrawField(Node node, FieldInfo fieldInfo) {
        if(fieldInfo.FieldType == typeof(float)) {
            float value = (float) fieldInfo.GetValue(node);
            ImGui.DragFloat(fieldInfo.Name, ref value, 0.01f, float.MinValue, float.MaxValue, "%g");
            fieldInfo.SetValue(node, value);
            return;
        }
        if(fieldInfo.FieldType == typeof(string)) {
            string value = (string) fieldInfo.GetValue(node);
            ImGui.InputText(fieldInfo.Name, ref value, 30);
            fieldInfo.SetValue(node, value);
            return;
        }
        if(fieldInfo.FieldType == typeof(Color)) {
            Color value = (Color) fieldInfo.GetValue(node);
            System.Numerics.Vector4 v4 = new System.Numerics.Vector4(value.R, value.G, value.B, value.A);
            ImGui.ColorEdit4(fieldInfo.Name, ref v4);
            value = new Color(v4.X, v4.Y, v4.Z, v4.W);
            fieldInfo.SetValue(node, value);
            return;
        }
        if(fieldInfo.FieldType == typeof(Vector3)) {
            Vector3 value = (Vector3) fieldInfo.GetValue(node);
            System.Numerics.Vector3 v3 = new System.Numerics.Vector3(value.X, value.Y, value.Z);
            ImGui.DragFloat3(fieldInfo.Name, ref v3, 0.01f);
            value = new Vector3(v3.X, v3.Y, v3.Z);
            fieldInfo.SetValue(node, value);
            return;
        }
    }
    
    private static void DrawProperty(Node node, PropertyInfo propertyInfo) {
        if(!propertyInfo.CanRead)
            return;
        
        bool isReadonly = !propertyInfo.CanWrite;
        MethodInfo? getter = propertyInfo.GetGetMethod(true);
        MethodInfo? setter = propertyInfo.GetSetMethod(true);
            
        if(getter.ReturnType == typeof(float)) {
            float value = (float) propertyInfo.GetValue(node);
            ImGui.DragFloat(propertyInfo.Name, ref value, 0.01f, float.MinValue, float.MaxValue, "%g", isReadonly ? ImGuiSliderFlags.NoInput : ImGuiSliderFlags.None);
            if(!isReadonly)
                propertyInfo.SetValue(node, value);
            return;
        }
        if(getter.ReturnType == typeof(string)) {
            string value = (string) propertyInfo.GetValue(node);
            ImGui.InputText(propertyInfo.Name, ref value, 30);
            if(!isReadonly)
                propertyInfo.SetValue(node, value);
            return;
        }
        if(getter.ReturnType == typeof(Color)) {
            Color value = (Color) propertyInfo.GetValue(node);
            System.Numerics.Vector4 v4 = new System.Numerics.Vector4(value.R, value.G, value.B, value.A);
            ImGui.ColorEdit4(propertyInfo.Name, ref v4);

            if(!isReadonly) {
                value = new Color(v4.X, v4.Y, v4.Z, v4.W);
                propertyInfo.SetValue(node, value);
            }
            return;
        }
        if(getter.ReturnType == typeof(Vector3)) {
            Vector3 value = (Vector3) propertyInfo.GetValue(node);
            System.Numerics.Vector3 v3 = new System.Numerics.Vector3(value.X, value.Y, value.Z);
            ImGui.DragFloat3(propertyInfo.Name, ref v3, 0.01f);

            if(!isReadonly) {
                value = new Vector3(v3.X, v3.Y, v3.Z);
                propertyInfo.SetValue(node, value);
            }
            return;
        }
    }
    
    private static List<MemberInfo> GetSerializedMembers(Type type) {
        List<MemberInfo> members = new List<MemberInfo>();
        for(Type? currentType = type; currentType is not null; currentType = currentType.BaseType) {
            members.AddRange(
                currentType
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(prop => prop.GetCustomAttribute<Serialized>(false) is not null)
            );
            members.AddRange(
                currentType
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(prop => prop.GetCustomAttribute<Serialized>(false) is not null)
            );
        }
        return members;
    }
}

public abstract class NodeDrawer<TNode> : NodeDrawer where TNode : Node {
    
    protected internal sealed override Type NodeType => typeof(TNode);
    protected internal sealed override void DrawNodeInternal(Node node) => DrawNode(node as TNode);
    
    protected abstract void DrawNode(TNode node);
    
}
