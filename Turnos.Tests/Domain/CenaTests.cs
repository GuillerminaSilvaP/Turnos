using Turnos.Domain;

namespace Turnos.Tests.Domain;

[TestClass]
public class CenaTests
{
    private readonly Grupo _grupo = new("Las del liceo", DayOfWeek.Thursday);
    private readonly Miembro _anfitriona;

    // Jueves 15 de octubre de 2026, el mismo día de la semana que el grupo.
    private static readonly DateOnly UnJueves = new(2026, 10, 15);

    public CenaTests()
    {
        _anfitriona = new Miembro(new Usuario("Guille", "guille@mail.com"), _grupo, 1);
    }

    // ---------- Casos válidos ----------

    [TestMethod]
    public void CrearCena_DatosValidos_GuardaAnfitrionaYFecha()
    {
        var cena = new Cena(_anfitriona, UnJueves);

        Assert.AreSame(_anfitriona, cena.Anfitriona);
        Assert.AreEqual(UnJueves, cena.Fecha);
    }

    [TestMethod]
    public void CrearCena_ElGrupoSeObtieneDeLaAnfitriona()
    {
        // Cena no guarda el grupo: lo saca de su anfitriona, así nunca se contradicen.
        var cena = new Cena(_anfitriona, UnJueves);

        Assert.AreSame(_grupo, cena.Grupo);
    }

    // ---------- Datos inválidos ----------

    [TestMethod]
    public void CrearCena_SinAnfitriona_LanzaExcepcion()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new Cena(null!, UnJueves));
    }

    [TestMethod]
    [DataRow(2026, 10, 14)] // miércoles
    [DataRow(2026, 10, 16)] // viernes
    public void CrearCena_FechaQueNoCaeElDiaDelGrupo_LanzaExcepcion(int anio, int mes, int dia)
    {
        var fecha = new DateOnly(anio, mes, dia);

        Assert.ThrowsExactly<ArgumentException>(() => new Cena(_anfitriona, fecha));
    }
}
