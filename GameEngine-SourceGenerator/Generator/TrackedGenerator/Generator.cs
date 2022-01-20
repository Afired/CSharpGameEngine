using System.Diagnostics;
using GameEngine.Generator.Tracked.Tracking;
using Microsoft.CodeAnalysis;

namespace GameEngine.Generator.Tracked {

// todo: update to incremental generator
// https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/
    [Generator]
    public class Generator : ISourceGenerator {
        
        public void Initialize(GeneratorInitializationContext context) {
            // uncomment for debugging of the source generator process
            #if DEBUG
            if(!Debugger.IsAttached) Debugger.Launch();
            #endif
        }
        
        public void Execute(GeneratorExecutionContext context) {
            ComponentInterfaceGenerator.Execute(context, out ComponentInterfaceRegister componentInterfaceRegister);
            PartialComponentGenerator.Execute(context, componentInterfaceRegister);
            PartialEntityGenerator.Execute(context, componentInterfaceRegister);
        }
        
    }
    
}
