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
        private Button btnDashboard;
        private Button btnExportar;
        private Button btnEmpleados;
        private Button btnVentas;
        private Button btnMantenimientos;
        private GroupBox grpRegistro;
        private GroupBox grpBusqueda;
        private GroupBox grpResultados;
        private DataGridView dgvVehiculos;

        // Labels de validación
        private Label lblErrorTipo;
        private Label lblErrorMarca;
        private Label lblErrorModelo;
        private Label lblErrorAño;
        private Label lblErrorPrecio;
        private Label lblErrorCaracteristica;

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
            dgvVehiculos.Columns.Add("Estado", "Estado");

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
                    valorActual.ToString("F2"), vehiculo.Estado
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
            LimpiarErrores();
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
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new Font("Segoe UI", 10);

            Size buttonSize = new Size(130, 40);

            // ── Grupo Registro ───────────────────────────────────
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

            lblErrorTipo = CrearLabelError(new Point(15, 58));
            lblErrorMarca = CrearLabelError(new Point(15, 93));
            lblErrorModelo = CrearLabelError(new Point(15, 128));
            lblErrorAño = CrearLabelError(new Point(15, 163));
            lblErrorPrecio = CrearLabelError(new Point(15, 198));
            lblErrorCaracteristica = CrearLabelError(new Point(15, 268));

            grpRegistro.Controls.Add(comboTipo);
            grpRegistro.Controls.Add(txtMarca);
            grpRegistro.Controls.Add(txtModelo);
            grpRegistro.Controls.Add(txtAño);
            grpRegistro.Controls.Add(txtPrecio);
            grpRegistro.Controls.Add(txtColor);
            grpRegistro.Controls.Add(txtCaracteristica);
            grpRegistro.Controls.Add(btnRegistrar);
            grpRegistro.Controls.Add(btnMostrar);
            grpRegistro.Controls.Add(lblErrorTipo);
            grpRegistro.Controls.Add(lblErrorMarca);
            grpRegistro.Controls.Add(lblErrorModelo);
            grpRegistro.Controls.Add(lblErrorAño);
            grpRegistro.Controls.Add(lblErrorPrecio);
            grpRegistro.Controls.Add(lblErrorCaracteristica);

            // ── Grupo Búsqueda ───────────────────────────────────
            grpBusqueda = new GroupBox();
            grpBusqueda.Text = "Búsqueda y Eliminación";
            grpBusqueda.Location = new Point(450, 20);
            grpBusqueda.Size = new Size(420, 120);

            txtBuscarMarca = new TextBox();
            txtBuscarMarca.Location = new Point(20, 40);
            txtBuscarMarca.Width = 360;
            txtBuscarMarca.PlaceholderText = "Buscar por Marca";

            btnBuscar = CrearBoton("Buscar", new Point(20, 75), new Size(120, 35), System.Drawing.Color.FromArgb(52, 152, 219));
            btnEliminar = CrearBoton("Eliminar", new Point(155, 75), new Size(120, 35), System.Drawing.Color.FromArgb(192, 57, 43));

            grpBusqueda.Controls.Add(txtBuscarMarca);
            grpBusqueda.Controls.Add(btnBuscar);
            grpBusqueda.Controls.Add(btnEliminar);

            // ── Botones módulos ──────────────────────────────────
            btnDashboard = CrearBoton("📊 Dashboard", new Point(450, 155),
                new Size(130, 38), System.Drawing.Color.FromArgb(142, 68, 173));

            btnExportar = CrearBoton("📥 Excel", new Point(590, 155),
                new Size(130, 38), System.Drawing.Color.FromArgb(39, 174, 96));

            btnEmpleados = CrearBoton("👥 Empleados", new Point(730, 155),
                new Size(140, 38), System.Drawing.Color.FromArgb(41, 128, 185));

            btnVentas = CrearBoton("💰 Ventas", new Point(450, 203),
                new Size(130, 38), System.Drawing.Color.FromArgb(243, 156, 18));

            btnMantenimientos = CrearBoton("🔧 Mantenimientos", new Point(590, 203),
                new Size(280, 38), System.Drawing.Color.FromArgb(22, 160, 133));

            // ── Grupo Resultados ─────────────────────────────────
            grpResultados = new GroupBox();
            grpResultados.Text = "Listado de Vehículos";
            grpResultados.Location = new Point(20, 370);
            grpResultados.Size = new Size(850, 230);

            dgvVehiculos = new DataGridView();
            dgvVehiculos.Dock = DockStyle.Fill;
            dgvVehiculos.ReadOnly = true;
            dgvVehiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVehiculos.BackgroundColor = System.Drawing.Color.White;
            dgvVehiculos.AllowUserToAddRows = false;
            dgvVehiculos.RowHeadersVisible = false;
            dgvVehiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVehiculos.BorderStyle = BorderStyle.None;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dgvVehiculos.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvVehiculos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvVehiculos.EnableHeadersVisualStyles = false;
            dgvVehiculos.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            grpResultados.Controls.Add(dgvVehiculos);

            this.Controls.Add(grpRegistro);
            this.Controls.Add(grpBusqueda);
            this.Controls.Add(btnDashboard);
            this.Controls.Add(btnExportar);
            this.Controls.Add(btnEmpleados);
            this.Controls.Add(btnVentas);
            this.Controls.Add(btnMantenimientos);
            this.Controls.Add(grpResultados);
        }

        private Label CrearLabelError(Point location)
        {
            return new Label()
            {
                Location = location,
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(192, 57, 43),
                Font = new Font("Segoe UI", 7.5f, FontStyle.Italic),
                Text = ""
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
            return btn;
        }

        private void LimpiarErrores()
        {
            lblErrorTipo.Text = "";
            lblErrorMarca.Text = "";
            lblErrorModelo.Text = "";
            lblErrorAño.Text = "";
            lblErrorPrecio.Text = "";
            lblErrorCaracteristica.Text = "";

            comboTipo.BackColor = System.Drawing.Color.White;
            txtMarca.BackColor = System.Drawing.Color.White;
            txtModelo.BackColor = System.Drawing.Color.White;
            txtAño.BackColor = System.Drawing.Color.White;
            txtPrecio.BackColor = System.Drawing.Color.White;
            txtCaracteristica.BackColor = System.Drawing.Color.White;
        }

        private bool ValidarFormulario()
        {
            LimpiarErrores();
            bool valido = true;

            if (comboTipo.SelectedItem == null)
            {
                lblErrorTipo.Text = "⚠ Seleccione un tipo de vehículo";
                comboTipo.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtMarca.Text))
            {
                lblErrorMarca.Text = "⚠ La marca es obligatoria";
                txtMarca.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtModelo.Text))
            {
                lblErrorModelo.Text = "⚠ El modelo es obligatorio";
                txtModelo.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                valido = false;
            }
            if (!int.TryParse(txtAño.Text, out int año) || año < 1900 || año > DateTime.Now.Year)
            {
                lblErrorAño.Text = $"⚠ Año válido entre 1900 y {DateTime.Now.Year}";
                txtAño.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                valido = false;
            }
            if (!double.TryParse(txtPrecio.Text, out double precio) || precio <= 0)
            {
                lblErrorPrecio.Text = "⚠ Precio debe ser mayor a 0";
                txtPrecio.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                valido = false;
            }
            if (comboTipo.SelectedItem != null)
            {
                string tipo = comboTipo.SelectedItem.ToString()!;
                if (tipo == "Camion" && !double.TryParse(txtCaracteristica.Text, out _))
                {
                    lblErrorCaracteristica.Text = "⚠ Peso debe ser un número válido";
                    txtCaracteristica.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                    valido = false;
                }
                if (tipo == "Automovil" && !int.TryParse(txtCaracteristica.Text, out _))
                {
                    lblErrorCaracteristica.Text = "⚠ Puertas debe ser un número entero";
                    txtCaracteristica.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                    valido = false;
                }
                if (tipo == "Motocicleta" && string.IsNullOrWhiteSpace(txtCaracteristica.Text))
                {
                    lblErrorCaracteristica.Text = "⚠ El tipo de moto es obligatorio";
                    txtCaracteristica.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                    valido = false;
                }
            }
            return valido;
        }

        private void WireEvents()
        {
            btnRegistrar.Click += (s, e) =>
            {
                if (ValidarFormulario())
                    _presenter.RegistrarVehiculo();
            };
            btnMostrar.Click += (s, e) => _presenter.CargarVehiculos();
            btnBuscar.Click += (s, e) => _presenter.BuscarVehiculo();
            btnEliminar.Click += (s, e) => _presenter.EliminarVehiculo();
            btnDashboard.Click += (s, e) =>
            {
                var dashboard = new FormDashboard(_servicio.ObtenerTodos(), _servicio.GetConnectionString()); dashboard.ShowDialog();
            };
            btnExportar.Click += (s, e) =>
            {
                try
                {
                    var exportService = new ExportService();
                    string ruta = exportService.ExportarAExcel(_servicio.ObtenerTodos());
                    MostrarMensaje($"Archivo exportado en:\n{ruta}", "Exportación exitosa");
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error al exportar: " + ex.Message, "Error", true);
                }
            };
            btnEmpleados.Click += (s, e) =>
            {
                var form = new FormEmpleados(_servicio.GetConnectionString());
                form.ShowDialog();
            };
            btnVentas.Click += (s, e) =>
            {
                var form = new FormVentas(_servicio.GetConnectionString());
                form.ShowDialog();
                _presenter.CargarVehiculos();
            };
            btnMantenimientos.Click += (s, e) =>
            {
                var form = new FormMantenimientos(_servicio.GetConnectionString());
                form.ShowDialog();
                _presenter.CargarVehiculos();
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