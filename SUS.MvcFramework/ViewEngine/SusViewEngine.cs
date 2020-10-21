using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace SUS.MvcFramework.ViewEngine
{
    public class SusViewEngine : IViewEngine
    {
        public string GetHtml(string templateCode, object viewModel)
        {
            var csharpCode = GenerateCSharpFromTemplate(templateCode);
            var executableObject = GenerateExecutableCode(csharpCode, viewModel);
            var html = executableObject.GetHtml(viewModel);

            return html;
        }

        private string GenerateCSharpFromTemplate(string templateCode)
        {
            var methodBody = GetMethodBody(templateCode);
            var csharpCode = @"
using System;
using System.Text;
using System.Linq;
using System.Collection.Generic;

namespace ViewNamespace
{
    public class ViewClass : IView
    {
        public string GetHtml(object viewModel)
        {
            var html = new StringBuilder();
            
            " + methodBody + @"

            return html.ToString();
        }
    }
}
";

            return csharpCode;
        }

        private string GetMethodBody(string templateCode)
        {
            return string.Empty;
        }

        private IView GenerateExecutableCode(string csharpCode, object viewModel)
        {
            var compileResult = CSharpCompilation.Create("ViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location));
            if (viewModel != null)
            {
                compileResult = compileResult
                .AddReferences(MetadataReference.CreateFromFile(viewModel.GetType().Assembly.Location));
            }

            var libraries = Assembly.Load(new AssemblyName("netstandart")).GetReferencedAssemblies();

            foreach (var library in libraries)
            {
                compileResult = compileResult
                .AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            compileResult = compileResult.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(csharpCode));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                EmitResult result = compileResult.Emit(memoryStream);
                if (!result.Success)
                {
                    return new ErrorView(result.Diagnostics
                        .Where(x => x.Severity == DiagnosticSeverity.Error)
                        .Select(x => x.GetMessage()), csharpCode);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                var byteAssembly = memoryStream.ToArray();
                var assembly = Assembly.Load(byteAssembly);
                var viewType = assembly.GetType("ViewNamespace.ViewClass");
                var instance = Activator.CreateInstance(viewType);
                return instance as IView;
            }
        }
    }
}
