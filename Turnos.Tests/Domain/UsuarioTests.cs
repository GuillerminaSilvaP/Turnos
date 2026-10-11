using Turnos.Domain;

namespace Turnos.Tests.Domain;

[TestClass]
public class UsuarioTests
{
    // ---------- Casos válidos ----------

    [TestMethod]
    public void CrearUsuario_DatosValidos_GuardaNombreYEmail()
    {
        var usuario = new Usuario("Guille", "guille@mail.com");

        Assert.AreEqual("Guille", usuario.Nombre);
        Assert.AreEqual("guille@mail.com", usuario.Email);
    }

    [TestMethod]
    public void CrearUsuario_NombreConEspacios_GuardaNombreSinEspaciosAlrededor()
    {
        var usuario = new Usuario("  Guille  ", "guille@mail.com");

        Assert.AreEqual("Guille", usuario.Nombre);
    }

    [TestMethod]
    public void CrearUsuario_EmailConMayusculasYEspacios_GuardaEmailNormalizado()
    {
        var usuario = new Usuario("Guille", "  Guille@Mail.COM ");

        Assert.AreEqual("guille@mail.com", usuario.Email);
    }

    // ---------- Nombre inválido ----------

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(null)]
    public void CrearUsuario_NombreVacio_LanzaExcepcion(string? nombre)
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Usuario(nombre!, "guille@mail.com"));
    }

    // ---------- Email inválido ----------

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(null)]
    public void CrearUsuario_EmailVacio_LanzaExcepcion(string? email)
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Usuario("Guille", email!));
    }

    [TestMethod]
    public void CrearUsuario_EmailSinArroba_LanzaExcepcion()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Usuario("Guille", "guillemail.com"));
    }

    [TestMethod]
    [DataRow("@mail.com")]        // nada antes del @
    [DataRow("guille@")]          // nada después del @
    [DataRow("guille@mail")]      // dominio sin punto
    [DataRow("guille@@mail.com")] // dos @
    [DataRow("gui lle@mail.com")] // espacio en el medio
    public void CrearUsuario_EmailMalFormado_LanzaExcepcion(string email)
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Usuario("Guille", email));
    }
}
