using System;
using System.Diagnostics;
using GameEngine.SourceGenerator.Tracked.Tracking;
using Microsoft.CodeAnalysis;

namespace GameEngine.SourceGenerator.Tracked {

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
            try {
                AssemblyScanner.ScanOtherAssemblies(context);
                AssemblyScanner.ScanThisAssembly(context);
                NodeRegister.Resolve();
                NodeInterfaceGenerator.Execute(context);
                PartialNodeGenerator.Execute(context);
//                PartialComponentGenerator.Execute(context);
//                PartialEntityGenerator.Execute(context);
            } catch(Exception exception) {
                // todo: report diagnostics
                // these currently dont work on runtime, but when building solution which is not very helpful in this case
            }
        }
        
    }
    
}
