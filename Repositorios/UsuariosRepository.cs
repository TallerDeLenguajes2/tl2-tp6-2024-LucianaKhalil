using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

//interfaz
public interface IUsuarioRepository{
    void Create(Usuario usuario);
    List<Usuario> getAll();
    Usuario GetById(int id);
    void Update(Usuario usuario);
    void Delete(int id);

}

public class UsuarioRepositorio : IUsuarioRepository{

    private string cadenaConexion = @"Data Source=db\Tienda.db;Cache=Shared";

    // Constructor
    public UsuarioRepositorio(string CadenaConexion)
    {
        cadenaConexion = CadenaConexion;
    }

    // Obtener todos los usuarios
    public List<Usuario> getAll()
    {
        var queryString = @"SELECT * FROM Usuarios;";
        List<Usuario> usuarios = new List<Usuario>();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            SQLiteCommand command = new SQLiteCommand(queryString, connection);
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Password = reader["Password"].ToString(),
                        RolUsuario = (Rol)Enum.Parse(typeof(Rol), reader["RolUsuario"].ToString())
                    };
                    usuarios.Add(usuario); // Agregar a la lista
                }
            }
            connection.Close();
        }
        return usuarios;
    }

    // Obtener un usuario por ID
    public Usuario GetById(int id)
    {
        Usuario usuario = null;
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            var query = "SELECT * FROM Usuarios WHERE Id = @Id";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", id));
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Password = reader["Password"].ToString(),
                        RolUsuario = (Rol)Enum.Parse(typeof(Rol), reader["RolUsuario"].ToString())
                    };
                }
            }
            connection.Close();
        }
        return usuario;
    }

    // Crear un nuevo usuario
    public void Create(Usuario usuario)
    {
        var query = "INSERT INTO Usuarios (Nombre, NombreUsuario, Password, RolUsuario) VALUES (@Nombre, @NombreUsuario, @Password, @RolUsuario)";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.Add(new SQLiteParameter("@Nombre", usuario.Nombre));
            command.Parameters.Add(new SQLiteParameter("@NombreUsuario", usuario.NombreUsuario));
            command.Parameters.Add(new SQLiteParameter("@Password", usuario.Password));
            command.Parameters.Add(new SQLiteParameter("@RolUsuario", usuario.RolUsuario.ToString()));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    // Actualizar un usuario existente
    public void Update(Usuario usuario)
    {
        var query = "UPDATE Usuarios SET Nombre = @Nombre, NombreUsuario = @NombreUsuario, Password = @Password, RolUsuario = @RolUsuario WHERE Id = @Id";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(query, connection);

            command.Parameters.Add(new SQLiteParameter("@Id", usuario.Id));
            command.Parameters.Add(new SQLiteParameter("@Nombre", usuario.Nombre));
            command.Parameters.Add(new SQLiteParameter("@NombreUsuario", usuario.NombreUsuario));
            command.Parameters.Add(new SQLiteParameter("@Password", usuario.Password));
            command.Parameters.Add(new SQLiteParameter("@RolUsuario", usuario.RolUsuario.ToString()));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    // Eliminar un usuario
    public void Delete(int id)
    {
        var query = "DELETE FROM Usuarios WHERE Id = @Id";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", id));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
