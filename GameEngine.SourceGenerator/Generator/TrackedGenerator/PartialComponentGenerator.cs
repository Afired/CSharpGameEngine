using System.Text;
using GameEngine.SourceGenerator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.SourceGenerator.Tracked {
    
    internal static class PartialComponentGenerator {
        
        private const string GAME_ENGINE_ENTITY_NAMESPACE = "GameEngine.Core.Entities";

        internal static void Execute(GeneratorExecutionContext context) {

            foreach(NodeDefinition definition in NodeRegister.EnumerateDefinitionsFromThisAssembly()) {
                
                const string CLASS_ACCESSIBILITY = "public"; //classSymbol.DeclaredAccessibility.AsText();
                
                string autogenProperties = string.Empty;
                if(definition.HasChildNodes) {
                    StringBuilder sb = new StringBuilder();
                    foreach(NodeDefinition required in definition.GetAllChildNodes()) {
                        sb.Append("    public ");
                        sb.Append(required.Namespace);
                        sb.Append('.');
                        sb.Append(required.ClassName);
                        sb.Append(' ');
                        sb.Append(required.ClassName);
                        sb.Append(" => (Entity as ");
                        sb.Append(required.Namespace);
                        sb.Append('.');
                        sb.Append(required.InterfaceName);
                        sb.Append(")!.");
                        sb.Append(required.ClassName);
                        sb.Append(";\n");
                    }
                    autogenProperties = sb.ToString();
                }
                
                var sourceBuilder = new StringBuilder();
                sourceBuilder.Append(
$@"//using {GAME_ENGINE_ENTITY_NAMESPACE};

{definition.NamespaceAsFileScopedText()}

{CLASS_ACCESSIBILITY} partial class {definition.ClassName} {{

{autogenProperties}

    public {definition.ClassName}({GAME_ENGINE_ENTITY_NAMESPACE}.Entity entity) : base(entity) {{ }}

}}
"
                );
                context.AddSource($"{definition.ClassName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
        }
    }
}
