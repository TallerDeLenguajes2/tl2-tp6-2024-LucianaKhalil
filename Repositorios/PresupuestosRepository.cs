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
    var queryString = @"SELECT * FROM Presupuestos;";
    List<Presupuesto> presupuestos = new List<Presupuesto>();
    using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
    {
        SQLiteCommand command = new SQLiteCommand(queryString, connection);
        connection.Open();
    
        using(SQLiteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var presupuesto = new Presupuesto
                {
                    IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                    NombreDestinatario = reader["NombreDestinatario"].ToString(),
                    FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
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
        string querystring = "SELECT * FROM Presupuestos WHERE idPresupuesto = @id";
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
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCreacion = DateTime.Parse(reader["FechaCreacion"].ToString());
                }
            }
            connection.Close();
            }
        return presupuesto;
        }

    public void Create(Presupuesto presupuesto)
    {
        var query = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
                    VALUES (@NombreDestinatario, @FechaCreacion)";
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();
            var command = new SQLiteCommand(query, connection);

            command.Parameters.Add(new SQLiteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SQLiteParameter("@FechaCreacion", DateTime.Now.ToString("yyyy-MM-dd")));

            command.ExecuteNonQuery();
            connection.Close();   
        }
    }

    

    public void Update(Presupuesto presupuesto){
        var query ="UPDATE Presupuestos SET NombreDestinatario = @Nombre, FechaCreacion= @Fecha WHERE idPresupuesto = @id;";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {

                connection.Open();
                var command = new SQLiteCommand(query, connection);

                command.Parameters.Add(new SQLiteParameter("@id", presupuesto.IdPresupuesto));
                command.Parameters.Add(new SQLiteParameter("@Nombre", presupuesto.NombreDestinatario));
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
public void agregarDetalle(int idPresupuesto, int idProducto, int cantidad)
{
    var querystring = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad)";

    try
    {
        using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
        {
            connection.Open();

            SQLiteCommand command = new SQLiteCommand(querystring, connection);
            command.Parameters.Add(new SQLiteParameter("@idPresupuesto", idPresupuesto));
            command.Parameters.Add(new SQLiteParameter("@idProducto", idProducto));
            command.Parameters.Add(new SQLiteParameter("@Cantidad", cantidad));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    catch (Exception ex)
    {
        // Puedes agregar un log o mostrar el error
        Console.WriteLine("Error al agregar el detalle: " + ex.Message);
    }
}

}
