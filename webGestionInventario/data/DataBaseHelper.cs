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

        public async Task RegistrarVenta(Venta venta, List<DetalleVenta> detalles)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        
                        string insertVentaQuery = @"
                            INSERT INTO Ventas (NombreCliente, Fecha, IVA, Total) 
                            OUTPUT INSERTED.Id 
                            VALUES (@NombreCliente, @Fecha, @IVA, @Total)";

                        int ventaId;
                        using (var cmdVenta = new SqlCommand(insertVentaQuery, connection, transaction))
                        {
                            cmdVenta.Parameters.AddWithValue("@NombreCliente", (object)venta.NombreCliente ?? DBNull.Value);
                            cmdVenta.Parameters.AddWithValue("@Fecha", venta.Fecha);
                            cmdVenta.Parameters.AddWithValue("@IVA", venta.IVA);
                            cmdVenta.Parameters.AddWithValue("@Total", venta.Total);
                            ventaId = (int)await cmdVenta.ExecuteScalarAsync();
                        }

                        foreach (var detalle in detalles)
                        {
                            
                            string insertDetalleQuery = @"
                                INSERT INTO DetalleVenta (IdVenta, IdProducto, Producto, Precio, Cantidad, Subtotal) 
                                VALUES (@IdVenta, @IdProducto, @Producto, @Precio, @Cantidad, @Subtotal)";

                            using (var cmdDetalle = new SqlCommand(insertDetalleQuery, connection, transaction))
                            {
                                cmdDetalle.Parameters.AddWithValue("@IdVenta", ventaId);
                                cmdDetalle.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                                cmdDetalle.Parameters.AddWithValue("@Producto", detalle.Producto);
                                cmdDetalle.Parameters.AddWithValue("@Precio", detalle.Precio);
                                cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                                cmdDetalle.Parameters.AddWithValue("@Subtotal", detalle.Subtotal);
                                await cmdDetalle.ExecuteNonQueryAsync();
                            }

                            
                            string checkAndUpdateStockQuery = @"
                                DECLARE @Insuficiente TABLE (Nombre NVARCHAR(100));

                                INSERT INTO @Insuficiente (Nombre)
                                SELECT i.Nombre
                                FROM Insumos i
                                INNER JOIN Recetas r ON i.Id = r.Id_Insumo
                                WHERE r.Id_Producto = @IdProducto 
                                  AND i.StockActual < (r.CantidadRequerida * @CantidadVendida);

                                IF EXISTS (SELECT 1 FROM @Insuficiente)
                                BEGIN
                                    SELECT Nombre FROM @Insuficiente;
                                END
                                ELSE
                                BEGIN
                                    UPDATE Insumos 
                                    SET StockActual = StockActual - (r.CantidadRequerida * @CantidadVendida)
                                    FROM Insumos i
                                    INNER JOIN Recetas r ON i.Id = r.Id_Insumo
                                    WHERE r.Id_Producto = @IdProducto;
                                    
                                    SELECT NULL; -- Indicar éxito
                                END";

                            using (var cmdStock = new SqlCommand(checkAndUpdateStockQuery, connection, transaction))
                            {
                                cmdStock.Parameters.AddWithValue("@CantidadVendida", detalle.Cantidad);
                                cmdStock.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                                
                                using (var reader = await cmdStock.ExecuteReaderAsync())
                                {
                                    if (await reader.ReadAsync() && !reader.IsDBNull(0))
                                    {
                                        string insumoFaltante = reader.GetString(0);
                                        throw new InvalidOperationException($"Stock insuficiente para el insumo: {insumoFaltante} al procesar {detalle.Producto}.");
                                    }
                                }
                            }
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<List<Producto>> ObtenerProductos()
        {
            var productos = new List<Producto>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var command = new SqlCommand("SELECT Id, Nombre, PrecioVenta FROM Productos", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productos.Add(new Producto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"]?.ToString() ?? "Sin Nombre",
                            PrecioVenta = reader["PrecioVenta"] != DBNull.Value ? Convert.ToDecimal(reader["PrecioVenta"]) : 0m
                        });
                    }
                }
            }
            return productos;
        }

        public async Task GuardarProductoCalculado(string nombre, decimal precio, List<TempReceta> ingredientes)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insertamos el Producto con el precio que acabamos de calcular
                        string sqlProducto = "INSERT INTO Productos (Nombre, PrecioVenta, estado) OUTPUT INSERTED.Id VALUES (@Nombre, @Precio, 1)";
                        int idProducto;
                        using (var cmd = new SqlCommand(sqlProducto, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Nombre", nombre);
                            cmd.Parameters.AddWithValue("@Precio", precio);
                            idProducto = (int)await cmd.ExecuteScalarAsync();
                        }

                        foreach (var ing in ingredientes)
                        {
                            string sqlReceta = "INSERT INTO Recetas (Id_Producto, Id_Insumo, CantidadRequerida) VALUES (@IdProducto, @IdInsumo, @Cantidad)";
                            using (var cmd = new SqlCommand(sqlReceta, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                                cmd.Parameters.AddWithValue("@IdInsumo", ing.IdInsumo);
                                cmd.Parameters.AddWithValue("@Cantidad", ing.Cantidad);
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch { await transaction.RollbackAsync(); throw; }
                }
            }
        }
        public async Task<List<Receta>> ObtenerIngredientesPorProducto(int idProducto)
        {
            var lista = new List<Receta>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Esta es la consulta que une ambas tablas
                string sql = @"
            SELECT i.Id, i.Nombre, r.CantidadRequerida, i.UnidadMedida
            FROM Recetas r
            INNER JOIN Insumos i ON r.Id_Insumo = i.Id
            WHERE r.Id_Producto = @IdProducto";

                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@IdProducto", idProducto);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Receta
                        {
                            Id = (int)reader["Id"],
                            Nombre = reader["Nombre"].ToString(),
                            Cantidad = Convert.ToSingle(reader["CantidadRequerida"]),
                            Unidad = reader["UnidadMedida"].ToString()
                        });
                    }
                }
            }
            return lista;
        }


    }
}
