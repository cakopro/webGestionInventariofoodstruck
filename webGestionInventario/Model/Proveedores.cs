namespace webGestionInventario.Model
{
    public class Proveedores
    {
        int id;
        string nombre;
        string telefono; 
        string rut;
        string correo;
        string empresa;
        string direccion;

        bool estado;

        public Proveedores(int id, string nombre, string telefono, string rut, string correo, string empresa, string direccion,  bool estado)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Telefono = telefono;
            this.Rut = rut;
            this.Correo = correo;
            this.Empresa = empresa;
            this.Direccion = direccion;
            this.Estado = estado;
        }

        public Proveedores() { }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Empresa { get => empresa; set => empresa = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Rut { get => rut; set => rut = value; }
        public bool Estado { get => estado; set => estado = value; }
    }
}
