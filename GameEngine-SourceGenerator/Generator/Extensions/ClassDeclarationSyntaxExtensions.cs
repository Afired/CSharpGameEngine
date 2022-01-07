using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameEngine.Generator.Extensions {
    
    public static class ClassDeclarationSyntaxExtensions {

        public static string GetNamespace(this ClassDeclarationSyntax source) {
            if(source == null) return null;

            var parent = source.Parent;
            while(parent.IsKind(SyntaxKind.ClassDeclaration)) {
                var parentClass = parent as ClassDeclarationSyntax;

                if(parentClass == null) return null;

                parent = parent.Parent;
            }

            var nameSpace = parent as NamespaceDeclarationSyntax;

            return nameSpace?.Name.ToString();
        }

        public static bool HasAttribute(this ClassDeclarationSyntax source, string attributeName) {
            return source.AttributeLists.Any(x => x.Attributes.Any(y => y.Name.ToString().Contains(attributeName)));
        }

    }
    
}
