namespace Turnos.Domain;

public class Cena
{
    public Miembro Anfitriona { get; private set; }
    public DateOnly Fecha { get; private set; }

    // El grupo no se guarda aparte: sale de la anfitriona, así nunca se contradicen.
    public Grupo Grupo => Anfitriona.Grupo;

    public Cena(Miembro anfitriona, DateOnly fecha)
    {
        ArgumentNullException.ThrowIfNull(anfitriona);

        if (fecha.DayOfWeek != anfitriona.Grupo.DiaCena)
            throw new ArgumentException(
                $"La cena tiene que ser un {anfitriona.Grupo.DiaCena}, que es el día del grupo.");

        Anfitriona = anfitriona;
        Fecha = fecha;
    }
}
