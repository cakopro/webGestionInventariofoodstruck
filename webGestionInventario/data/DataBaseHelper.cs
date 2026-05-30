using Microsoft.Data.SqlClient;
using webGestionInventario.Model;
namespace webGestionInventario.data

{
    public class DataBaseHelper
    {
        private readonly string _connectionString;
        public DataBaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MiConexion");
        }

        public async Task<List<Proveedores>> ObtenerProveedores()
        {
            var proveedores = new List<Proveedores>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Proveedores", connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        proveedores.Add(new Proveedores
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Rut = reader["Rut"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Empresa = reader["Empresa"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Estado = (bool)reader["Estado"]
                        });
                    }
                }

            }
            return proveedores;
        }
        public async Task InsertProveedor(Proveedores provedor)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Proveedores (Nombre, Telefono, Rut, Correo, Empresa, Direccion) VALUES (@Nombre,@Telefono,@Rut,@Correo,@Empresa,@Direccion)";
                using (SqlCommand cmd = new SqlCommand(query, conn)) {
                    cmd.Parameters.AddWithValue("@Nombre",provedor.Nombre);
                    cmd.Parameters.AddWithValue("@Telefono", provedor.Telefono);
                    cmd.Parameters.AddWithValue("@Rut", provedor.Rut);
                    cmd.Parameters.AddWithValue("@Correo", provedor.Correo);
                    cmd.Parameters.AddWithValue("@Empresa", provedor.Empresa);
                    cmd.Parameters.AddWithValue("@Direccion", provedor.Direccion);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
