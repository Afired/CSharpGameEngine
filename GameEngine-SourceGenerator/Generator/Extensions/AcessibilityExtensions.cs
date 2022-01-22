using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GameEngine.Generator.Extensions {
    
    public static class AcessibilityExtensions {
        
        public static string AsText(this Accessibility accessibility) {
            return SyntaxFacts.GetText(accessibility);
        }
        
    }
    
}
