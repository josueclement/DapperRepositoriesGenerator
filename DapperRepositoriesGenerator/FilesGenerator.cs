using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DapperRepositoriesGenerator;

public class Dummy
{
    public void Foo()
    {
        var generator = new FilesGenerator()
            .SetSqlGeneratorOptions(options =>
            {
                options.ParameterPrefix = "@";
                options.Quote = "`";
            });
    }
}

public class FilesGenerator
{
    private SqlGeneratorOptions _sqlGeneratorOptions;
    private SqlGenerator _sqlGenerator;
    private RepositoryGeneratorOptions _repositoryGeneratorOptions;
    private RepositoryGenerator _repositoryGenerator;
    private ServiceGeneratorOptions _serviceGeneratorOptions;
    private ServiceGenerator _serviceGenerator;
    private string? _sqlScriptPath;
    private string? _entitiesPath;
    private string? _repositoriesInterfacesPath;
    private string? _repositoriesImplementationsPath;
    private string? _servicesInterfacesPath;
    private string? _servicesImplementationsPath;
    private bool? _generateRepositoriesBaseInterface;
    private bool? _generateServicesBaseInterface;

    public FilesGenerator()
    {
        _sqlGeneratorOptions = new SqlGeneratorOptions();
        _sqlGenerator = new SqlGenerator(_sqlGeneratorOptions);
        _repositoryGeneratorOptions = new RepositoryGeneratorOptions();
        _repositoryGenerator = new RepositoryGenerator(_sqlGenerator,  _repositoryGeneratorOptions);
        _serviceGeneratorOptions = new ServiceGeneratorOptions();
        _serviceGenerator = new ServiceGenerator(_serviceGeneratorOptions);
    }
    
    public FilesGenerator SetSqlGeneratorOptions(Action<SqlGeneratorOptions> options)
    {
        options.Invoke(_sqlGeneratorOptions);
        return this;
    }
    
    public FilesGenerator SetRepositoriesGeneratorOptions(Action<RepositoryGeneratorOptions> options)
    {
        options.Invoke(_repositoryGeneratorOptions);
        return this;
    }
    
    public FilesGenerator SetServicesGeneratorOptions(Action<ServiceGeneratorOptions> options)
    {
        options.Invoke(_serviceGeneratorOptions);
        return this;
    }

    public FilesGenerator GenerateSqlCreationScript(string path)
    {
        _sqlScriptPath = path;
        return this;
    }

    public FilesGenerator GenerateEntities(string path)
    {
        _entitiesPath = path;
        return this;
    }

    public FilesGenerator GenerateRepositories(
        string interfacesPath,
        string implementationsPath,
        bool generateBaseInterface = true)
    {
        _repositoriesInterfacesPath = interfacesPath;
        _repositoriesImplementationsPath = implementationsPath;
        _generateRepositoriesBaseInterface = generateBaseInterface;
        return this;
    }

    public FilesGenerator GenerateServices(
        string interfacesPath,
        string implementationsPath,
        bool generateBaseInterface = true)
    {
        _servicesInterfacesPath = interfacesPath;
        _servicesImplementationsPath = implementationsPath;
        _generateServicesBaseInterface = generateBaseInterface;
        return this;
    }

    public async Task GenerateFilesAsync(params DbTable[] tables)
    {
        await GenerateRepositoryGenericInterface();
        await GenerateServiceGenericInterface();

        foreach (var table in tables)
        {
            //TODO: 
        }
    }

    private async Task GenerateRepositoryGenericInterface()
    {
        if (_generateRepositoriesBaseInterface is true && _repositoriesInterfacesPath is not null)
        {
            var filePath = Path.Combine(_repositoriesInterfacesPath, "IRepository.cs");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            var content = _repositoryGenerator.GenerateRepositoryGenericInterface();
            await writer.WriteAsync(content);
        }
    }

    private async Task GenerateServiceGenericInterface()
    {
        
    }
}