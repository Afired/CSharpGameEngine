using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace GameEngine.Generator.Extensions {
    
    public static class ClassSymbolExtensions {
        
        // can return INamedTypeSymbol of TypeKind.Error when the interface is auto generated in a different assembly
        public static IEnumerable<INamedTypeSymbol> GetAllInterfaces(this INamedTypeSymbol classSymbol) {
            return classSymbol.Interfaces;
        }
        
        public static IEnumerable<INamedTypeSymbol> GetAllInterfacesRecursively(this INamedTypeSymbol classSymbol) {
            return classSymbol.Interfaces.RecursiveSelect(@interface => @interface.Interfaces);
        }
        
        public static bool IsDerivedFromType(this INamedTypeSymbol symbol, string typeName) {
            INamedTypeSymbol currentBaseSymbol = symbol;
            while((currentBaseSymbol = currentBaseSymbol.BaseType) != null) {
                if(currentBaseSymbol.Name == typeName) {
                    return true;
                }
            }
            return false;
        }
        
    }
    
}
