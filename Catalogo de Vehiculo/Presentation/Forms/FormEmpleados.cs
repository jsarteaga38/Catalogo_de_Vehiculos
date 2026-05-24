using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Infrastructure.Repositories;

namespace Catalogo_de_Vehiculo.Presentation.Forms
{
    public class FormEmpleados : Form
    {
        private readonly EmpleadoRepository _repo;

        private TextBox txtNombre;
        private TextBox txtCedula;
        private TextBox txtCargo;
        private TextBox txtContacto;
        private TextBox txtBuscar;
        private Button btnAgregar;
        private Button btnEliminar;
        private Button btnBuscar;
        private Button btnMostrarTodos;
        private DataGridView dgvEmpleados;
        private GroupBox grpDatos;
        private GroupBox grpBusqueda;
        private GroupBox grpLista;
        private Label lblErrorNombre;
        private Label lblErrorCedula;
        private Label lblErrorCargo;

        public FormEmpleados(string connectionString)
        {
            _repo = new EmpleadoRepository(connectionString);
            InicializarUI();
            WireEvents();
            CargarEmpleados();
        }

        private void InicializarUI()
        {
            this.Text = "👥 Gestión de Empleados";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9);

            // ── Panel datos ──────────────────────────────────────
            grpDatos = new GroupBox();
            grpDatos.Text = "➕ Registrar Empleado";
            grpDatos.Location = new Point(15, 15);
            grpDatos.Size = new Size(380, 280);
            grpDatos.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpDatos.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            txtNombre = CrearTextBox(new Point(15, 30), "Nombre completo *");
            txtCedula = CrearTextBox(new Point(15, 80), "Cédula *");
            txtCargo = CrearTextBox(new Point(15, 130), "Cargo *");
            txtContacto = CrearTextBox(new Point(15, 180), "Contacto (teléfono o email)");

            lblErrorNombre = CrearLabelError(new Point(15, 58));
            lblErrorCedula = CrearLabelError(new Point(15, 108));
            lblErrorCargo = CrearLabelError(new Point(15, 158));

            btnAgregar = CrearBoton("✔ Agregar", new Point(15, 225), new Size(130, 35),
                System.Drawing.Color.FromArgb(39, 174, 96));
            btnEliminar = CrearBoton("🗑 Eliminar", new Point(160, 225), new Size(130, 35),
                System.Drawing.Color.FromArgb(192, 57, 43));

            grpDatos.Controls.AddRange(new Control[] {
                txtNombre, txtCedula, txtCargo, txtContacto,
                lblErrorNombre, lblErrorCedula, lblErrorCargo,
                btnAgregar, btnEliminar
            });

            // ── Panel búsqueda ───────────────────────────────────
            grpBusqueda = new GroupBox();
            grpBusqueda.Text = "🔍 Buscar por Cédula";
            grpBusqueda.Location = new Point(415, 15);
            grpBusqueda.Size = new Size(450, 100);
            grpBusqueda.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpBusqueda.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);

            txtBuscar = CrearTextBox(new Point(15, 30), "Cédula del empleado");
            txtBuscar.Width = 250;

            btnBuscar = CrearBoton("🔍 Buscar", new Point(280, 28), new Size(80, 30),
                System.Drawing.Color.FromArgb(52, 152, 219));
            btnMostrarTodos = CrearBoton("↺ Todos", new Point(370, 28), new Size(65, 30),
                System.Drawing.Color.FromArgb(127, 140, 141));

            grpBusqueda.Controls.AddRange(new Control[] { txtBuscar, btnBuscar, btnMostrarTodos });

            // ── Panel lista ──────────────────────────────────────
            grpLista = new GroupBox();
            grpLista.Text = "📋 Lista de Empleados";
            grpLista.Location = new Point(15, 310);
            grpLista.Size = new Size(850, 240);
            grpLista.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grpLista.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            grpLista.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            dgvEmpleados = new DataGridView();
            dgvEmpleados.Dock = DockStyle.Fill;
            dgvEmpleados.ReadOnly = true;
            dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmpleados.BackgroundColor = System.Drawing.Color.White;
            dgvEmpleados.AllowUserToAddRows = false;
            dgvEmpleados.BorderStyle = BorderStyle.None;
            dgvEmpleados.RowHeadersVisible = false;
            dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dgvEmpleados.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvEmpleados.EnableHeadersVisualStyles = false;
            dgvEmpleados.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            dgvEmpleados.Columns.Add("Id", "ID");
            dgvEmpleados.Columns.Add("Nombre", "Nombre");
            dgvEmpleados.Columns.Add("Cedula", "Cédula");
            dgvEmpleados.Columns.Add("Cargo", "Cargo");
            dgvEmpleados.Columns.Add("Contacto", "Contacto");

            grpLista.Controls.Add(dgvEmpleados);

            this.Controls.Add(grpDatos);
            this.Controls.Add(grpBusqueda);
            this.Controls.Add(grpLista);
        }

        private void WireEvents()
        {
            btnAgregar.Click += (s, e) => AgregarEmpleado();
            btnEliminar.Click += (s, e) => EliminarEmpleado();
            btnBuscar.Click += (s, e) => BuscarEmpleado();
            btnMostrarTodos.Click += (s, e) => CargarEmpleados();
        }

        private void AgregarEmpleado()
        {
            LimpiarErrores();
            bool valido = true;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                lblErrorNombre.Text = "⚠ El nombre es obligatorio";
                txtNombre.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtCedula.Text))
            {
                lblErrorCedula.Text = "⚠ La cédula es obligatoria";
                txtCedula.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtCargo.Text))
            {
                lblErrorCargo.Text = "⚠ El cargo es obligatorio";
                txtCargo.BackColor = System.Drawing.Color.FromArgb(255, 235, 235);
                valido = false;
            }

            if (!valido) return;

            try
            {
                var empleado = new Empleado(
                    txtNombre.Text, txtCedula.Text,
                    txtCargo.Text, txtContacto.Text);

                _repo.Agregar(empleado);
                CargarEmpleados();
                LimpiarCampos();
                MessageBox.Show("Empleado registrado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarEmpleado()
        {
            if (dgvEmpleados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un empleado de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvEmpleados.SelectedRows[0].Cells["Id"].Value);
            string nombre = dgvEmpleados.SelectedRows[0].Cells["Nombre"].Value.ToString()!;

            var confirm = MessageBox.Show(
                $"¿Desea eliminar al empleado {nombre}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _repo.Eliminar(id);
                    CargarEmpleados();
                    MessageBox.Show("Empleado eliminado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BuscarEmpleado()
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text)) return;

            var empleado = _repo.BuscarPorCedula(txtBuscar.Text);
            dgvEmpleados.Rows.Clear();

            if (empleado != null)
            {
                dgvEmpleados.Rows.Add(empleado.Id, empleado.Nombre,
                    empleado.Cedula, empleado.Cargo, empleado.Contacto);
            }
            else
            {
                MessageBox.Show("No se encontró ningún empleado con esa cédula.", "Sin resultados",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CargarEmpleados()
        {
            dgvEmpleados.Rows.Clear();
            foreach (var e in _repo.ObtenerTodos())
                dgvEmpleados.Rows.Add(e.Id, e.Nombre, e.Cedula, e.Cargo, e.Contacto);
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear(); txtCedula.Clear();
            txtCargo.Clear(); txtContacto.Clear();
            LimpiarErrores();
        }

        private void LimpiarErrores()
        {
            lblErrorNombre.Text = ""; lblErrorCedula.Text = ""; lblErrorCargo.Text = "";
            txtNombre.BackColor = System.Drawing.Color.White;
            txtCedula.BackColor = System.Drawing.Color.White;
            txtCargo.BackColor = System.Drawing.Color.White;
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
    }
}