using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Infrastructure.Repositories;

namespace Catalogo_de_Vehiculo.Presentation.Forms
{
    public class FormMantenimientos : Form
    {
        private readonly MantenimientoRepository _repoMant;
        private readonly VehiculoRepository _repoVehiculo;

        private ComboBox comboVehiculo;
        private ComboBox comboTipo;
        private TextBox txtCosto;
        private DateTimePicker dtpProximo;
        private Button btnRegistrar;
        private Button btnMostrarTodos;
        private Button btnLiberar;
        private DataGridView dgvMantenimientos;
        private GroupBox grpDatos;
        private GroupBox grpLista;
        private Label lblErrorVehiculo;
        private Label lblErrorTipo;
        private Label lblErrorCosto;

        public FormMantenimientos(string connectionString)
        {
            _repoMant = new MantenimientoRepository(connectionString);
            _repoVehiculo = new VehiculoRepository(connectionString);
            InicializarUI();
            WireEvents();
            CargarCombos();
            CargarMantenimientos();
        }

        private void InicializarUI()
        {
            this.Text = "🔧 Gestión de Mantenimientos";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9);

            // ── Panel datos ──────────────────────────────────────
            grpDatos = new GroupBox();
            grpDatos.Text = "🔧 Registrar Mantenimiento";
            grpDatos.Location = new Point(15, 15);
            grpDatos.Size = new Size(960, 200);
            grpDatos.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpDatos.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            var lblVehiculo = new Label() { Text = "Vehículo:", Location = new Point(15, 25), AutoSize = true };
            comboVehiculo = new ComboBox() { Location = new Point(15, 45), Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };
            lblErrorVehiculo = CrearLabelError(new Point(15, 70));

            var lblTipo = new Label() { Text = "Tipo de mantenimiento:", Location = new Point(385, 25), AutoSize = true };
            comboTipo = new ComboBox() { Location = new Point(385, 45), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            comboTipo.Items.AddRange(new string[] {
                "Cambio de aceite",
                "Revisión de frenos",
                "Cambio de llantas",
                "Revisión general",
                "Mantenimiento eléctrico",
                "Cambio de batería",
                "Revisión de motor",
                "Otro"
            });
            lblErrorTipo = CrearLabelError(new Point(385, 70));

            var lblCosto = new Label() { Text = "Costo ($):", Location = new Point(655, 25), AutoSize = true };
            txtCosto = new TextBox() { Location = new Point(655, 45), Width = 150, PlaceholderText = "Costo *" };
            lblErrorCosto = CrearLabelError(new Point(655, 70));

            var lblProximo = new Label() { Text = "Próximo mantenimiento:", Location = new Point(820, 25), AutoSize = true };
            dtpProximo = new DateTimePicker()
            {
                Location = new Point(820, 45),
                Width = 120,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(3)
            };

            btnRegistrar = CrearBoton("✔ Registrar", new Point(15, 140), new Size(150, 40),
                System.Drawing.Color.FromArgb(39, 174, 96));
            btnMostrarTodos = CrearBoton("↺ Mostrar Todos", new Point(180, 140), new Size(150, 40),
                System.Drawing.Color.FromArgb(52, 152, 219));
            btnLiberar = CrearBoton("✅ Liberar Vehículo", new Point(345, 140), new Size(160, 40),
                System.Drawing.Color.FromArgb(142, 68, 173));

            grpDatos.Controls.AddRange(new Control[] {
                lblVehiculo, comboVehiculo, lblErrorVehiculo,
                lblTipo, comboTipo, lblErrorTipo,
                lblCosto, txtCosto, lblErrorCosto,
                lblProximo, dtpProximo,
                btnRegistrar, btnMostrarTodos, btnLiberar
            });

            // ── Panel lista ──────────────────────────────────────
            grpLista = new GroupBox();
            grpLista.Text = "📋 Historial de Mantenimientos";
            grpLista.Location = new Point(15, 225);
            grpLista.Size = new Size(960, 375);
            grpLista.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpLista.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            grpLista.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            dgvMantenimientos = new DataGridView();
            dgvMantenimientos.Dock = DockStyle.Fill;
            dgvMantenimientos.ReadOnly = true;
            dgvMantenimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMantenimientos.BackgroundColor = System.Drawing.Color.White;
            dgvMantenimientos.AllowUserToAddRows = false;
            dgvMantenimientos.BorderStyle = BorderStyle.None;
            dgvMantenimientos.RowHeadersVisible = false;
            dgvMantenimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMantenimientos.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dgvMantenimientos.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvMantenimientos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvMantenimientos.EnableHeadersVisualStyles = false;
            dgvMantenimientos.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            dgvMantenimientos.Columns.Add("Id", "ID");
            dgvMantenimientos.Columns.Add("Vehiculo", "Vehículo");
            dgvMantenimientos.Columns.Add("Tipo", "Tipo");
            dgvMantenimientos.Columns.Add("Costo", "Costo");
            dgvMantenimientos.Columns.Add("Fecha", "Fecha");
            dgvMantenimientos.Columns.Add("Proximo", "Próximo Mantenimiento");

            grpLista.Controls.Add(dgvMantenimientos);

            this.Controls.Add(grpDatos);
            this.Controls.Add(grpLista);
        }

        private void WireEvents()
        {
            btnRegistrar.Click += (s, e) => RegistrarMantenimiento();
            btnMostrarTodos.Click += (s, e) => CargarMantenimientos();
            btnLiberar.Click += (s, e) => LiberarVehiculo();
        }

        private void CargarCombos()
        {
            comboVehiculo.Items.Clear();
            var vehiculos = _repoVehiculo.ObtenerTodos();
            foreach (var v in vehiculos)
            {
                string estado = v.Estado == "Disponible" ? "✅" : v.Estado == "En Mantenimiento" ? "🔧" : "❌";
                comboVehiculo.Items.Add(new ComboItem(v.Id,
                    $"{estado} {v.Marca} {v.Modelo} ({v.Año}) - {v.Estado}"));
            }
        }

        private void RegistrarMantenimiento()
        {
            LimpiarErrores();
            bool valido = true;

            if (comboVehiculo.SelectedItem == null)
            {
                lblErrorVehiculo.Text = "⚠ Seleccione un vehículo";
                valido = false;
            }
            if (comboTipo.SelectedItem == null)
            {
                lblErrorTipo.Text = "⚠ Seleccione el tipo";
                valido = false;
            }
            if (!double.TryParse(txtCosto.Text, out double costo) || costo < 0)
            {
                lblErrorCosto.Text = "⚠ Costo inválido";
                valido = false;
            }

            if (!valido) return;

            try
            {
                int vehiculoId = ((ComboItem)comboVehiculo.SelectedItem!).Id;
                var mant = new Mantenimiento(
                    vehiculoId,
                    comboTipo.SelectedItem.ToString()!,
                    costo,
                    dtpProximo.Value
                );

                _repoMant.Agregar(mant);
                CargarMantenimientos();
                CargarCombos();
                LimpiarCampos();

                MessageBox.Show("Mantenimiento registrado. El vehículo quedó en estado 'En Mantenimiento'.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LiberarVehiculo()
        {
            if (comboVehiculo.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un vehículo para liberar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int vehiculoId = ((ComboItem)comboVehiculo.SelectedItem!).Id;

            try
            {
                string sql = "UPDATE Vehiculos SET Estado = 'Disponible' WHERE Id = @Id";
                using var conn = new Microsoft.Data.SqlClient.SqlConnection(
                    _repoVehiculo.GetConnectionString());
                using var cmd = new Microsoft.Data.SqlClient.SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", vehiculoId);
                conn.Open();
                cmd.ExecuteNonQuery();

                CargarCombos();
                MessageBox.Show("Vehículo liberado. Estado cambiado a Disponible.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarMantenimientos()
        {
            dgvMantenimientos.Rows.Clear();
            foreach (var m in _repoMant.ObtenerTodos())
                dgvMantenimientos.Rows.Add(
                    m.Id, m.VehiculoDescripcion, m.TipoMantenimiento,
                    m.Costo.ToString("C"), m.Fecha.ToString("dd/MM/yyyy"),
                    m.ProximoMantenimiento.ToString("dd/MM/yyyy"));
        }

        private void LimpiarCampos()
        {
            txtCosto.Clear();
            comboVehiculo.SelectedIndex = -1;
            comboTipo.SelectedIndex = -1;
            dtpProximo.Value = DateTime.Now.AddMonths(3);
            LimpiarErrores();
        }

        private void LimpiarErrores()
        {
            lblErrorVehiculo.Text = "";
            lblErrorTipo.Text = "";
            lblErrorCosto.Text = "";
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

        private class ComboItem
        {
            public int Id { get; }
            private string _texto;
            public ComboItem(int id, string texto) { Id = id; _texto = texto; }
            public override string ToString() => _texto;
        }
    }
}