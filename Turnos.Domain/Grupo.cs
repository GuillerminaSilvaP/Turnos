using System.Security.Cryptography;

namespace Turnos.Domain;

public class Grupo
{
    // Sin 0/O ni 1/I/L, que se confunden al dictar o leer el código.
    private const string CaracteresCodigo = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
    private const int LargoCodigo = 6;

    public string Nombre { get; private set; }
    public DayOfWeek DiaCena { get; private set; }
    public string CodigoInvitacion { get; private set; }

    public Grupo(string nombre, DayOfWeek diaCena)
    {
        Nombre = ValidarNombre(nombre);
        DiaCena = ValidarDia(diaCena);
        CodigoInvitacion = GenerarCodigo();
    }

    private static string ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del grupo no puede estar vacío.");

        return nombre.Trim();
    }

    private static DayOfWeek ValidarDia(DayOfWeek dia)
    {
        if (!Enum.IsDefined(dia))
            throw new ArgumentException("El día de la cena no es válido.");

        return dia;
    }

    private static string GenerarCodigo()
    {
        return RandomNumberGenerator.GetString(CaracteresCodigo, LargoCodigo);
    }
}
