using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        // Registro
        private ComboBox comboTipo;
        private TextBox txtMarca;
        private TextBox txtModelo;
        private TextBox txtAño;
        private TextBox txtPrecio;
        private TextBox txtColor;
        private TextBox txtCaracteristica;
        private Button btnRegistrar;
        private Button btnLimpiar;
        private GroupBox grpRegistro;

        // Filtros
        private ComboBox comboFiltroTipo;
        private TextBox txtFiltroMarca;
        private TextBox txtFiltroAño;
        private Button btnFiltrar;
        private Button btnMostrarTodos;
        private GroupBox grpFiltros;

        // Búsqueda
        private TextBox txtBuscarMarca;
        private Button btnBuscar;
        private Button btnEliminar;
        private GroupBox grpBusqueda;

        // Resultados
        private DataGridView dgvVehiculos;
        private GroupBox grpResultados;

        // Barra de estado
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblTotal;
        private ToolStripStatusLabel lblValorFlota;
        private ToolStripStatusLabel lblDepreciacion;

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
            dgvVehiculos.Rows.Clear();
            foreach (Vehiculo vehiculo in lista)
            {
                double depreciacion = vehiculo.CalcularDepreciacion();
                double valorActual = Math.Max(0, vehiculo.Precio - depreciacion);
                string tipo = vehiculo.GetType().Name;

                string caracteristica = "";
                if (vehiculo is Camion camion)
                    caracteristica = camion.peso + " Ton";
                else if (vehiculo is Automovil auto)
                    caracteristica = auto.cantidadPlasas + " Puertas";
                else if (vehiculo is Motocicleta moto)
                    caracteristica = moto.tipoMotosicleta;

                dgvVehiculos.Rows.Add(
                    tipo, vehiculo.Marca, vehiculo.Modelo, vehiculo.Año,
                    vehiculo.Precio.ToString("C"), vehiculo.Color,
                    caracteristica, depreciacion.ToString("C"),
                    valorActual.ToString("C")
                );
            }
            ActualizarEstadisticas(lista);
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

        // ── Estadísticas ─────────────────────────────────────────
        private void ActualizarEstadisticas(List<Vehiculo> lista)
        {
            double valorFlota = lista.Sum(v => Math.Max(0, v.Precio - v.CalcularDepreciacion()));
            double depreciacionTotal = lista.Sum(v => v.CalcularDepreciacion());

            lblTotal.Text = $"Total vehículos: {lista.Count}";
            lblValorFlota.Text = $"Valor flota: {valorFlota:C}";
            lblDepreciacion.Text = $"Depreciación total: {depreciacionTotal:C}";
        }

        // ── Filtros ──────────────────────────────────────────────
        private void AplicarFiltros()
        {
            List<Vehiculo> todos = _servicio.ObtenerTodos();
            List<Vehiculo> filtrados = todos;

            if (comboFiltroTipo.SelectedItem != null && comboFiltroTipo.SelectedIndex > 0)
                filtrados = filtrados.Where(v => v.GetType().Name == comboFiltroTipo.SelectedItem.ToString()).ToList();

            if (!string.IsNullOrWhiteSpace(txtFiltroMarca.Text))
                filtrados = filtrados.Where(v => v.Marca.ToLower().Contains(txtFiltroMarca.Text.ToLower())).ToList();

            if (int.TryParse(txtFiltroAño.Text, out int año))
                filtrados = filtrados.Where(v => v.Año == año).ToList();

            MostrarVehiculos(filtrados);
        }

        // ── UI ───────────────────────────────────────────────────
        private void InicializarUI()
        {
            this.Text = "Sistema Empresarial — Catálogo de Vehículos";
            this.Size = new Size(1100, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9);
            this.MinimumSize = new Size(1000, 680);

            // ── Barra de estado ──────────────────────────────────
            statusStrip = new StatusStrip();
            statusStrip.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            statusStrip.ForeColor = System.Drawing.Color.White;

            lblTotal = new ToolStripStatusLabel("Total vehículos: 0");
            lblTotal.ForeColor = System.Drawing.Color.White;

            lblValorFlota = new ToolStripStatusLabel("Valor flota: $0");
            lblValorFlota.ForeColor = System.Drawing.Color.LightGreen;

            lblDepreciacion = new ToolStripStatusLabel("Depreciación total: $0");
            lblDepreciacion.ForeColor = System.Drawing.Color.LightSalmon;

            statusStrip.Items.Add(lblTotal);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(lblValorFlota);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(lblDepreciacion);
            this.Controls.Add(statusStrip);

            Size btnSize = new Size(120, 35);

            // ── Panel Registro ───────────────────────────────────
            grpRegistro = new GroupBox();
            grpRegistro.Text = "➕ Registro de Vehículo";
            grpRegistro.Location = new Point(15, 15);
            grpRegistro.Size = new Size(330, 360);
            grpRegistro.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpRegistro.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            comboTipo = new ComboBox() { Location = new Point(15, 30), Width = 295, DropDownStyle = ComboBoxStyle.DropDownList };
            comboTipo.Items.AddRange(new string[] { "Camion", "Automovil", "Motocicleta" });
            comboTipo.Font = new Font("Segoe UI", 9);

            txtMarca = CrearTextBox(new Point(15, 70), "Marca *");
            txtModelo = CrearTextBox(new Point(15, 105), "Modelo *");
            txtAño = CrearTextBox(new Point(15, 140), "Año *");
            txtPrecio = CrearTextBox(new Point(15, 175), "Precio *");
            txtColor = CrearTextBox(new Point(15, 210), "Color");
            txtCaracteristica = CrearTextBox(new Point(15, 245), "Característica *");
            txtCaracteristica.Visible = false;

            btnRegistrar = CrearBoton("✔ Registrar", new Point(15, 305), btnSize, System.Drawing.Color.FromArgb(39, 174, 96));
            btnLimpiar = CrearBoton("✖ Limpiar", new Point(150, 305), btnSize, System.Drawing.Color.FromArgb(149, 165, 166));

            grpRegistro.Controls.AddRange(new Control[] {
                comboTipo, txtMarca, txtModelo, txtAño, txtPrecio,
                txtColor, txtCaracteristica, btnRegistrar, btnLimpiar
            });

            // ── Panel Filtros ────────────────────────────────────
            grpFiltros = new GroupBox();
            grpFiltros.Text = "🔍 Filtros";
            grpFiltros.Location = new Point(360, 15);
            grpFiltros.Size = new Size(350, 150);
            grpFiltros.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpFiltros.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            comboFiltroTipo = new ComboBox() { Location = new Point(15, 30), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            comboFiltroTipo.Items.AddRange(new string[] { "Todos", "Camion", "Automovil", "Motocicleta" });
            comboFiltroTipo.SelectedIndex = 0;
            comboFiltroTipo.Font = new Font("Segoe UI", 9);

            txtFiltroMarca = CrearTextBox(new Point(175, 30), "Marca");
            txtFiltroMarca.Width = 155;
            txtFiltroAño = CrearTextBox(new Point(15, 75), "Año exacto");
            txtFiltroAño.Width = 100;

            btnFiltrar = CrearBoton("🔍 Filtrar", new Point(15, 108), new Size(100, 30), System.Drawing.Color.FromArgb(52, 152, 219));
            btnMostrarTodos = CrearBoton("↺ Todos", new Point(125, 108), new Size(100, 30), System.Drawing.Color.FromArgb(127, 140, 141));

            grpFiltros.Controls.AddRange(new Control[] {
                comboFiltroTipo, txtFiltroMarca, txtFiltroAño, btnFiltrar, btnMostrarTodos
            });

            // ── Panel Búsqueda ───────────────────────────────────
            grpBusqueda = new GroupBox();
            grpBusqueda.Text = "🔎 Búsqueda y Eliminación";
            grpBusqueda.Location = new Point(360, 175);
            grpBusqueda.Size = new Size(350, 100);
            grpBusqueda.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpBusqueda.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            txtBuscarMarca = CrearTextBox(new Point(15, 30), "Buscar por marca exacta");
            txtBuscarMarca.Width = 295;

            btnBuscar = CrearBoton("🔎 Buscar", new Point(15, 60), new Size(100, 30), System.Drawing.Color.FromArgb(52, 152, 219));
            btnEliminar = CrearBoton("🗑 Eliminar", new Point(125, 60), new Size(110, 30), System.Drawing.Color.FromArgb(192, 57, 43));

            grpBusqueda.Controls.AddRange(new Control[] { txtBuscarMarca, btnBuscar, btnEliminar });

            // ── Panel Resultados ─────────────────────────────────
            grpResultados = new GroupBox();
            grpResultados.Text = "📋 Listado de Vehículos";
            grpResultados.Location = new Point(15, 390);
            grpResultados.Size = new Size(1055, 270);
            grpResultados.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpResultados.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            grpResultados.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            dgvVehiculos = new DataGridView();
            dgvVehiculos.Dock = DockStyle.Fill;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVehiculos.BackgroundColor = System.Drawing.Color.White;
            dgvVehiculos.AllowUserToAddRows = false;
            dgvVehiculos.BorderStyle = BorderStyle.None;
            dgvVehiculos.RowHeadersVisible = false;
            dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculos.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            // Encabezados del grid
            dgvVehiculos.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dgvVehiculos.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvVehiculos.EnableHeadersVisualStyles = false;

            dgvVehiculos.Columns.Add("Tipo", "Tipo");
            dgvVehiculos.Columns.Add("Marca", "Marca");
            dgvVehiculos.Columns.Add("Modelo", "Modelo");
            dgvVehiculos.Columns.Add("Año", "Año");
            dgvVehiculos.Columns.Add("Precio", "Precio");
            dgvVehiculos.Columns.Add("Color", "Color");
            dgvVehiculos.Columns.Add("Caracteristica", "Característica");
            dgvVehiculos.Columns.Add("Depreciacion", "Depreciación");
            dgvVehiculos.Columns.Add("ValorActual", "Valor Actual");

            grpResultados.Controls.Add(dgvVehiculos);

            this.Controls.Add(grpRegistro);
            this.Controls.Add(grpFiltros);
            this.Controls.Add(grpBusqueda);
            this.Controls.Add(grpResultados);
        }

        private TextBox CrearTextBox(Point location, string placeholder)
        {
            return new TextBox()
            {
                Location = location,
                Width = 295,
                PlaceholderText = placeholder,
                Font = new Font("Segoe UI", 9)
            };
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
            btn.Font = new Font("Segoe UI", 9);
            return btn;
        }

        private void WireEvents()
        {
            btnRegistrar.Click += (s, e) => _presenter.RegistrarVehiculo();
            btnLimpiar.Click += (s, e) => LimpiarFormulario();
            btnBuscar.Click += (s, e) => _presenter.BuscarVehiculo();
            btnEliminar.Click += (s, e) => _presenter.EliminarVehiculo();
            btnFiltrar.Click += (s, e) => AplicarFiltros();
            btnMostrarTodos.Click += (s, e) => _presenter.CargarVehiculos();
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