using Turnos.Domain;

namespace Turnos.Tests.Domain;

[TestClass]
public class GrupoTests
{
    // ---------- Casos válidos ----------

    [TestMethod]
    public void CrearGrupo_DatosValidos_GuardaNombreYDia()
    {
        var grupo = new Grupo("Las del liceo", DayOfWeek.Thursday);

        Assert.AreEqual("Las del liceo", grupo.Nombre);
        Assert.AreEqual(DayOfWeek.Thursday, grupo.DiaCena);
    }

    [TestMethod]
    public void CrearGrupo_NombreConEspacios_GuardaNombreSinEspaciosAlrededor()
    {
        var grupo = new Grupo("  Las del liceo  ", DayOfWeek.Thursday);

        Assert.AreEqual("Las del liceo", grupo.Nombre);
    }

    // ---------- Nombre inválido ----------

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow(null)]
    public void CrearGrupo_NombreVacio_LanzaExcepcion(string? nombre)
    {
        Assert.ThrowsExactly<ArgumentException>(() => new Grupo(nombre!, DayOfWeek.Thursday));
    }

    // ---------- Día inválido ----------

    [TestMethod]
    public void CrearGrupo_DiaFueraDeRango_LanzaExcepcion()
    {
        // Un enum en C# acepta cualquier número con un cast, aunque no sea un día real.
        Assert.ThrowsExactly<ArgumentException>(() => new Grupo("Las del liceo", (DayOfWeek)9));
    }

    // ---------- Código de invitación ----------

    [TestMethod]
    public void CrearGrupo_GeneraCodigoDeSeisCaracteres()
    {
        var grupo = new Grupo("Las del liceo", DayOfWeek.Thursday);

        Assert.AreEqual(6, grupo.CodigoInvitacion.Length);
    }

    [TestMethod]
    public void CrearGrupo_CodigoSoloTieneLetrasMayusculasYNumerosFacilesDeDictar()
    {
        // Sin 0/O ni 1/I/L, que se confunden al dictarlos o leerlos.
        const string permitidos = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";

        for (int i = 0; i < 100; i++)
        {
            var grupo = new Grupo("Las del liceo", DayOfWeek.Thursday);

            foreach (var caracter in grupo.CodigoInvitacion)
                Assert.Contains(caracter.ToString(), permitidos);
        }
    }

    [TestMethod]
    public void CrearGrupo_DosGrupos_TienenCodigosDistintos()
    {
        var codigos = new HashSet<string>();

        for (int i = 0; i < 100; i++)
            codigos.Add(new Grupo("Las del liceo", DayOfWeek.Thursday).CodigoInvitacion);

        Assert.HasCount(100, codigos);
    }
}
