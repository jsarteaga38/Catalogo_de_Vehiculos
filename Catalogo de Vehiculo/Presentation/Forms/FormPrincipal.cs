using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Application.Services;
using Catalogo_de_Vehiculo.Infrastructure.Repositories;

namespace Catalogo_de_Vehiculo.Presentation.Forms
{
    public partial class FormPrincipal : Form
    {
        private ServicioVehiculo servicio;
        private VehiculoRepository repositorio;

        private ComboBox comboTipo;
        private TextBox txtMarca;
        private TextBox txtModelo;
        private TextBox txtAño;
        private TextBox txtPrecio;
        private TextBox txtColor;
        private TextBox txtCaracteristica;
        private TextBox txtBuscarMarca;
        private Button btnRegistrar;
        private Button btnMostrar;
        private Button btnBuscar;
        private Button btnEliminar;
        private GroupBox grpRegistro;
        private GroupBox grpBusqueda;
        private GroupBox grpResultados;
        private DataGridView dgvVehiculos;

        public FormPrincipal()
        {
            servicio = new ServicioVehiculo();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CatalogoVehiculos;Integrated Security=True;Trust Server Certificate=True";
            repositorio = new VehiculoRepository(connectionString);

            InicializarUI();
            WireEvents();
            UpdateUIState();
        }

        private void InicializarUI()
        {
            this.Text = "Sistema Empresarial - Catálogo de Vehículos";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 242, 245);
            this.Font = new Font("Segoe UI", 10);

            Size buttonSize = new Size(130, 40);

            grpRegistro = new GroupBox();
            grpRegistro.Text = "Registro de Vehículo";
            grpRegistro.Location = new Point(20, 20);
            grpRegistro.Size = new Size(400, 340);

            comboTipo = new ComboBox();
            comboTipo.Location = new Point(20, 40);
            comboTipo.Width = 340;
            comboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTipo.Items.AddRange(new string[] { "Camion", "Automovil", "Motocicleta" });

            txtMarca = new TextBox() { Location = new Point(20, 80), Width = 340, PlaceholderText = "Marca" };
            txtModelo = new TextBox() { Location = new Point(20, 115), Width = 340, PlaceholderText = "Modelo" };
            txtAño = new TextBox() { Location = new Point(20, 150), Width = 340, PlaceholderText = "Año" };
            txtPrecio = new TextBox() { Location = new Point(20, 185), Width = 340, PlaceholderText = "Precio" };
            txtColor = new TextBox() { Location = new Point(20, 220), Width = 340, PlaceholderText = "Color" };
            txtCaracteristica = new TextBox() { Location = new Point(20, 255), Width = 340, Visible = false };

            btnRegistrar = CrearBoton("Registrar", new Point(20, 290), buttonSize, Color.FromArgb(46, 134, 193));
            btnMostrar = CrearBoton("Mostrar", new Point(170, 290), buttonSize, Color.FromArgb(39, 174, 96));

            grpRegistro.Controls.Add(comboTipo);
            grpRegistro.Controls.Add(txtMarca);
            grpRegistro.Controls.Add(txtModelo);
            grpRegistro.Controls.Add(txtAño);
            grpRegistro.Controls.Add(txtPrecio);
            grpRegistro.Controls.Add(txtColor);
            grpRegistro.Controls.Add(txtCaracteristica);
            grpRegistro.Controls.Add(btnRegistrar);
            grpRegistro.Controls.Add(btnMostrar);

            grpBusqueda = new GroupBox();
            grpBusqueda.Text = "Búsqueda y Eliminación";
            grpBusqueda.Location = new Point(450, 20);
            grpBusqueda.Size = new Size(400, 200);

            txtBuscarMarca = new TextBox();
            txtBuscarMarca.Location = new Point(20, 40);
            txtBuscarMarca.Width = 340;
            txtBuscarMarca.PlaceholderText = "Buscar por Marca";

            btnBuscar = CrearBoton("Buscar", new Point(20, 90), buttonSize, Color.FromArgb(52, 152, 219));
            btnEliminar = CrearBoton("Eliminar", new Point(170, 90), buttonSize, Color.FromArgb(192, 57, 43));

            grpBusqueda.Controls.Add(txtBuscarMarca);
            grpBusqueda.Controls.Add(btnBuscar);
            grpBusqueda.Controls.Add(btnEliminar);

            grpResultados = new GroupBox();
            grpResultados.Text = "Listado de Vehículos";
            grpResultados.Location = new Point(20, 380);
            grpResultados.Size = new Size(830, 180);

            dgvVehiculos = new DataGridView();
            dgvVehiculos.Dock = DockStyle.Fill;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVehiculos.BackgroundColor = Color.White;
            dgvVehiculos.AllowUserToAddRows = false;

            grpResultados.Controls.Add(dgvVehiculos);

            this.Controls.Add(grpRegistro);
            this.Controls.Add(grpBusqueda);
            this.Controls.Add(grpResultados);
        }

        private Button CrearBoton(string texto, Point location, Size size, Color color)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Location = location;
            btn.Size = size;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void WireEvents()
        {
            btnRegistrar.Click += BtnRegistrar_Click;
            btnMostrar.Click += BtnMostrar_Click;
            btnBuscar.Click += BtnBuscar_Click;
            btnEliminar.Click += BtnEliminar_Click;

            comboTipo.SelectedIndexChanged += ComboTipo_SelectedIndexChanged;
            txtMarca.TextChanged += (s, e) => UpdateUIState();
        }

        private void UpdateUIState()
        {
            btnRegistrar.Enabled =
                comboTipo.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(txtMarca.Text);
        }

        private void ComboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTipo.SelectedItem == null)
            {
                txtCaracteristica.Visible = false;
                UpdateUIState();
                return;
            }

            string tipo = comboTipo.SelectedItem.ToString();
            txtCaracteristica.Visible = true;

            switch (tipo)
            {
                case "Camion":
                    txtCaracteristica.PlaceholderText = "Peso de carga (Ton)";
                    break;
                case "Automovil":
                    txtCaracteristica.PlaceholderText = "Cantidad de puertas";
                    break;
                case "Motocicleta":
                    txtCaracteristica.PlaceholderText = "Tipo de motocicleta";
                    break;
            }

            UpdateUIState();
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                Vehiculo vehiculo = CrearVehiculo();
                if (vehiculo == null) return;

                repositorio.Agregar(vehiculo);  // ← guarda en base de datos
                MostrarEnGrid();
                LimpiarCampos();

                MessageBox.Show("Vehículo registrado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMostrar_Click(object sender, EventArgs e)
        {
            MostrarEnGrid();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            Vehiculo encontrado = repositorio.BuscarPorMarca(txtBuscarMarca.Text);  // ← busca en base de datos

            if (encontrado != null)
            {
                List<Vehiculo> lista = new List<Vehiculo> { encontrado };
                dgvVehiculos.Rows.Clear();
                MostrarLista(lista);
            }
            else
            {
                MessageBox.Show("No se encontró ningún vehículo con esa marca.", "Sin resultados",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            Vehiculo encontrado = repositorio.BuscarPorMarca(txtBuscarMarca.Text);  // ← busca en base de datos

            if (encontrado != null)
            {
                DialogResult confirm = MessageBox.Show(
                    $"¿Desea eliminar el vehículo {encontrado.Marca} {encontrado.Modelo}?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    repositorio.Eliminar(encontrado);  // ← elimina de base de datos
                    MostrarEnGrid();
                    MessageBox.Show("Vehículo eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No se encontró ningún vehículo con esa marca.", "Sin resultados",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private Vehiculo CrearVehiculo()
        {
            if (comboTipo.SelectedItem == null)
                return null;

            if (!int.TryParse(txtAño.Text, out int año))
            {
                MessageBox.Show("El año debe ser un número entero válido.", "Dato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            if (!double.TryParse(txtPrecio.Text, out double precio))
            {
                MessageBox.Show("El precio debe ser un número válido.", "Dato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            string tipo = comboTipo.SelectedItem.ToString();

            if (tipo == "Camion")
            {
                if (!double.TryParse(txtCaracteristica.Text, out double peso))
                {
                    MessageBox.Show("El peso debe ser un número válido.", "Dato inválido",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                return new Camion(txtMarca.Text, txtModelo.Text, año, precio, txtColor.Text, peso);
            }

            if (tipo == "Automovil")
            {
                if (!int.TryParse(txtCaracteristica.Text, out int puertas))
                {
                    MessageBox.Show("El número de puertas debe ser un entero válido.", "Dato inválido",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                return new Automovil(txtMarca.Text, txtModelo.Text, año, precio, txtColor.Text, puertas);
            }

            if (tipo == "Motocicleta")
            {
                return new Motocicleta(
                    txtMarca.Text, txtModelo.Text, año, precio,
                    txtColor.Text, txtCaracteristica.Text);
            }

            return null;
        }

        private void MostrarEnGrid()
        {
            List<Vehiculo> lista = repositorio.ObtenerTodos();  // ← trae de base de datos
            MostrarLista(lista);
        }

        private void MostrarLista(List<Vehiculo> lista)
        {
            dgvVehiculos.Columns.Clear();
            dgvVehiculos.Rows.Clear();

            dgvVehiculos.Columns.Add("Marca", "Marca");
            dgvVehiculos.Columns.Add("Modelo", "Modelo");
            dgvVehiculos.Columns.Add("Año", "Año");
            dgvVehiculos.Columns.Add("Precio", "Precio ($)");
            dgvVehiculos.Columns.Add("Color", "Color");
            dgvVehiculos.Columns.Add("Caracteristica", "Característica");
            dgvVehiculos.Columns.Add("Depreciacion", "Depreciación ($)");
            dgvVehiculos.Columns.Add("ValorActual", "Valor Actual ($)");

            foreach (Vehiculo vehiculo in lista)
            {
                double depreciacion = vehiculo.CalcularDepreciacion();
                double valorActual = Math.Max(0, vehiculo.Precio - depreciacion);

                string caracteristica = "";
                if (vehiculo is Camion camion)
                    caracteristica = camion.peso + " Ton";
                else if (vehiculo is Automovil auto)
                    caracteristica = auto.cantidadPlasas + " Puertas";
                else if (vehiculo is Motocicleta moto)
                    caracteristica = moto.tipoMotosicleta;

                dgvVehiculos.Rows.Add(
                    vehiculo.Marca,
                    vehiculo.Modelo,
                    vehiculo.Año,
                    vehiculo.Precio.ToString("F2"),
                    vehiculo.Color,
                    caracteristica,
                    depreciacion.ToString("F2"),
                    valorActual.ToString("F2")
                );
            }
        }

        private void LimpiarCampos()
        {
            txtMarca.Clear();
            txtModelo.Clear();
            txtAño.Clear();
            txtPrecio.Clear();
            txtColor.Clear();
            txtCaracteristica.Clear();
            comboTipo.SelectedIndex = -1;
            txtCaracteristica.Visible = false;
        }
    }
}