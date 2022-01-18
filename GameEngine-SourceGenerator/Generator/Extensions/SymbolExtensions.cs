using Microsoft.CodeAnalysis;

namespace GameEngine.Generator.Extensions {
    
    public static class ClassSymbolExtensions {
        
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
