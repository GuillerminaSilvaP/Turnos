using Turnos.BusinessLogic;
using Turnos.Domain;

namespace Turnos.Tests.BusinessLogic;

[TestClass]
public class ServicioRotacionTests
{
    private readonly ServicioRotacion _servicio = new();
    private readonly Grupo _grupo = new("Las del liceo", DayOfWeek.Thursday);

    private readonly Miembro _ana;
    private readonly Miembro _belu;
    private readonly Miembro _caro;

    // Jueves 15 y 22 de octubre de 2026.
    private static readonly DateOnly Jueves15 = new(2026, 10, 15);
    private static readonly DateOnly Jueves22 = new(2026, 10, 22);

    public ServicioRotacionTests()
    {
        _ana = CrearMiembro("Ana", 1);
        _belu = CrearMiembro("Belu", 2);
        _caro = CrearMiembro("Caro", 3);
    }

    private Miembro CrearMiembro(string nombre, int orden, Grupo? grupo = null) =>
        new(new Usuario(nombre, $"{nombre.ToLower()}@mail.com"), grupo ?? _grupo, orden);

    // ================= Siguiente anfitriona =================

    [TestMethod]
    public void SiguienteAnfitriona_SinCenasPrevias_EsLaDeMenorOrden()
    {
        // Las paso desordenadas a propósito: lo que manda es el OrdenRotacion, no la posición en la lista.
        var miembros = new[] { _caro, _ana, _belu };

        var anfitriona = _servicio.SiguienteAnfitriona(miembros, ultimaCena: null);

        Assert.AreSame(_ana, anfitriona);
    }

    [TestMethod]
    public void SiguienteAnfitriona_DespuesDeLaPrimera_EsLaSegunda()
    {
        var ultimaCena = new Cena(_ana, Jueves15);

        var anfitriona = _servicio.SiguienteAnfitriona([_ana, _belu, _caro], ultimaCena);

        Assert.AreSame(_belu, anfitriona);
    }

    [TestMethod]
    public void SiguienteAnfitriona_DespuesDeLaUltima_VuelveALaPrimera()
    {
        var ultimaCena = new Cena(_caro, Jueves15);

        var anfitriona = _servicio.SiguienteAnfitriona([_ana, _belu, _caro], ultimaCena);

        Assert.AreSame(_ana, anfitriona);
    }

    [TestMethod]
    public void SiguienteAnfitriona_UnaSolaIntegrante_LeTocaSiempreAElla()
    {
        var ultimaCena = new Cena(_ana, Jueves15);

        var anfitriona = _servicio.SiguienteAnfitriona([_ana], ultimaCena);

        Assert.AreSame(_ana, anfitriona);
    }

    [TestMethod]
    public void SiguienteAnfitriona_LaUltimaAnfitrionaSeFueDelGrupo_SigueConLaQueVeniaDespues()
    {
        // Belu fue anfitriona y después se fue: ya no está en la lista.
        var ultimaCena = new Cena(_belu, Jueves15);

        var anfitriona = _servicio.SiguienteAnfitriona([_ana, _caro], ultimaCena);

        Assert.AreSame(_caro, anfitriona);
    }

    [TestMethod]
    public void SiguienteAnfitriona_EntraUnaNueva_LeTocaCuandoLlegaSuTurno()
    {
        // Dani entra con el orden 4: va al final de la ronda, no se cuela.
        var dani = CrearMiembro("Dani", 4);
        var ultimaCena = new Cena(_caro, Jueves15);

        var anfitriona = _servicio.SiguienteAnfitriona([_ana, _belu, _caro, dani], ultimaCena);

        Assert.AreSame(dani, anfitriona);
    }

    [TestMethod]
    public void SiguienteAnfitriona_SinMiembros_LanzaExcepcion()
    {
        Assert.ThrowsExactly<InvalidOperationException>(
            () => _servicio.SiguienteAnfitriona([], ultimaCena: null));
    }

    [TestMethod]
    public void SiguienteAnfitriona_MiembrosDeOtroGrupo_LanzaExcepcion()
    {
        var otroGrupo = new Grupo("Las de la facu", DayOfWeek.Thursday);
        var intrusa = CrearMiembro("Intrusa", 4, otroGrupo);

        Assert.ThrowsExactly<ArgumentException>(
            () => _servicio.SiguienteAnfitriona([_ana, _belu, intrusa], ultimaCena: null));
    }

    [TestMethod]
    public void SiguienteAnfitriona_UltimaCenaDeOtroGrupo_LanzaExcepcion()
    {
        var otroGrupo = new Grupo("Las de la facu", DayOfWeek.Thursday);
        var cenaAjena = new Cena(CrearMiembro("Otra", 1, otroGrupo), Jueves15);

        Assert.ThrowsExactly<ArgumentException>(
            () => _servicio.SiguienteAnfitriona([_ana, _belu], cenaAjena));
    }

    // ================= Próxima fecha =================

    [TestMethod]
    public void ProximaFecha_HoyEsElDiaDeLaCena_EsHoy()
    {
        var fecha = _servicio.ProximaFecha(DayOfWeek.Thursday, desde: Jueves15);

        Assert.AreEqual(Jueves15, fecha);
    }

    [TestMethod]
    [DataRow(2026, 10, 16)] // viernes
    [DataRow(2026, 10, 19)] // lunes
    [DataRow(2026, 10, 21)] // miércoles
    public void ProximaFecha_OtroDia_EsElJuevesSiguiente(int anio, int mes, int dia)
    {
        var fecha = _servicio.ProximaFecha(DayOfWeek.Thursday, desde: new DateOnly(anio, mes, dia));

        Assert.AreEqual(Jueves22, fecha);
    }

    // ================= Generar la próxima cena =================

    [TestMethod]
    public void GenerarProximaCena_PrimeraCena_EsElProximoJuevesConLaPrimeraDeLaRonda()
    {
        var lunes12 = new DateOnly(2026, 10, 12);

        var cena = _servicio.GenerarProximaCena([_ana, _belu, _caro], ultimaCena: null, hoy: lunes12);

        Assert.AreSame(_ana, cena.Anfitriona);
        Assert.AreEqual(Jueves15, cena.Fecha);
    }

    [TestMethod]
    public void GenerarProximaCena_ConCenaPrevia_EsLaSemanaSiguienteConLaQueSigue()
    {
        // Hoy es el mismo jueves de la última cena: la próxima no puede ser hoy otra vez.
        var ultimaCena = new Cena(_ana, Jueves15);

        var cena = _servicio.GenerarProximaCena([_ana, _belu, _caro], ultimaCena, hoy: Jueves15);

        Assert.AreSame(_belu, cena.Anfitriona);
        Assert.AreEqual(Jueves22, cena.Fecha);
    }

    [TestMethod]
    public void GenerarProximaCena_PasaronSemanasSinCenar_EsElProximoJuevesDesdeHoy()
    {
        // La última fue hace un mes: no se generan cenas en el pasado.
        var ultimaCena = new Cena(_ana, new DateOnly(2026, 9, 17));

        var cena = _servicio.GenerarProximaCena([_ana, _belu, _caro], ultimaCena, hoy: new DateOnly(2026, 10, 19));

        Assert.AreSame(_belu, cena.Anfitriona);
        Assert.AreEqual(Jueves22, cena.Fecha);
    }
}
