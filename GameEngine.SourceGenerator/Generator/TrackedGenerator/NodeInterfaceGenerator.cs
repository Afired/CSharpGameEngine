using System.Text;
using GameEngine.SourceGenerator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.SourceGenerator.Tracked {
    
    internal static class NodeInterfaceGenerator {
        
        internal static void Execute(GeneratorExecutionContext context) {

            foreach(NodeDefinition definition in NodeRegister.EnumerateDefinitionsFromThisAssembly()) {
                
//                string requiredInterfaces = null;
//                if(definition.HasChildNodes) {
//                    StringBuilder sb = new StringBuilder();
//                    sb.Append(" : ");
//                    foreach(NodeDefinition required in definition.GetAllRequiredComponents()) {
//                        sb.Append(required.Namespace);
//                        sb.Append('.');
//                        sb.Append(required.InterfaceName);
//                        sb.Append(", ");
//                    }
//                    sb.Remove(sb.Length - (", ").Length, (", ").Length);
//                    requiredInterfaces = sb.ToString();
//                }
                
                StringBuilder sourceBuilder = new StringBuilder();
                sourceBuilder.Append(
$@"{definition.NamespaceAsFileScopedText()}

public interface {definition.InterfaceName} {{
    {definition.ClassName} {definition.ClassName} {{ get; }}
}}
"
                );
                context.AddSource($"{definition.InterfaceName}",
                    SourceText.From(sourceBuilder.ToString(), Encoding.UTF8)
                );
            }
        }
    }
}
