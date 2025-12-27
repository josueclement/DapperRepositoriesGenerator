using System.IO;
using System.Reflection;

namespace DapperRepositoriesGenerator;

public static class AssemblyHelper
{
    public static readonly Assembly Assembly = typeof(AssemblyHelper).Assembly;
    
    public static Stream GetEmbeddedStream(string resourceName)
        => Assembly.GetManifestResourceStream(resourceName)
           ?? throw new FileNotFoundException($"Embedded resource {resourceName} not found");
}