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

        public async Task<List<Proveedores>> ObtenerProveedores(bool mostrarInactivos = false)
        {
            var proveedores = new List<Proveedores>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

              
                int estadoFiltro = mostrarInactivos ? 0 : 1;

                var command = new SqlCommand("SELECT * FROM Proveedores WHERE Estado = @Estado", connection);
                command.Parameters.AddWithValue("@Estado", estadoFiltro);

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
    
        public async Task DeleteProveedor(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Proveedores SET Estado = 0 WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Insumos>> ObtenerInsumos(bool mostrarInactivos = false)
        {
            var insumos = new List<Insumos>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                int estadoFiltro = mostrarInactivos ? 0 : 1;

               
                string sql = @"
                    SELECT i.*, p.Nombre AS NombreProveedor 
                    FROM Insumos i 
                    INNER JOIN Proveedores p ON i.Id_Proveedor = p.Id 
                    WHERE i.Estado = @Estado";

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Estado", estadoFiltro);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        insumos.Add(new Insumos
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            StockActual = (int)reader["StockActual"],
                            UnidadMedida = reader["UnidadMedida"].ToString(),
                            PrecioUnitario = (decimal)reader["PrecioUnitario"],
                            FechaCaducidad = (DateTime)reader["FechaCaducidad"],
                            Id_Proveedor = (int)reader["Id_Proveedor"],
                            Estado = (bool)reader["Estado"],

                          
                            NombreProveedor = reader["NombreProveedor"].ToString()
                        });
                    }
                }
            }
            return insumos;
        }
        public async Task InsertInsumo(Insumos insumo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Insumos (Nombre, StockActual, UnidadMedida, PrecioUnitario, FechaCaducidad, Id_Proveedor) VALUES (@Nombre, @StockActual, @UnidadMedida, @PrecioUnitario, @FechaCaducidad, @Id_Proveedor)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", insumo.Nombre);
                    cmd.Parameters.AddWithValue("@StockActual", insumo.StockActual);
                    cmd.Parameters.AddWithValue("@UnidadMedida", insumo.UnidadMedida);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", insumo.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@FechaCaducidad", insumo.FechaCaducidad);
                    cmd.Parameters.AddWithValue("@Id_Proveedor", insumo.Id_Proveedor);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

       
        public async Task<Insumos> ObtenerInsumoPorId(int id)
        {
            Insumos insumo = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT * FROM Insumos WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            insumo = new Insumos
                            {
                                Id = (int)reader["Id"],
                                Nombre = reader["Nombre"].ToString(),
                                StockActual = (int)reader["StockActual"],
                                UnidadMedida = reader["UnidadMedida"].ToString(),
                                PrecioUnitario = (decimal)reader["PrecioUnitario"],
                                FechaCaducidad = (DateTime)reader["FechaCaducidad"],
                                Id_Proveedor = (int)reader["Id_Proveedor"],
                                Estado = (bool)reader["Estado"]
                            };
                        }
                    }
                }
            }
            return insumo;
        }

        
        public async Task UpdateInsumo(Insumos insumo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Insumos SET Nombre = @Nombre, StockActual = @StockActual, UnidadMedida = @UnidadMedida, PrecioUnitario = @PrecioUnitario, FechaCaducidad = @FechaCaducidad, Id_Proveedor = @Id_Proveedor WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", insumo.Nombre);
                    cmd.Parameters.AddWithValue("@StockActual", insumo.StockActual);
                    cmd.Parameters.AddWithValue("@UnidadMedida", insumo.UnidadMedida);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", insumo.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@FechaCaducidad", insumo.FechaCaducidad);
                    cmd.Parameters.AddWithValue("@Id_Proveedor", insumo.Id_Proveedor);
                    cmd.Parameters.AddWithValue("@Id", insumo.Id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

     
        public async Task DeleteInsumo(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
              
                string query = "UPDATE Insumos SET Estado = 0 WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


 
        public async Task<List<Proveedores>> ObtenerProveedoresActivos()
        {
            var proveedores = new List<Proveedores>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
            
                var command = new SqlCommand("SELECT * FROM Proveedores WHERE Estado = 1", connection);
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


    }
}
