using Turnos.Domain;

namespace Turnos.Tests.Domain;

[TestClass]
public class UsuarioTests
{
    [TestMethod]
    public void CrearUsuario_NombreVacio_LanzaExcepcion()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Usuario("", "guille@mail.com"));
    }

} 