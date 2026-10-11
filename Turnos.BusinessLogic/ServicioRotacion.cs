using Turnos.Domain;

namespace Turnos.BusinessLogic;

public class ServicioRotacion
{
    /// <summary>
    /// Decide a quién le toca ser anfitriona en la próxima cena.
    /// Sigue el OrdenRotacion: después de la última anfitriona va la que tiene el orden
    /// siguiente, y al llegar al final vuelve a empezar la ronda.
    /// </summary>
    public Miembro SiguienteAnfitriona(IReadOnlyCollection<Miembro> miembros, Cena? ultimaCena)
    {
        ValidarMiembros(miembros, ultimaCena);

        var enOrden = miembros.OrderBy(m => m.OrdenRotacion).ToList();

        if (ultimaCena is null)
            return enOrden.First();

        // Se busca por número de orden y no por la persona: así funciona aunque
        // la última anfitriona ya no esté en el grupo.
        var ordenAnterior = ultimaCena.Anfitriona.OrdenRotacion;

        return enOrden.FirstOrDefault(m => m.OrdenRotacion > ordenAnterior)
               ?? enOrden.First();
    }

    /// <summary>
    /// Primera fecha que cae en el día indicado, contando desde hoy inclusive.
    /// </summary>
    public DateOnly ProximaFecha(DayOfWeek dia, DateOnly desde)
    {
        var diasHastaElDia = ((int)dia - (int)desde.DayOfWeek + 7) % 7;
        return desde.AddDays(diasHastaElDia);
    }

    /// <summary>
    /// Arma la próxima cena del grupo: quién es la anfitriona y qué día es.
    /// </summary>
    public Cena GenerarProximaCena(IReadOnlyCollection<Miembro> miembros, Cena? ultimaCena, DateOnly hoy)
    {
        var anfitriona = SiguienteAnfitriona(miembros, ultimaCena);

        // Nunca en el pasado, y nunca el mismo día que la última cena.
        var desde = hoy;
        if (ultimaCena is not null && ultimaCena.Fecha >= hoy)
            desde = ultimaCena.Fecha.AddDays(1);

        var fecha = ProximaFecha(anfitriona.Grupo.DiaCena, desde);

        return new Cena(anfitriona, fecha);
    }

    private static void ValidarMiembros(IReadOnlyCollection<Miembro> miembros, Cena? ultimaCena)
    {
        ArgumentNullException.ThrowIfNull(miembros);

        if (miembros.Count == 0)
            throw new InvalidOperationException("El grupo no tiene integrantes.");

        var grupo = miembros.First().Grupo;

        if (miembros.Any(m => m.Grupo != grupo))
            throw new ArgumentException("Todas las integrantes tienen que ser del mismo grupo.");

        if (ultimaCena is not null && ultimaCena.Grupo != grupo)
            throw new ArgumentException("La última cena es de otro grupo.");
    }
}
