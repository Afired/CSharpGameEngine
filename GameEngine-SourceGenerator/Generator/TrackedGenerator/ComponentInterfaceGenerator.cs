using System.Text;
using GameEngine.Generator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.Generator.Tracked {
    
    internal static class ComponentInterfaceGenerator {
        
        internal static void Execute(GeneratorExecutionContext context) {

            foreach(ComponentInterfaceDefinition definition in ComponentInterfaceRegister.EnumerateDefinitionsFromThisAssembly()) {

                string requiredInterfaces = null;
                if(definition.HasRequiredComponents) {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" : ");
                    foreach(ComponentInterfaceDefinition required in definition.GetAllRequiredComponents()) {
                        sb.Append(required.Namespace);
                        sb.Append('.');
                        sb.Append(required.InterfaceName);
                        sb.Append(", ");
                    }
                    sb.Remove(sb.Length - (", ").Length, (", ").Length);
                    requiredInterfaces = sb.ToString();
                }
                
                StringBuilder sourceBuilder = new StringBuilder();
                sourceBuilder.Append(
$@"{definition.NamespaceAsFileScopedText()}

public interface {definition.InterfaceName}{requiredInterfaces} {{
    {definition.ComponentName} {definition.ComponentName} {{ get; }}
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
