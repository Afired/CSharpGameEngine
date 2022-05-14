using System.Reflection;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Serialization;
using ImGuiNET;

namespace GameEngine.Editor.NodeDrawers;

internal interface INodeDrawer {
    public Type NodeType { get; }
    public void DrawNodeInternal(object node);
}

public abstract class NodeDrawer<T> : INodeDrawer where T : Node {
    
    public Type NodeType => typeof(T);

    public void DrawNodeInternal(object node) => DrawNode(node as T);
    private static Dictionary<Type, INodeDrawer>? _nodeDrawerLookup;
    
    internal static void Draw(Node node) {
        _nodeDrawerLookup ??= BuildNodeDrawerLookup();

        if(_nodeDrawerLookup.TryGetValue(node.GetType(), out INodeDrawer? nodeDrawer)) {
            nodeDrawer.DrawNodeInternal(node);
        } else {
            DrawDefault(node);
        }
    }
    
    private static Dictionary<Type, INodeDrawer> BuildNodeDrawerLookup() {
        Dictionary<Type, INodeDrawer> nodeDrawerLookup = new();
        List<Type> derivedTypes = ReflectionHelper.GetDerivedTypes(typeof(NodeDrawer<>), typeof(NodeDrawer<>).Assembly);
        foreach(Type type in derivedTypes) { 
      //  foreach(Type type in typeof(NodeDrawer<>).Assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(NodeDrawer<>)))) {
            //object? instance = Activator.CreateInstance(type);
            INodeDrawer nodeDrawer = Activator.CreateInstance(type) as INodeDrawer ?? throw new NullReferenceException();
            if(!nodeDrawerLookup.TryAdd(nodeDrawer.NodeType, nodeDrawer))
                Console.LogWarning($"Failed to register Node Drawer of type {nodeDrawer.NodeType}");
        }
        return nodeDrawerLookup;
    }
    
    public abstract void DrawNode(T node);
    
    protected static void DrawDefault(Node node) {
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
