/*using System;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceGeneratorsKit;

namespace GameEngine.Generator {

// todo: update to incremental generator
// https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/
    [Generator]
    public class ComponentGenerator : ISourceGenerator {
        
        //const string ATTRIBUTE_NAME = "GenerateComponentInterface";
        const string COMPONENT_BASECLASS_NAME = "Component";
        
        // using source generator kit: https://github.com/kant2002/SourceGeneratorsKit
        private readonly SyntaxReceiver _syntaxReceiver = new DerivedClassesReceiver(COMPONENT_BASECLASS_NAME);
        
        public void Initialize(GeneratorInitializationContext context) {
            #if DEBUG
            if(!Debugger.IsAttached) Debugger.Launch();
            #endif
            
            context.RegisterForSyntaxNotifications(() => _syntaxReceiver);
        }
        
        public void Execute(GeneratorExecutionContext context) {

            Console.WriteLine("Test");
            
            // Retrieve the populated receiver
//            if (!(context.SyntaxContextReceiver is SyntaxReceiver)) {
//                return;
//            }
//            
//            foreach (INamedTypeSymbol classSymbol in _syntaxReceiver.Classes) {
//                
//                if(classSymbol.IsAbstract)
//                    return;
//                
//                string namespaceName = classSymbol.ContainingNamespace.Name;
//                string namespaceAsText = string.IsNullOrEmpty(namespaceName) ? "" : $"namespace {namespaceName};";
//                string className = classSymbol.Name;
//                string interfaceName = $"I{className}";
//                
//                var sourceBuilder = new StringBuilder();
//                sourceBuilder.Append(
//                    $@"
//{namespaceAsText}
//// this is a test
//public interface {interfaceName} {{
//    {className} {className} {{ get; set; }}
//}}
//"
//                );
//                context.AddSource($"{interfaceName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
//            }
            
        }
        
    }
    
}
*/