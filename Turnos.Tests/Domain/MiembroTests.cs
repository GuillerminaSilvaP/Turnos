using Turnos.Domain;

namespace Turnos.Tests.Domain;

[TestClass]
public class MiembroTests
{
    private readonly Usuario _usuario = new("Guille", "guille@mail.com");
    private readonly Grupo _grupo = new("Las del liceo", DayOfWeek.Thursday);

    // ---------- Casos válidos ----------

    [TestMethod]
    public void CrearMiembro_DatosValidos_GuardaUsuarioGrupoYOrden()
    {
        var miembro = new Miembro(_usuario, _grupo, 1);

        Assert.AreSame(_usuario, miembro.Usuario);
        Assert.AreSame(_grupo, miembro.Grupo);
        Assert.AreEqual(1, miembro.OrdenRotacion);
    }

    // ---------- Datos faltantes ----------

    [TestMethod]
    public void CrearMiembro_SinUsuario_LanzaExcepcion()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new Miembro(null!, _grupo, 1));
    }

    [TestMethod]
    public void CrearMiembro_SinGrupo_LanzaExcepcion()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new Miembro(_usuario, null!, 1));
    }

    // ---------- Orden de rotación ----------

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void CrearMiembro_OrdenMenorAUno_LanzaExcepcion(int orden)
    {
        // El orden empieza en 1: la primera anfitriona es la número 1.
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new Miembro(_usuario, _grupo, orden));
    }
}
