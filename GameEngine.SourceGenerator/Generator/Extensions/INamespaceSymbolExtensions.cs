using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace GameEngine.SourceGenerator.Extensions {
    
    public static class INamespaceSymbolExtensions {
        
        public static IEnumerable<INamespaceSymbol> GetAllNamespaces(this INamespaceSymbol namespaceSymbol) {
            foreach (INamespaceSymbol symbol in namespaceSymbol.GetNamespaceMembers()) {
                yield return symbol;
                foreach (INamespaceSymbol childSymbol in symbol.GetAllNamespaces()) {
                    yield return childSymbol;
                }
            }
        }
        
    }
    
}
