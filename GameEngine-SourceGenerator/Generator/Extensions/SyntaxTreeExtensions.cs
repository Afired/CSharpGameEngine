using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameEngine.Generator.Extensions {
    
    public static class SyntaxTreeExtensions {
        
        public static IEnumerable<UsingDirectiveSyntax> GetUsingDirectives(this SyntaxTree file) {
            return file.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
        }
        
        public static string Format(this IEnumerable<UsingDirectiveSyntax> usingDirectives) {
            return string.Join("\r\n", usingDirectives);
        }
        
    }
    
}
