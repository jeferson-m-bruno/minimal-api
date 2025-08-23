using MinimalAPI.Domain.Entities;

namespace  TestMinimalApi.Domain.Entities;

public class AdminTest
{
    [Fact]
    public void TestGeSetProperties()
    {
        // Arrange
        var adm = new Admin();

        // Act
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Password = "teste";
        adm.Perfil = "Adm";

        // Assert
        Assert.Equal(1, adm.Id);
        Assert.Equal("teste@teste.com", adm.Email);
        Assert.Equal("teste", adm.Password);
        Assert.Equal("Adm", adm.Perfil);
    }
    
}