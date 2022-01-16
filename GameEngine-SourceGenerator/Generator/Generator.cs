using Microsoft.CodeAnalysis;

namespace GameEngine.Generator {

// todo: update to incremental generator
// https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/
    [Generator]
    public class Generator : ISourceGenerator {
        
        public void Initialize(GeneratorInitializationContext context) {
            // uncomment for debugging of the source generator process
//            #if DEBUG
//            if(!Debugger.IsAttached) Debugger.Launch();
//            #endif
        }

        public void Execute(GeneratorExecutionContext context) {
            ComponentInterfaceGenerator.Execute(context);
            PartialEntityGenerator.Execute(context);
            PartialComponentGenerator.Execute(context);
        }
        
    }
    
}
