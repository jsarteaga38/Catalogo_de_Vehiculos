using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Infrastructure.Repositories;

namespace Catalogo_de_Vehiculo.Presentation.Forms
{
    public class FormDashboard : Form
    {
        private readonly List<Vehiculo> _vehiculos;
        private readonly string _connectionString;
        private Panel pnlGrafica;

        public FormDashboard(List<Vehiculo> vehiculos, string connectionString = "")
        {
            _vehiculos = vehiculos;
            _connectionString = connectionString;
            InicializarUI();
        }

        private void InicializarUI()
        {
            this.Text = "📊 Dashboard — Estadísticas del Catálogo";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9);
            this.AutoScroll = true;

            // ── Título ───────────────────────────────────────────
            Label lblTitulo = new Label();
            lblTitulo.Text = "📊 Dashboard del Catálogo de Vehículos";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.AutoSize = true;
            this.Controls.Add(lblTitulo);

            // ── Calcular estadísticas ────────────────────────────
            int automoviles = _vehiculos.Count(v => v is Automovil);
            int camiones = _vehiculos.Count(v => v is Camion);
            int motos = _vehiculos.Count(v => v is Motocicleta);
            int disponibles = _vehiculos.Count(v => v.Estado == "Disponible");
            int vendidos = _vehiculos.Count(v => v.Estado == "Vendido");
            int enMantenimiento = _vehiculos.Count(v => v.Estado == "En Mantenimiento");
            double valorFlota = _vehiculos.Sum(v => Math.Max(0, v.Precio - v.CalcularDepreciacion()));
            double depreciacion = _vehiculos.Sum(v => v.CalcularDepreciacion());
            double mantenimiento = _vehiculos.Sum(v => v.CalcularCostoMantenimiento());
            double totalInvertido = _vehiculos.Sum(v => v.Precio);

            // ── Tarjetas por tipo ────────────────────────────────
            Label lblTipoTitulo = new Label();
            lblTipoTitulo.Text = "Distribución por tipo";
            lblTipoTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTipoTitulo.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblTipoTitulo.Location = new Point(20, 55);
            lblTipoTitulo.AutoSize = true;
            this.Controls.Add(lblTipoTitulo);

            CrearTarjeta("🚗 Automóviles", automoviles.ToString(),
                System.Drawing.Color.FromArgb(52, 152, 219), new Point(20, 80));
            CrearTarjeta("🚛 Camiones", camiones.ToString(),
                System.Drawing.Color.FromArgb(231, 76, 60), new Point(200, 80));
            CrearTarjeta("🏍 Motocicletas", motos.ToString(),
                System.Drawing.Color.FromArgb(39, 174, 96), new Point(380, 80));
            CrearTarjeta("🚘 Total", _vehiculos.Count.ToString(),
                System.Drawing.Color.FromArgb(44, 62, 80), new Point(560, 80));

            // ── Tarjetas por estado ──────────────────────────────
            Label lblEstadoTitulo = new Label();
            lblEstadoTitulo.Text = "Estado de la flota";
            lblEstadoTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblEstadoTitulo.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblEstadoTitulo.Location = new Point(20, 195);
            lblEstadoTitulo.AutoSize = true;
            this.Controls.Add(lblEstadoTitulo);

            CrearTarjeta("✅ Disponibles", disponibles.ToString(),
                System.Drawing.Color.FromArgb(39, 174, 96), new Point(20, 220));
            CrearTarjeta("💰 Vendidos", vendidos.ToString(),
                System.Drawing.Color.FromArgb(243, 156, 18), new Point(200, 220));
            CrearTarjeta("🔧 En Mantenimiento", enMantenimiento.ToString(),
                System.Drawing.Color.FromArgb(142, 68, 173), new Point(380, 220));

            // ── Tarjetas financieras ─────────────────────────────
            Label lblFinTitulo = new Label();
            lblFinTitulo.Text = "Resumen financiero";
            lblFinTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblFinTitulo.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblFinTitulo.Location = new Point(20, 335);
            lblFinTitulo.AutoSize = true;
            this.Controls.Add(lblFinTitulo);

            CrearTarjetaFinanciera("💵 Total Invertido", totalInvertido.ToString("C"),
                System.Drawing.Color.FromArgb(44, 62, 80), new Point(20, 360));
            CrearTarjetaFinanciera("💰 Valor Actual Flota", valorFlota.ToString("C"),
                System.Drawing.Color.FromArgb(39, 174, 96), new Point(270, 360));
            CrearTarjetaFinanciera("📉 Depreciación Total", depreciacion.ToString("C"),
                System.Drawing.Color.FromArgb(231, 76, 60), new Point(520, 360));
            CrearTarjetaFinanciera("🔧 Mantenimiento Total", mantenimiento.ToString("C"),
                System.Drawing.Color.FromArgb(243, 156, 18), new Point(770, 360));

            // ── Gráfica de barras ────────────────────────────────
            Label lblGrafica = new Label();
            lblGrafica.Text = "Distribución por tipo de vehículo";
            lblGrafica.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblGrafica.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblGrafica.Location = new Point(20, 460);
            lblGrafica.AutoSize = true;
            this.Controls.Add(lblGrafica);

            pnlGrafica = new Panel();
            pnlGrafica.Location = new Point(20, 490);
            pnlGrafica.Size = new Size(950, 160);
            pnlGrafica.BackColor = System.Drawing.Color.White;
            pnlGrafica.BorderStyle = BorderStyle.FixedSingle;
            pnlGrafica.Paint += (s, e) => DibujarGrafica(e.Graphics,
                automoviles, camiones, motos, disponibles, vendidos, enMantenimiento);
            this.Controls.Add(pnlGrafica);
        }

        private void CrearTarjeta(string titulo, string valor, System.Drawing.Color color, Point location)
        {
            Panel card = new Panel();
            card.Location = location;
            card.Size = new Size(160, 100);
            card.BackColor = color;

            Label lblTitulo = new Label();
            lblTitulo.Text = titulo;
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblTitulo.Location = new Point(10, 12);
            lblTitulo.AutoSize = true;

            Label lblValor = new Label();
            lblValor.Text = valor;
            lblValor.ForeColor = System.Drawing.Color.White;
            lblValor.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblValor.Location = new Point(10, 45);
            lblValor.AutoSize = true;

            card.Controls.Add(lblTitulo);
            card.Controls.Add(lblValor);
            this.Controls.Add(card);
        }

        private void CrearTarjetaFinanciera(string titulo, string valor, System.Drawing.Color color, Point location)
        {
            Panel card = new Panel();
            card.Location = location;
            card.Size = new Size(230, 80);
            card.BackColor = System.Drawing.Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;

            Panel indicador = new Panel();
            indicador.Size = new Size(8, 80);
            indicador.Location = new Point(0, 0);
            indicador.BackColor = color;
            card.Controls.Add(indicador);

            Label lblTitulo = new Label();
            lblTitulo.Text = titulo;
            lblTitulo.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);
            lblTitulo.Font = new Font("Segoe UI", 8);
            lblTitulo.Location = new Point(18, 12);
            lblTitulo.AutoSize = true;

            Label lblValor = new Label();
            lblValor.Text = valor;
            lblValor.ForeColor = color;
            lblValor.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblValor.Location = new Point(18, 38);
            lblValor.AutoSize = true;

            card.Controls.Add(lblTitulo);
            card.Controls.Add(lblValor);
            this.Controls.Add(card);
        }

        private void DibujarGrafica(Graphics g, int automoviles, int camiones,
            int motos, int disponibles, int vendidos, int enMant)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int[] valoresTipo = { automoviles, camiones, motos };
            int[] valoresEstado = { disponibles, vendidos, enMant };
            string[] labelsTipo = { "Automóviles", "Camiones", "Motocicletas" };
            string[] labelsEstado = { "Disponibles", "Vendidos", "En Mantenimiento" };

            System.Drawing.Color[] coloresTipo = {
                System.Drawing.Color.FromArgb(52, 152, 219),
                System.Drawing.Color.FromArgb(231, 76, 60),
                System.Drawing.Color.FromArgb(39, 174, 96)
            };
            System.Drawing.Color[] coloresEstado = {
                System.Drawing.Color.FromArgb(39, 174, 96),
                System.Drawing.Color.FromArgb(243, 156, 18),
                System.Drawing.Color.FromArgb(142, 68, 173)
            };

            int maxVal = Math.Max(
                valoresTipo.Length > 0 ? valoresTipo.Max() : 1,
                valoresEstado.Length > 0 ? valoresEstado.Max() : 1);
            if (maxVal == 0) maxVal = 1;

            int panelH = pnlGrafica.Height - 40;
            int barWidth = 60;
            int startXTipo = 30;
            int startXEstado = 530;

            // Título tipos
            g.DrawString("Por tipo", new Font("Segoe UI", 9, FontStyle.Bold),
                Brushes.Gray, startXTipo, 5);

            // Título estados
            g.DrawString("Por estado", new Font("Segoe UI", 9, FontStyle.Bold),
                Brushes.Gray, startXEstado, 5);

            // Dibujar barras por tipo
            for (int i = 0; i < 3; i++)
            {
                int barH = (int)((double)valoresTipo[i] / maxVal * (panelH - 30));
                int x = startXTipo + i * (barWidth + 40);
                int y = panelH - barH;

                using (SolidBrush brush = new SolidBrush(coloresTipo[i]))
                    g.FillRectangle(brush, x, y, barWidth, barH);

                g.DrawString(valoresTipo[i].ToString(),
                    new Font("Segoe UI", 10, FontStyle.Bold),
                    Brushes.Black, x + barWidth / 2 - 8, y - 22);

                g.DrawString(labelsTipo[i],
                    new Font("Segoe UI", 7),
                    Brushes.Gray, x, panelH + 5);
            }

            // Dibujar barras por estado
            for (int i = 0; i < 3; i++)
            {
                int barH = (int)((double)valoresEstado[i] / maxVal * (panelH - 30));
                int x = startXEstado + i * (barWidth + 40);
                int y = panelH - barH;

                using (SolidBrush brush = new SolidBrush(coloresEstado[i]))
                    g.FillRectangle(brush, x, y, barWidth, barH);

                g.DrawString(valoresEstado[i].ToString(),
                    new Font("Segoe UI", 10, FontStyle.Bold),
                    Brushes.Black, x + barWidth / 2 - 8, y - 22);

                g.DrawString(labelsEstado[i],
                    new Font("Segoe UI", 7),
                    Brushes.Gray, x, panelH + 5);
            }

            // Línea divisoria
            g.DrawLine(Pens.LightGray, 500, 0, 500, panelH + 20);
        }
    }
}