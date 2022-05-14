using System.Reflection;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Serialization;
using GameEngine.Editor.PropertyDrawers;
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
                Console.LogWarning($"Failed to register Node Drawer for {nodeDrawer.NodeType}");
        }
        return nodeDrawerLookup;
    }
    
    protected static void DrawDefaultHeader(Node node) {
//        ImGui.CollapsingHeader(node.GetType().ToString());
        ImGui.Text(node.GetType().ToString());
        ImGui.Spacing();
    }
    
    protected static void DrawDefaultDrawers(Node node) {
        foreach(MemberInfo memberInfo in GetSerializedMembersNotBeingHidden(node.GetType())) {
            if(memberInfo is PropertyInfo propertyInfo) {
                PropertyDrawer.Draw(node, propertyInfo);
            } else if(memberInfo is FieldInfo fieldInfo) {
                PropertyDrawer.Draw(node, fieldInfo);
            }
        }
    }
    
    //todo: exclude if [Serialized(Hidden)]
    private static List<MemberInfo> GetSerializedMembersNotBeingHidden(Type type) {
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
