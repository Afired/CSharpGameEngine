using System.Text;
using GameEngine.SourceGenerator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.SourceGenerator.Tracked {
    
    internal static class PartialNodeGenerator {

        private const string CLASS_ACCESSIBILITY = "public";
        private const string GAME_ENGINE_CORE_NAMESPACE = "GameEngine.Core";
        private const string ENTITY_BASECLASS_NAME = "Node";
        
        internal static void Execute(GeneratorExecutionContext context) {
            
            foreach(NodeDefinition node in NodeRegister.EnumerateDefinitionsFromThisAssembly()) {
                
                StringBuilder propertiesSb = new StringBuilder();
                StringBuilder initializationSb = new StringBuilder();
                StringBuilder addToComponentsListSb = new StringBuilder();
                foreach(NodeDefinition childNode in node.GetAllChildNodes()) {
                    propertiesSb.Append("    [GameEngine.Core.Serialization.Serialized(GameEngine.Core.Serialization.Editor.Hidden)] public ");
                    propertiesSb.Append(childNode.Namespace);
                    propertiesSb.Append('.');
                    propertiesSb.Append(childNode.ClassName);
                    propertiesSb.Append(' ');
                    propertiesSb.Append(childNode.ClassName);
                    propertiesSb.Append(" { get; init; } = null!;\n");
                    
                    initializationSb.Append("\n        ");
                    initializationSb.Append(childNode.ClassName);
                    initializationSb.Append(" = new ");
                    initializationSb.Append(childNode.Namespace);
                    initializationSb.Append('.');
                    initializationSb.Append(childNode.ClassName);
                    initializationSb.Append("(this);");
                    
                    addToComponentsListSb.Append("\n        childNodes.Add(");
                    addToComponentsListSb.Append(childNode.ClassName);
                    addToComponentsListSb.Append(");");
                }
                string properties = propertiesSb.ToString();
                string initialization = initializationSb.ToString();
                string addToComponentsList = addToComponentsListSb.ToString();
                
                var sourceBuilder = new StringBuilder();
                sourceBuilder.Append(
$@"namespace {node.Namespace};

{CLASS_ACCESSIBILITY} partial class {node.ClassName} {{
    
{properties}
    public {node.ClassName}(GameEngine.Core.Nodes.Node? parentNode = null) : this(parentNode, out System.Collections.Generic.List<GameEngine.Core.Nodes.Node> childNodes) {{
        _childNodes = childNodes;
    }}
    
    public {node.ClassName}(GameEngine.Core.Nodes.Node? parentNode, out System.Collections.Generic.List<GameEngine.Core.Nodes.Node> childNodes) : base(parentNode, out childNodes) {{{initialization}
{addToComponentsList}
    }}
    
    [Newtonsoft.Json.JsonConstructor]
    protected {node.ClassName}(bool isJsonConstructed) : base(isJsonConstructed) {{ }}
    
}}
"
                );
                context.AddSource($"{node.ClassName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
            
        }
        
    }
    
}
