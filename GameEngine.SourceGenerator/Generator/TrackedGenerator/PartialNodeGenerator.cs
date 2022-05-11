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
                    propertiesSb.Append("    public ");
                    propertiesSb.Append(childNode.Namespace);
                    propertiesSb.Append('.');
                    propertiesSb.Append(childNode.ClassName);
                    propertiesSb.Append(' ');
                    propertiesSb.Append(childNode.ClassName);
                    propertiesSb.Append(" { get; }\n");
                            
                    initializationSb.Append("        ");
                    initializationSb.Append(childNode.ClassName);
                    initializationSb.Append(" = new ");
                    initializationSb.Append(childNode.Namespace);
                    initializationSb.Append('.');
                    initializationSb.Append(childNode.ClassName);
                    initializationSb.Append("(this);\n");
                    
                    addToComponentsListSb.Append("        ChildNodes.Add(");
                    addToComponentsListSb.Append(childNode.ClassName);
                    addToComponentsListSb.Append(");\n");
                }
                string properties = propertiesSb.ToString();
                string initialization = initializationSb.ToString();
                string addToComponentsList = addToComponentsListSb.ToString();
                
                var sourceBuilder = new StringBuilder();
                sourceBuilder.Append(
$@"//usingDirectives

namespace {node.Namespace};

{CLASS_ACCESSIBILITY} partial class {node.ClassName} {{

{properties}
    public {node.ClassName}(GameEngine.Core.Nodes.Node? parentNode = null) : base(parentNode) {{
{initialization}
{addToComponentsList}
    }}

}}
"
                );
                context.AddSource($"{node.ClassName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
            
        }
        
    }
    
}
