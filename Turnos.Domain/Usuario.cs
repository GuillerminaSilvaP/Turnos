namespace Turnos.Domain;

public class Usuario
{
    public string Nombre { get; private set; }
    public string Email { get; private set; }

    public Usuario(string nombre, string email)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("Nombre no puede ser vacio"); 

        Nombre = nombre;
        Email = email;
    }
}