using System.Reflection;
using GameEngine.Core;
using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;
using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;
using JetBrains.Annotations;

namespace GameEngine.Editor.NodeDrawers;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class NodeDrawer {
    
    private static Dictionary<Type, NodeDrawer>? _nodeDrawerLookup;
    protected internal abstract Type NodeType { get; }
    protected internal abstract void DrawNodeInternal(Node node);
    protected internal abstract bool DrawHeaderInternal(Node node);
    
    internal static void Draw(Node node) {
        _nodeDrawerLookup ??= BuildNodeDrawerLookup();

        foreach(Type currentType in EnumerateFromBaseTypeUp(node.GetType())) {

            List<MemberInfo> memberInfos = GetSerializedMembersDisplayedInInspector(currentType);
            
            if(memberInfos.Count == 0)
                continue;
            
            if(_nodeDrawerLookup.TryGetValue(currentType, out NodeDrawer? nodeDrawer)) {
                if(nodeDrawer.DrawHeaderInternal(node)) {
                    nodeDrawer.DrawNodeInternal(node);
                }
            } else {
                if(DrawDefaultHeader(currentType)) {
                    DrawDefaultDrawers(node, currentType);
                }
            }
            
//            if(ImGui.CollapsingHeader(currentType.Name, ImGuiTreeNodeFlags.DefaultOpen)) {
//                if(_nodeDrawerLookup.TryGetValue(currentType, out NodeDrawer? nodeDrawer))
//                    nodeDrawer.DrawNodeInternal(node);
//                else
//                    DrawDefaultDrawers(node, currentType);
//            }
            
        }
        
//        if(_nodeDrawerLookup.TryGetValue(node.GetType(), out NodeDrawer? nodeDrawer)) {
//            nodeDrawer.DrawNodeInternal(node);
//        } else {
//            DrawDefaultHeader(node);
//            DrawDefaultDrawers(node);
//        }
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
    
    protected static bool DrawDefaultHeader(Type type) {
        return ImGui.CollapsingHeader(type.Name, ImGuiTreeNodeFlags.DefaultOpen);
    }
    
    protected static void DrawDefaultDrawers(Node node, Type type) {
        foreach(MemberInfo memberInfo in GetSerializedMembersDisplayedInInspector(type)) {
            if(memberInfo is PropertyInfo propertyInfo) {
                PropertyDrawer.Draw(node, propertyInfo);
            } else if(memberInfo is FieldInfo fieldInfo) {
                PropertyDrawer.Draw(node, fieldInfo);
            }
        }
    }
    
    protected static List<MemberInfo> GetSerializedMembersNotBeingHiddenWithBaseTypesIncluded(Type type) {
        List<MemberInfo> members = new List<MemberInfo>();
        for(Type? currentType = type; currentType is not null; currentType = currentType.BaseType) {
            members.AddRange(
                currentType
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(prop => {
                        Serialized? serializedAttribute = prop.GetCustomAttribute<Serialized>(false);
                        if(serializedAttribute is null) return false;
                        if(serializedAttribute.Editor == GameEngine.Core.Serialization.Editor.Hidden) return false;
                        return true;
                    })
            );
            members.AddRange(
                currentType
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(prop => {
                        Serialized? serializedAttribute = prop.GetCustomAttribute<Serialized>(false);
                        if(serializedAttribute is null) return false;
                        if(serializedAttribute.Editor == GameEngine.Core.Serialization.Editor.Hidden) return false;
                        return true;
                    })
            );
        }
        return members;
    }
    
    public static List<MemberInfo> GetSerializedMembersDisplayedInInspector(Type type) {
        List<MemberInfo> members = new();
        members.AddRange(
            type
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(prop => {
                    Serialized? serializedAttribute = prop.GetCustomAttribute<Serialized>(false);
                    if(serializedAttribute is null) return false;
                    if(serializedAttribute.Editor == GameEngine.Core.Serialization.Editor.Inspector) return true;
                    return false;
                })
        );
        members.AddRange(
            type
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(prop => {
                    Serialized? serializedAttribute = prop.GetCustomAttribute<Serialized>(false);
                    if(serializedAttribute is null) return false;
                    if(serializedAttribute.Editor == GameEngine.Core.Serialization.Editor.Inspector) return true;
                    return false;
                })
        );
        return members;
    }
    
    protected static IEnumerable<Type> EnumerateFromBaseTypeUp(Type type) {
        Stack<Type> typeStack = new();
        for(Type? currentType = type; currentType is not null; currentType = currentType.BaseType) {
            typeStack.Push(currentType);
        }
        while(typeStack.TryPop(out Type? currentType)) {
            yield return currentType;
        }
    }
}

public abstract class NodeDrawer<TNode> : NodeDrawer where TNode : Node {
    
    protected internal sealed override Type NodeType => typeof(TNode);
    protected internal sealed override void DrawNodeInternal(Node node) => DrawNode(node as TNode);
    protected internal sealed override bool DrawHeaderInternal(Node node) => DrawHeader(node as TNode);
    
    protected abstract void DrawNode(TNode node);
    
    protected virtual bool DrawHeader(TNode node) {
        return DrawDefaultHeader(typeof(TNode));
    }
    
}
