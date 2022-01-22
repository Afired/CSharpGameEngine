using System.Text;
using GameEngine.Generator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.Generator.Tracked {
    
    internal static class PartialComponentGenerator {
        
        private const string GAME_ENGINE_ENTITY_NAMESPACE = "GameEngine.Entities";

        internal static void Execute(GeneratorExecutionContext context) {

            foreach(ComponentInterfaceDefinition definition in ComponentInterfaceRegister.EnumerateDefinitionsFromThisAssembly()) {
                
                const string CLASS_ACCESSIBILITY = "public"; //classSymbol.DeclaredAccessibility.AsText();
                
                string autogenProperties = string.Empty;
                if(definition.HasRequiredComponents) {
                    StringBuilder sb = new StringBuilder();
                    foreach(ComponentInterfaceDefinition required in definition.GetAllRequiredComponents()) {
                        sb.Append("    public ");
                        sb.Append(required.Namespace);
                        sb.Append('.');
                        sb.Append(required.ComponentName);
                        sb.Append(' ');
                        sb.Append(required.ComponentName);
                        sb.Append(" => (Entity as ");
                        sb.Append(required.Namespace);
                        sb.Append('.');
                        sb.Append(required.InterfaceName);
                        sb.Append(")!.");
                        sb.Append(required.ComponentName);
                        sb.Append(";\n");
                    }
                    autogenProperties = sb.ToString();
                }
                
                var sourceBuilder = new StringBuilder();
                sourceBuilder.Append(
$@"//using {GAME_ENGINE_ENTITY_NAMESPACE};

{definition.NamespaceAsFileScopedText()}

{CLASS_ACCESSIBILITY} partial class {definition.ComponentName} {{

{autogenProperties}

    public {definition.ComponentName}({GAME_ENGINE_ENTITY_NAMESPACE}.Entity entity) : base(entity) {{ }}

}}
"
                );
                context.AddSource($"{definition.ComponentName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
        }
    }
}
