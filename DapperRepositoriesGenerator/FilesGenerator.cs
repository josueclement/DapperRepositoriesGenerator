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
    private EntityGeneratorOptions _entityGeneratorOptions;
    private EntityGenerator _entityGenerator;
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
        _entityGeneratorOptions = new EntityGeneratorOptions();
        _entityGenerator = new EntityGenerator(_entityGeneratorOptions);
    }
    
    public FilesGenerator SetSqlGeneratorOptions(Action<SqlGeneratorOptions> options)
    {
        options.Invoke(_sqlGeneratorOptions);
        return this;
    }
    
    public FilesGenerator SetRepositoryGeneratorOptions(Action<RepositoryGeneratorOptions> options)
    {
        options.Invoke(_repositoryGeneratorOptions);
        return this;
    }
    
    public FilesGenerator SetServiceGeneratorOptions(Action<ServiceGeneratorOptions> options)
    {
        options.Invoke(_serviceGeneratorOptions);
        return this;
    }

    public FilesGenerator SetEntityGeneratorOptions(Action<EntityGeneratorOptions> options)
    {
        options.Invoke(_entityGeneratorOptions);
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
        var sqlSb = new StringBuilder();
        
        await GenerateRepositoryGenericInterface().ConfigureAwait(false);
        await GenerateServiceGenericInterface().ConfigureAwait(false);

        foreach (var table in tables)
        {
            await GenerateRepositoryInterface(table).ConfigureAwait(false);
            await GenerateServiceInterface(table).ConfigureAwait(false);
            await GenerateRepository(table).ConfigureAwait(false);
            await GenerateService(table).ConfigureAwait(false);
            await GenerateEntity(table).ConfigureAwait(false);
            GenerateSqlScriptTable(sqlSb, table);
        }

        await GenerateScriptFile(sqlSb).ConfigureAwait(false);
    }

    private async Task GenerateRepositoryGenericInterface()
    {
        if (_generateRepositoriesBaseInterface is true && _repositoriesInterfacesPath is not null)
        {
            var filePath = Path.Combine(_repositoriesInterfacesPath, "IRepository.cs");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            //TODO: add FilesGeneratorOptions with SqlFilesEncoding and CsharpFilesEncoding
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            var content = _repositoryGenerator.GenerateRepositoryGenericInterface();
            await writer.WriteAsync(content).ConfigureAwait(false);
        }
    }

    private async Task GenerateServiceGenericInterface()
    {
        if (_generateServicesBaseInterface is true && _servicesInterfacesPath is not null)
        {
            var filePath = Path.Combine(_servicesInterfacesPath, "IService.cs");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            var content = _serviceGenerator.GenerateServiceGenericInterface();
            await writer.WriteAsync(content).ConfigureAwait(false);
        }
    }

    private async Task GenerateRepositoryInterface(DbTable table)
    {
        if (_repositoriesInterfacesPath is not null)
        {
            var filePath = Path.Combine(_repositoriesInterfacesPath, $"I{table.TableName}Repository.cs");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            var content = _repositoryGenerator.GenerateRepositoryInterface(table);
            await writer.WriteAsync(content).ConfigureAwait(false);
        }
    }

    private async Task GenerateServiceInterface(DbTable table)
    {
        if (_servicesInterfacesPath is not null)
        {
            var filePath = Path.Combine(_servicesInterfacesPath, $"I{table.TableName}Service.cs");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            var content = _serviceGenerator.GenerateServiceInterface(table);
            await writer.WriteAsync(content).ConfigureAwait(false);
        }   
    }

    private async Task GenerateRepository(DbTable table)
    {
        if (_repositoriesImplementationsPath is not null)
        {
            var filePath = Path.Combine(_repositoriesImplementationsPath, $"{table.TableName}Repository.cs");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            var content = _repositoryGenerator.GenerateRepository(table);
            await writer.WriteAsync(content).ConfigureAwait(false);
        }
    }

    private async Task GenerateService(DbTable table)
    {
        if (_servicesImplementationsPath is not null)
        {
            var filePath = Path.Combine(_servicesImplementationsPath, $"{table.TableName}Service.cs");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            var content = _serviceGenerator.GenerateService(table);
            await writer.WriteAsync(content).ConfigureAwait(false);
        }
    }
    
    private async Task GenerateEntity(DbTable table)
    {
        if (_entitiesPath is not null)
        {
            var filePath = Path.Combine(_entitiesPath, $"{table.TableName}.cs");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            var content = _entityGenerator.GenerateEntity(table);
            await writer.WriteAsync(content).ConfigureAwait(false);
        }
    }

    private void GenerateSqlScriptTable(StringBuilder sb, DbTable table)
    {
        sb.AppendLine(_sqlGenerator.GenerateCreateTableScript(table));
        sb.AppendLine();
    }

    private async Task GenerateScriptFile(StringBuilder content)
    {
        if (_sqlScriptPath is not null)
        {
            var filePath = Path.Combine(_sqlScriptPath, "Script.sql");
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            await writer.WriteAsync(content.ToString()).ConfigureAwait(false);
        }
    }
}