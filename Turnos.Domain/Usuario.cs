using System.Text.RegularExpressions;

namespace Turnos.Domain;

public class Usuario
{
    // Algo sin espacios ni @, un solo @, y un dominio con al menos un punto.
    private static readonly Regex FormatoEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public string Nombre { get; private set; }
    public string Email { get; private set; }

    public Usuario(string nombre, string email)
    {
        Nombre = ValidarNombre(nombre);
        Email = ValidarEmail(email);
    }

    private static string ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.");

        return nombre.Trim();
    }

    private static string ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío.");

        var normalizado = email.Trim().ToLowerInvariant();

        if (!FormatoEmail.IsMatch(normalizado))
            throw new ArgumentException("El email no tiene un formato válido.");

        return normalizado;
    }
}
