using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using GameEngine.SourceGenerator.Tracked;
using GameEngine.SourceGenerator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
//
namespace GameEngine.SourceGenerator.HasGen {
//     
//     // todo: update to incremental generator
// // https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/
//     [Generator]
    public class Generator : IIncrementalGenerator {
        
        public void Initialize(IncrementalGeneratorInitializationContext context) {
            // uncomment for debugging of the source generator process
            #if DEBUG
            if(!Debugger.IsAttached) Debugger.Launch();
            // SpinWait.SpinUntil(() => Debugger.IsAttached);
            #endif
            
            // Add the marker attribute to the compilation
            // context.RegisterPostInitializationOutput(Execute);
            
            
            // Do a simple filter for enums
            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider.CreateSyntaxProvider<ClassDeclarationSyntax>(
                    IsSyntaxTargetForGeneration,
                    GetSemanticTargetForGeneration)
                .Where(static m => m is not null)!;
            
            // Combine the selected enums with the `Compilation`
            IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndEnums
                = context.CompilationProvider.Combine(classDeclarations.Collect());
            
            classDeclarations.Collect();
            
            // Generate the source using the compilation and enums
            // context.RegisterSourceOutput(compilationAndEnums,
            //     /*static*/ (spc, source) => Execute(source.Item1, source.Item2, spc));
        }
        
        // static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> enums, SourceProductionContext context) {
        //     if (enums.IsDefaultOrEmpty) {
        //         // nothing to do yet
        //         return;
        //     }
        //
        //     // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
        //     IEnumerable<ClassDeclarationSyntax> distinctEnums = enums.Distinct();
        //
        //     // Convert each EnumDeclarationSyntax to an EnumToGenerate
        //     List<EnumToGenerate> enumsToGenerate = GetTypesToGenerate(compilation, distinctEnums, context.CancellationToken);
        //
        //     // If there were errors in the EnumDeclarationSyntax, we won't create an
        //     // EnumToGenerate for it, so make sure we have something to generate
        //     if (enumsToGenerate.Count > 0)
        //     {
        //         // generate the source code and add it to the output
        //         string result = SourceGenerationHelper.GenerateExtensionClass(enumsToGenerate);
        //         context.AddSource("EnumExtensions.g.cs", SourceText.From(result, Encoding.UTF8));
        //     }
        // }
        
        private static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken _) =>
            node is ClassDeclarationSyntax; //m && IsDerivedFromType(m as ITypeSymbol)
        
        public static bool IsDerivedFromType(ITypeSymbol symbol, string typeName) {
            ITypeSymbol currentBaseSymbol = symbol;
            while((currentBaseSymbol = currentBaseSymbol.BaseType) != null) {
                if(currentBaseSymbol.Name == typeName) {
                    return true;
                }
            }
            return false;
        }
        
        private const string EnumExtensionsAttribute = "NetEscapades.EnumGenerators.EnumExtensionsAttribute";

        private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken ct) {
            // we know the node is a ClassDeclarationSyntax thanks to IsSyntaxTargetForGeneration
            ClassDeclarationSyntax classDeclarationSyntax = (context.Node as ClassDeclarationSyntax)!;

            BaseListSyntax list = classDeclarationSyntax.BaseList;
            
            foreach(BaseTypeSyntax baseTypeSyntax in classDeclarationSyntax.BaseList.Types) {
                ISymbol? symbol = context.SemanticModel.GetSymbolInfo(baseTypeSyntax).Symbol;
            }
            
            // we didn't find the attribute we were looking for
            return null;
        }
        
        
//         public void Execute(IncrementalGeneratorPostInitializationContext context) {
//             AssemblyScanner.ScanOtherAssemblies(context);
//             AssemblyScanner.ScanThisAssembly(context);
//             NodeRegister.Resolve();
//             NodeInterfaceGenerator.Execute(context);
//             PartialNodeGenerator.Execute(context);
//             
// //            PartialComponentGenerator.Execute(context);
// //            PartialEntityGenerator.Execute(context);
//         }
        
    }
    
}
