using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ObjectSim.Abstractions;

namespace ObjectSim.OutputModel.Test;

public class TemporalAssembly
{
    public static void CreateTemporalAssembly(string route, string assemblyName)
    {
        const string code = "";

        var compilation = CreateCompilation(code, assemblyName);

        var assemblyPath = Path.Combine(route, assemblyName + ".dll");
        SaveCompilationOnDisc(compilation, assemblyPath);
    }

    public static CSharpCompilation CreateCompilation(string code, string assemblyName)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var systemRuntimeReference = MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll"));

        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IOutputModelTransformer).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Model).Assembly.Location),
            systemRuntimeReference
        };

        return CSharpCompilation.Create(assemblyName,
            [syntaxTree],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }

    public static void SaveCompilationOnDisc(CSharpCompilation compilation, string route)
    {
        using var stream = new FileStream(route, FileMode.Create, FileAccess.Write);
        var emitResult = compilation.Emit(stream);

        if (!emitResult.Success)
        {
            var errors = string.Join(Environment.NewLine, emitResult.Diagnostics
                .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                .Select(diagnostic => diagnostic.ToString()));

            Console.WriteLine("Compilation errors:");
            Console.WriteLine(errors);

            throw new InvalidOperationException("Error when trying to compile temporal assembly: " + errors);
        }
        else
        {
            Console.WriteLine("Successful compilation. Compilation saved on: " + route);
        }
    }

}
