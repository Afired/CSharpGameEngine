using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace GameEngine.SourceGenerator; 

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
        PartialNodeGenerator.Execute(context);
    }
    
}
