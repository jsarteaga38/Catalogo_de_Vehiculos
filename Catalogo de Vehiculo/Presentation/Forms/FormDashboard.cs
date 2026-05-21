using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Presentation.Forms
{
    public class FormDashboard : Form
    {
        private readonly List<Vehiculo> _vehiculos;
        private Panel pnlGrafica;

        public FormDashboard(List<Vehiculo> vehiculos)
        {
            _vehiculos = vehiculos;
            InicializarUI();
        }

        private void InicializarUI()
        {
            this.Text = "📊 Dashboard — Estadísticas del Catálogo";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9);

            // ── Título ───────────────────────────────────────────
            Label lblTitulo = new Label();
            lblTitulo.Text = "📊 Dashboard del Catálogo de Vehículos";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.AutoSize = true;
            this.Controls.Add(lblTitulo);

            // ── Tarjetas de resumen ──────────────────────────────
            int automoviles = _vehiculos.Count(v => v is Automovil);
            int camiones = _vehiculos.Count(v => v is Camion);
            int motos = _vehiculos.Count(v => v is Motocicleta);
            double valorFlota = _vehiculos.Sum(v => Math.Max(0, v.Precio - v.CalcularDepreciacion()));
            double depreciacion = _vehiculos.Sum(v => v.CalcularDepreciacion());
            double mantenimiento = _vehiculos.Sum(v => v.CalcularCostoMantenimiento());

            CrearTarjeta("🚗 Automóviles", automoviles.ToString(),
                System.Drawing.Color.FromArgb(52, 152, 219), new Point(20, 60));
            CrearTarjeta("🚛 Camiones", camiones.ToString(),
                System.Drawing.Color.FromArgb(231, 76, 60), new Point(200, 60));
            CrearTarjeta("🏍 Motocicletas", motos.ToString(),
                System.Drawing.Color.FromArgb(39, 174, 96), new Point(380, 60));
            CrearTarjeta("🚘 Total", _vehiculos.Count.ToString(),
                System.Drawing.Color.FromArgb(44, 62, 80), new Point(560, 60));

            // ── Tarjetas financieras ─────────────────────────────
            CrearTarjetaFinanciera("💰 Valor de Flota", valorFlota.ToString("C"),
                System.Drawing.Color.FromArgb(39, 174, 96), new Point(20, 190));
            CrearTarjetaFinanciera("📉 Depreciación Total", depreciacion.ToString("C"),
                System.Drawing.Color.FromArgb(231, 76, 60), new Point(310, 190));
            CrearTarjetaFinanciera("🔧 Mantenimiento Total", mantenimiento.ToString("C"),
                System.Drawing.Color.FromArgb(243, 156, 18), new Point(600, 190));

            // ── Gráfica de barras ────────────────────────────────
            Label lblGrafica = new Label();
            lblGrafica.Text = "Distribución por tipo de vehículo";
            lblGrafica.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblGrafica.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            lblGrafica.Location = new Point(20, 310);
            lblGrafica.AutoSize = true;
            this.Controls.Add(lblGrafica);

            pnlGrafica = new Panel();
            pnlGrafica.Location = new Point(20, 340);
            pnlGrafica.Size = new Size(840, 200);
            pnlGrafica.BackColor = System.Drawing.Color.White;
            pnlGrafica.BorderStyle = BorderStyle.FixedSingle;
            pnlGrafica.Paint += (s, e) => DibujarGrafica(e.Graphics, automoviles, camiones, motos);
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
            lblTitulo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTitulo.Location = new Point(10, 15);
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
            card.Size = new Size(260, 80);
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
            lblValor.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblValor.Location = new Point(18, 35);
            lblValor.AutoSize = true;

            card.Controls.Add(lblTitulo);
            card.Controls.Add(lblValor);
            this.Controls.Add(card);
        }

        private void DibujarGrafica(Graphics g, int automoviles, int camiones, int motos)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int total = automoviles + camiones + motos;
            if (total == 0) return;

            int maxVal = Math.Max(Math.Max(automoviles, camiones), motos);
            if (maxVal == 0) return;

            int panelH = pnlGrafica.Height - 40;
            int barWidth = 120;
            int spacing = 80;
            int startX = 80;

            string[] labels = { "Automóviles", "Camiones", "Motocicletas" };
            int[] valores = { automoviles, camiones, motos };
            System.Drawing.Color[] colores = {
                System.Drawing.Color.FromArgb(52, 152, 219),
                System.Drawing.Color.FromArgb(231, 76, 60),
                System.Drawing.Color.FromArgb(39, 174, 96)
            };

            for (int i = 0; i < 3; i++)
            {
                int barH = (int)((double)valores[i] / maxVal * (panelH - 20));
                int x = startX + i * (barWidth + spacing);
                int y = panelH - barH;

                using (SolidBrush brush = new SolidBrush(colores[i]))
                    g.FillRectangle(brush, x, y, barWidth, barH);

                g.DrawString(valores[i].ToString(),
                    new Font("Segoe UI", 12, FontStyle.Bold),
                    Brushes.Black, x + barWidth / 2 - 8, y - 25);

                g.DrawString(labels[i],
                    new Font("Segoe UI", 8),
                    Brushes.Gray, x + 5, panelH + 5);
            }
        }
    }
}