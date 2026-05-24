using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Infrastructure.Repositories;

namespace Catalogo_de_Vehiculo.Presentation.Forms
{
    public class FormVentas : Form
    {
        private readonly VentaRepository _repoVenta;
        private readonly VehiculoRepository _repoVehiculo;
        private readonly EmpleadoRepository _repoEmpleado;
        private readonly ClienteRepository _repoCliente;

        private ComboBox comboVehiculo;
        private ComboBox comboEmpleado;
        private TextBox txtNombreCliente;
        private TextBox txtCedulaCliente;
        private TextBox txtContactoCliente;
        private TextBox txtPrecioVenta;
        private Button btnRegistrarVenta;
        private Button btnMostrarVentas;
        private DataGridView dgvVentas;
        private GroupBox grpVenta;
        private GroupBox grpCliente;
        private GroupBox grpLista;
        private Label lblErrorVehiculo;
        private Label lblErrorEmpleado;
        private Label lblErrorCliente;
        private Label lblErrorPrecio;

        public FormVentas(string connectionString)
        {
            _repoVenta = new VentaRepository(connectionString);
            _repoVehiculo = new VehiculoRepository(connectionString);
            _repoEmpleado = new EmpleadoRepository(connectionString);
            _repoCliente = new ClienteRepository(connectionString);
            InicializarUI();
            WireEvents();
            CargarCombos();
            CargarVentas();
        }

        private void InicializarUI()
        {
            this.Text = "💰 Registro de Ventas";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9);

            // ── Panel venta ──────────────────────────────────────
            grpVenta = new GroupBox();
            grpVenta.Text = "🚗 Datos de la Venta";
            grpVenta.Location = new Point(15, 15);
            grpVenta.Size = new Size(380, 200);
            grpVenta.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpVenta.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            var lblVehiculo = new Label() { Text = "Vehículo disponible:", Location = new Point(15, 25), AutoSize = true };
            comboVehiculo = new ComboBox() { Location = new Point(15, 45), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblEmpleado = new Label() { Text = "Empleado vendedor:", Location = new Point(15, 80), AutoSize = true };
            comboEmpleado = new ComboBox() { Location = new Point(15, 100), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblPrecio = new Label() { Text = "Precio de venta:", Location = new Point(15, 135), AutoSize = true };
            txtPrecioVenta = new TextBox() { Location = new Point(15, 155), Width = 200, PlaceholderText = "Precio de venta *" };

            lblErrorVehiculo = CrearLabelError(new Point(15, 68));
            lblErrorEmpleado = CrearLabelError(new Point(15, 123));
            lblErrorPrecio = CrearLabelError(new Point(220, 158));

            grpVenta.Controls.AddRange(new Control[] {
                lblVehiculo, comboVehiculo, lblEmpleado, comboEmpleado,
                lblPrecio, txtPrecioVenta,
                lblErrorVehiculo, lblErrorEmpleado, lblErrorPrecio
            });

            // ── Panel cliente ────────────────────────────────────
            grpCliente = new GroupBox();
            grpCliente.Text = "👤 Datos del Cliente";
            grpCliente.Location = new Point(415, 15);
            grpCliente.Size = new Size(560, 200);
            grpCliente.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpCliente.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            txtNombreCliente = CrearTextBox(new Point(15, 30), "Nombre del cliente *");
            txtNombreCliente.Width = 250;
            txtCedulaCliente = CrearTextBox(new Point(280, 30), "Cédula *");
            txtCedulaCliente.Width = 260;
            txtContactoCliente = CrearTextBox(new Point(15, 80), "Contacto (teléfono o email)");
            txtContactoCliente.Width = 525;

            lblErrorCliente = CrearLabelError(new Point(15, 58));

            btnRegistrarVenta = CrearBoton("✔ Registrar Venta", new Point(15, 140), new Size(160, 40),
                System.Drawing.Color.FromArgb(39, 174, 96));
            btnMostrarVentas = CrearBoton("↺ Mostrar Todas", new Point(190, 140), new Size(150, 40),
                System.Drawing.Color.FromArgb(52, 152, 219));

            grpCliente.Controls.AddRange(new Control[] {
                txtNombreCliente, txtCedulaCliente, txtContactoCliente,
                lblErrorCliente, btnRegistrarVenta, btnMostrarVentas
            });

            // ── Panel lista ──────────────────────────────────────
            grpLista = new GroupBox();
            grpLista.Text = "📋 Historial de Ventas";
            grpLista.Location = new Point(15, 230);
            grpLista.Size = new Size(960, 370);
            grpLista.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpLista.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            grpLista.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            dgvVentas = new DataGridView();
            dgvVentas.Dock = DockStyle.Fill;
            dgvVentas.ReadOnly = true;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentas.BackgroundColor = System.Drawing.Color.White;
            dgvVentas.AllowUserToAddRows = false;
            dgvVentas.BorderStyle = BorderStyle.None;
            dgvVentas.RowHeadersVisible = false;
            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dgvVentas.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvVentas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvVentas.EnableHeadersVisualStyles = false;
            dgvVentas.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            dgvVentas.Columns.Add("Id", "ID");
            dgvVentas.Columns.Add("Vehiculo", "Vehículo");
            dgvVentas.Columns.Add("Empleado", "Empleado");
            dgvVentas.Columns.Add("Cliente", "Cliente");
            dgvVentas.Columns.Add("Precio", "Precio Venta");
            dgvVentas.Columns.Add("Fecha", "Fecha");

            grpLista.Controls.Add(dgvVentas);

            this.Controls.Add(grpVenta);
            this.Controls.Add(grpCliente);
            this.Controls.Add(grpLista);
        }

        private void WireEvents()
        {
            btnRegistrarVenta.Click += (s, e) => RegistrarVenta();
            btnMostrarVentas.Click += (s, e) => CargarVentas();
        }

        private void CargarCombos()
        {
            // Cargar vehiculos disponibles
            comboVehiculo.Items.Clear();
            var vehiculos = _repoVehiculo.ObtenerTodos();
            foreach (var v in vehiculos)
            {
                if (v.Estado == "Disponible")
                    comboVehiculo.Items.Add(new ComboItem(v.Id, $"{v.Marca} {v.Modelo} ({v.Año}) - ${v.Precio:F0}"));
            }

            // Cargar empleados
            comboEmpleado.Items.Clear();
            var empleados = _repoEmpleado.ObtenerTodos();
            foreach (var e in empleados)
                comboEmpleado.Items.Add(new ComboItem(e.Id, $"{e.Nombre} - {e.Cargo}"));
        }

        private void RegistrarVenta()
        {
            LimpiarErrores();
            bool valido = true;

            if (comboVehiculo.SelectedItem == null)
            {
                lblErrorVehiculo.Text = "⚠ Seleccione un vehículo";
                valido = false;
            }
            if (comboEmpleado.SelectedItem == null)
            {
                lblErrorEmpleado.Text = "⚠ Seleccione un empleado";
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtNombreCliente.Text))
            {
                lblErrorCliente.Text = "⚠ El nombre del cliente es obligatorio";
                valido = false;
            }
            if (!double.TryParse(txtPrecioVenta.Text, out double precio) || precio <= 0)
            {
                lblErrorPrecio.Text = "⚠ Precio inválido";
                valido = false;
            }

            if (!valido) return;

            try
            {
                // Registrar o buscar cliente
                var cliente = _repoCliente.BuscarPorCedula(txtCedulaCliente.Text);
                if (cliente == null)
                {
                    cliente = new Cliente(txtNombreCliente.Text,
                        txtCedulaCliente.Text, txtContactoCliente.Text);
                    _repoCliente.Agregar(cliente);
                    cliente = _repoCliente.BuscarPorCedula(txtCedulaCliente.Text);
                }

                int vehiculoId = ((ComboItem)comboVehiculo.SelectedItem!).Id;
                int empleadoId = ((ComboItem)comboEmpleado.SelectedItem!).Id;

                var venta = new Venta(vehiculoId, empleadoId, cliente!.Id, precio);
                _repoVenta.Agregar(venta);

                CargarVentas();
                CargarCombos();
                LimpiarCampos();

                MessageBox.Show("Venta registrada correctamente. El vehículo quedó marcado como Vendido.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarVentas()
        {
            dgvVentas.Rows.Clear();
            foreach (var v in _repoVenta.ObtenerTodos())
                dgvVentas.Rows.Add(v.Id, v.VehiculoDescripcion, v.EmpleadoNombre,
                    v.ClienteNombre, v.PrecioVenta.ToString("C"), v.Fecha.ToString("dd/MM/yyyy HH:mm"));
        }

        private void LimpiarCampos()
        {
            txtNombreCliente.Clear(); txtCedulaCliente.Clear();
            txtContactoCliente.Clear(); txtPrecioVenta.Clear();
            comboVehiculo.SelectedIndex = -1;
            comboEmpleado.SelectedIndex = -1;
            LimpiarErrores();
        }

        private void LimpiarErrores()
        {
            lblErrorVehiculo.Text = "";
            lblErrorEmpleado.Text = "";
            lblErrorCliente.Text = "";
            lblErrorPrecio.Text = "";
        }

        private TextBox CrearTextBox(Point location, string placeholder)
        {
            return new TextBox
            {
                Location = location,
                Width = 340,
                PlaceholderText = placeholder,
                Font = new Font("Segoe UI", 9)
            };
        }

        private Label CrearLabelError(Point location)
        {
            return new Label
            {
                Location = location,
                AutoSize = true,
                ForeColor = System.Drawing.Color.FromArgb(192, 57, 43),
                Font = new Font("Segoe UI", 7.5f, FontStyle.Italic),
                Text = ""
            };
        }

        private Button CrearBoton(string texto, Point location, Size size, System.Drawing.Color color)
        {
            return new Button
            {
                Text = texto,
                Location = location,
                Size = size,
                BackColor = color,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9)
            };
        }

        // Clase auxiliar para ComboBox con Id
        private class ComboItem
        {
            public int Id { get; }
            private string _texto;
            public ComboItem(int id, string texto) { Id = id; _texto = texto; }
            public override string ToString() => _texto;
        }
    }
}