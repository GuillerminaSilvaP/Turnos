namespace Turnos.Domain;

public class Miembro
{
    public Usuario Usuario { get; private set; }
    public Grupo Grupo { get; private set; }
    public int OrdenRotacion { get; private set; }

    public Miembro(Usuario usuario, Grupo grupo, int ordenRotacion)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        ArgumentNullException.ThrowIfNull(grupo);
        ArgumentOutOfRangeException.ThrowIfLessThan(ordenRotacion, 1);

        Usuario = usuario;
        Grupo = grupo;
        OrdenRotacion = ordenRotacion;
    }
}
