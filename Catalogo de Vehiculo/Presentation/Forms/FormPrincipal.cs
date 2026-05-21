using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Catalogo_de_Vehiculo.Application.Services;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;
using Catalogo_de_Vehiculo.Infrastructure.Repositories;
using Catalogo_de_Vehiculo.Presentation.Presenters;

namespace Catalogo_de_Vehiculo.Presentation.Forms
{
    public partial class FormPrincipal : Form, ICatalogoView, ICatalogoObserver
    {
        private readonly CatalogoPresenter _presenter;
        private readonly CatalogoService _servicio;

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
        private Button btnDashboard;        // ← NUEVO
        private GroupBox grpRegistro;
        private GroupBox grpBusqueda;
        private GroupBox grpResultados;
        private DataGridView dgvVehiculos;

        // ── ICatalogoView ────────────────────────────────────────
        public string TipoSeleccionado => comboTipo.SelectedItem?.ToString() ?? "";
        public string Marca => txtMarca.Text;
        public string Modelo => txtModelo.Text;
        public string Anio => txtAño.Text;
        public string Precio => txtPrecio.Text;
        public string Color => txtColor.Text;
        public string Caracteristica => txtCaracteristica.Text;
        public string MarcaBusqueda => txtBuscarMarca.Text;

        public void MostrarVehiculos(List<Vehiculo> lista)
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
                    vehiculo.Marca, vehiculo.Modelo, vehiculo.Año,
                    vehiculo.Precio.ToString("F2"), vehiculo.Color,
                    caracteristica, depreciacion.ToString("F2"),
                    valorActual.ToString("F2")
                );
            }
        }

        public void MostrarMensaje(string mensaje, string titulo, bool esError = false)
        {
            MessageBoxIcon icono = esError ? MessageBoxIcon.Warning : MessageBoxIcon.Information;
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, icono);
        }

        public void LimpiarFormulario()
        {
            txtMarca.Clear(); txtModelo.Clear(); txtAño.Clear();
            txtPrecio.Clear(); txtColor.Clear(); txtCaracteristica.Clear();
            comboTipo.SelectedIndex = -1;
            txtCaracteristica.Visible = false;
        }

        // ── ICatalogoObserver ────────────────────────────────────
        public void OnCatalogoActualizado(string mensaje)
        {
            _presenter.CargarVehiculos();
        }

        // ── Constructor ──────────────────────────────────────────
        public FormPrincipal()
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CatalogoVehiculos;Integrated Security=True;Trust Server Certificate=True";
            var repositorio = new VehiculoRepository(connectionString);
            _servicio = new CatalogoService(repositorio);
            _servicio.Suscribir(this);
            _presenter = new CatalogoPresenter(this, _servicio);

            InicializarUI();
            WireEvents();
            UpdateUIState();
        }

        // ── UI ───────────────────────────────────────────────────
        private void InicializarUI()
        {
            this.Text = "Sistema Empresarial - Catálogo de Vehículos";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
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

            btnRegistrar = CrearBoton("Registrar", new Point(20, 290), buttonSize, System.Drawing.Color.FromArgb(46, 134, 193));
            btnMostrar = CrearBoton("Mostrar", new Point(170, 290), buttonSize, System.Drawing.Color.FromArgb(39, 174, 96));

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

            btnBuscar = CrearBoton("Buscar", new Point(20, 90), buttonSize, System.Drawing.Color.FromArgb(52, 152, 219));
            btnEliminar = CrearBoton("Eliminar", new Point(170, 90), buttonSize, System.Drawing.Color.FromArgb(192, 57, 43));

            grpBusqueda.Controls.Add(txtBuscarMarca);
            grpBusqueda.Controls.Add(btnBuscar);
            grpBusqueda.Controls.Add(btnEliminar);

            // ── Botón Dashboard ──────────────────────────────────
            btnDashboard = CrearBoton("📊 Dashboard", new Point(450, 240),
                new Size(180, 40), System.Drawing.Color.FromArgb(142, 68, 173));

            grpResultados = new GroupBox();
            grpResultados.Text = "Listado de Vehículos";
            grpResultados.Location = new Point(20, 380);
            grpResultados.Size = new Size(830, 180);

            dgvVehiculos = new DataGridView();
            dgvVehiculos.Dock = DockStyle.Fill;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVehiculos.BackgroundColor = System.Drawing.Color.White;
            dgvVehiculos.AllowUserToAddRows = false;

            grpResultados.Controls.Add(dgvVehiculos);

            this.Controls.Add(grpRegistro);
            this.Controls.Add(grpBusqueda);
            this.Controls.Add(btnDashboard);    // ← NUEVO
            this.Controls.Add(grpResultados);
        }

        private Button CrearBoton(string texto, Point location, Size size, System.Drawing.Color colorBoton)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Location = location;
            btn.Size = size;
            btn.BackColor = colorBoton;
            btn.ForeColor = System.Drawing.Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void WireEvents()
        {
            btnRegistrar.Click += (s, e) => _presenter.RegistrarVehiculo();
            btnMostrar.Click += (s, e) => _presenter.CargarVehiculos();
            btnBuscar.Click += (s, e) => _presenter.BuscarVehiculo();
            btnEliminar.Click += (s, e) => _presenter.EliminarVehiculo();
            btnDashboard.Click += (s, e) =>     // ← NUEVO
            {
                var dashboard = new FormDashboard(_servicio.ObtenerTodos());
                dashboard.ShowDialog();
            };
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

            string tipo = comboTipo.SelectedItem.ToString()!;
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
    }
}