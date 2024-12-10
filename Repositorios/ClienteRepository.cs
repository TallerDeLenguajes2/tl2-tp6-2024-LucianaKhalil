using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

//interfaz
public interface IClienteRepository{
    void Create(Cliente cliente);
    List<Cliente> getAll();

    Cliente GetById(int id);
    void Update(Cliente cliente);
    void Delete(int id);

}

public class ClienteRepositorio : IClienteRepository{
     private string cadenaConexion = @"Data Source=db\Tienda.db;Cache=Shared";
    //constructor
    public ClienteRepositorio(string CadenaConexion)
    {
        cadenaConexion = CadenaConexion;
    }
      public List<Cliente> getAll()
    {
    var queryString = @"SELECT * FROM Clientes;";
    List<Cliente> clientes = new List<Cliente>();
    using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
    {
        SQLiteCommand command = new SQLiteCommand(queryString, connection);
        connection.Open();
    
        using(SQLiteDataReader reader = command.ExecuteReader())
        {
            //creo objeto tipo Cliente llamado cliente
            while (reader.Read())
            {
                var cliente = new Cliente
                {
                    ClienteId = Convert.ToInt32(reader["ClienteId"]),
                    Nombre = reader["Nombre"].ToString(),
                    Email = reader["Email"].ToString(),
                    Telefono = reader["Telefono"].ToString(), 

                };

                clientes.Add(cliente);//lo agrego a la lista clientes
            }
        }
        connection.Close();
    }
    return clientes;
}
    public Cliente GetById(int id){
        string querystring = "SELECT * FROM Clientes WHERE ClienteId = @id";
        var cliente = new Cliente();

        using(SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(querystring, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));

            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    cliente.Nombre = reader["Nombre"].ToString();
                    cliente.Email = reader["Email"].ToString();
                    cliente.Telefono = reader["Telefono"].ToString();
                }
            }
            connection.Close();
            }
        return cliente;
        }

    public void Create(Cliente cliente)
    {
        var query = @"INSERT INTO Clientes (Nombre, Email, Telefono) 
                    VALUES (@nombre, @email, @tel)";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SQLiteCommand(query, connection);

            command.Parameters.Add(new SQLiteParameter("@nombre", cliente.Nombre));
            command.Parameters.Add(new SQLiteParameter("@email", cliente.Email));
            command.Parameters.Add(new SQLiteParameter("@tel", cliente.Telefono));
            command.ExecuteNonQuery();
            connection.Close();   
        }
    }

    

    public void Update(Cliente cliente)
    {
    var query = "UPDATE Clientes SET Nombre = @nombre, Email = @email, Telefono = @tel WHERE ClienteId = @id;";
    using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
    {
        connection.Open();
        var command = new SQLiteCommand(query, connection);

        // Asigna todos los parámetros necesarios al comando
        command.Parameters.Add(new SQLiteParameter("@nombre", cliente.Nombre));
        command.Parameters.Add(new SQLiteParameter("@email", cliente.Email));
        command.Parameters.Add(new SQLiteParameter("@tel", cliente.Telefono));
        command.Parameters.Add(new SQLiteParameter("@id", cliente.ClienteId));

        command.ExecuteNonQuery();

        connection.Close();
    }
    }

    public void Delete(int id){
        var query = "DELETE FROM Clientes WHERE ClienteId = @id";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            var commandCliente = new SQLiteCommand(query, connection);
            commandCliente.Parameters.Add(new SQLiteParameter("@id", id));
            commandCliente.ExecuteNonQuery(); 
        }

    }


}
