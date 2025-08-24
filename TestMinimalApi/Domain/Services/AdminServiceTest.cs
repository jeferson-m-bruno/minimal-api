using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Services;
using MinimalAPI.Infra.DB;

namespace TestMinimalApi.Domain.Services;

public class AdminServiceTest
{
    private MyContext CreateContextTest()
    {
        var assemblyPath = Path.GetDirectoryName(typeof(AdminServiceTest).Assembly.Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath!, "..", "..", ".."));
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        var configuration = builder.Build();
        return new MyContext(configuration);
    }
    
    [Fact]
    public void TestSaveAdmin()
    {
        // Testando regra com banco de dados
        
        // Arrange
        var context = CreateContextTest();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

        var adm = new Admin();
        adm.Email = "teste@teste.com";
        adm.Password = "teste";
        adm.Perfil = "Adm";

        var administradorServico = new AdminService(context);

        // Act
        administradorServico.Insert(adm);

        // Assert
        Assert.Single(administradorServico.GetAll(0));
    }
    
    [Fact]
    public void TestFindId()
    {
        // Arrange
        var context = CreateContextTest();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

        var adm = new Admin();
        adm.Email = "teste@teste.com";
        adm.Password = "teste";
        adm.Perfil = "Adm";

        var administradorServico = new AdminService(context);

        // Act
        administradorServico.Insert(adm);
        var admDoBanco = administradorServico.GetById(adm.Id);

        // Assert
        Assert.Equal(1, admDoBanco?.Id);
    }
}