using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

//interfaz
public interface IPresupuestosRepository{
    void Create(Presupuesto presupuesto);
    List<Presupuesto> getAll();

    Presupuesto GetById(int id);
    void Update(Presupuesto presupuesto);
    void Delete(int id);

    void agregarDetalle(int idPresupuesto, int idProducto, int cantidad);
    List<PresupuestoDetalle> GetPresupuestoDetalles(int id);
}

public class PresupuestoRepositorio : IPresupuestosRepository{
     private string cadenaConexion = @"Data Source=db\Tienda.db;Cache=Shared";
     
    //constructor
    public PresupuestoRepositorio(string CadenaConexion)
    {
        cadenaConexion = CadenaConexion;
    }
    public List<Presupuesto> getAll()
    {
        var queryString = @"
            SELECT p.*, c.ClienteId, c.Nombre, c.Email, c.Telefono
            FROM Presupuestos p
            INNER JOIN Clientes c ON p.ClienteId = c.ClienteId;";
        List<Presupuesto> presupuestos = new List<Presupuesto>();
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {

        SQLiteCommand command = new SQLiteCommand(queryString, connection);
        connection.Open();
    
        using(SQLiteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var cliente = new Cliente
                    {
                        ClienteId = Convert.ToInt32(reader["ClienteId"]),
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString()
                    };

                    var presupuesto = new Presupuesto
                    {
                        IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                        FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                        Cliente = cliente,
                        Detalle = GetPresupuestoDetalles(Convert.ToInt32(reader["idPresupuesto"]))
                    };


                // Obtener los detalles asociados al presupuesto
                presupuesto.Detalle = GetPresupuestoDetalles(presupuesto.IdPresupuesto);

                presupuestos.Add(presupuesto);
            }
        }
        connection.Close();
    }
    return presupuestos;
}
    public Presupuesto GetById(int id){
          var querystring = @"
            SELECT p.*, c.ClienteId, c.Nombre, c.Email, c.Telefono
            FROM Presupuestos p
            LEFT JOIN Clientes c ON p.ClienteId = c.ClienteId
            WHERE p.idPresupuesto = @id";
        var presupuesto = new Presupuesto();

        using(SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(querystring, connection);
                command.Parameters.Add(new SQLiteParameter("@id", id));

            using(SQLiteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                     var cliente = new Cliente
                    {
                        ClienteId = Convert.ToInt32(reader["ClienteId"]),
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString()
                    };

                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    presupuesto.Cliente = cliente;
                    presupuesto.Detalle = GetPresupuestoDetalles(id);
                }
            }
            connection.Close();
            }
        return presupuesto;
        }

    public void Create(Presupuesto presupuesto)
    {
        var query = @"INSERT INTO Presupuestos (ClienteId, FechaCreacion)
            VALUES (@ClienteId, @FechaCreacion)";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SQLiteCommand(query, connection);

            command.Parameters.Add(new SQLiteParameter("@ClienteId", presupuesto.Cliente.ClienteId));
            command.Parameters.Add(new SQLiteParameter("@FechaCreacion", DateTime.Now.ToString("yyyy-MM-dd")));

            command.ExecuteNonQuery();
            connection.Close();   
        }
    }

    

    public void Update(Presupuesto presupuesto){
         var query = @"
            UPDATE Presupuestos 
            SET ClienteId = @ClienteId, FechaCreacion = @Fecha
            WHERE idPresupuesto = @id;";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@id", presupuesto.IdPresupuesto));
                command.Parameters.Add(new SQLiteParameter("@ClienteId", presupuesto.Cliente.ClienteId));
                command.Parameters.Add(new SQLiteParameter("@Fecha", presupuesto.FechaCreacion));
                

                command.ExecuteNonQuery();

                connection.Close();   
            }
    }
    public void Delete(int id){
        var queryDetalles = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto";
        var queryPresupuesto = "DELETE FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";

        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            // Eliminar detalles primero
            var commandDetalles = new SQLiteCommand(queryDetalles, connection);
            commandDetalles.Parameters.Add(new SQLiteParameter("@idPresupuesto", id));
            commandDetalles.ExecuteNonQuery();

            // Eliminar presupuesto después
            var commandPresupuestos = new SQLiteCommand(queryPresupuesto, connection);
            commandPresupuestos.Parameters.Add(new SQLiteParameter("@idPresupuesto", id));
            commandPresupuestos.ExecuteNonQuery(); 
        }

    }

//obtener detalles de presupuesto private List<PresupuestoDetalle> GetPresupuestoDetalles(int id)---------------------

public List<PresupuestoDetalle> GetPresupuestoDetalles(int id)
{
    List<PresupuestoDetalle> presupuestosDetalles = new List<PresupuestoDetalle>();
    string query = @"SELECT p.idProducto, p.Descripcion, p.Precio, d.Cantidad
                     FROM PresupuestosDetalle d
                     JOIN Productos p ON d.idProducto = p.idProducto
                     WHERE d.idPresupuesto = @idPresupuesto;";

    using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
    {
        connection.Open();
        SQLiteCommand command = new SQLiteCommand(query, connection);
        command.Parameters.Add(new SQLiteParameter("@idPresupuesto", id));

        using (SQLiteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                // Crear el objeto Producto
                Productos producto = new Productos
                {
                    IdProducto = reader.GetInt32(0),
                    Descripcion = reader.GetString(1),
                    Precio = reader.GetInt32(2)
                };

                // Leer la cantidad
                int cantidad = reader.GetInt32(3);

                // Crear el PresupuestoDetalle y agregarlo a la lista
                presupuestosDetalles.Add(new PresupuestoDetalle(producto, cantidad));
            }
        }
    }
    return presupuestosDetalles;
}

//AGREGAR PRODUCTOS A PRESUPUESTO
public void agregarDetalle(int IdPresupuesto, int IdProducto, int cantidad)
{
    using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
    {
        connection.Open();
        string verificarQuery = "SELECT * FROM PresupuestosDetalle WHERE idPresupuesto = @IdPresupuesto AND idProducto = @IdProducto;";
        SQLiteCommand verificarCommand = new SQLiteCommand(verificarQuery, connection);
        verificarCommand.Parameters.AddWithValue("@IdPresupuesto", IdPresupuesto);
        verificarCommand.Parameters.AddWithValue("@IdProducto", IdProducto);

        using (SQLiteDataReader reader = verificarCommand.ExecuteReader())
        {
            if (!reader.Read())  // Si no existe el detalle, lo insertamos
            {
                string insertarDetalleQuery = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@IdPresupuesto, @IdProducto, @cantidad);";
                SQLiteCommand insertarCommand = new SQLiteCommand(insertarDetalleQuery, connection);
                insertarCommand.Parameters.AddWithValue("@IdPresupuesto", IdPresupuesto);
                insertarCommand.Parameters.AddWithValue("@IdProducto", IdProducto);
                insertarCommand.Parameters.AddWithValue("@cantidad", cantidad);

                insertarCommand.ExecuteNonQuery();
            }
        }
    }
}



}
