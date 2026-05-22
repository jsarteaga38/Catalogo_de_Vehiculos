using System;
using System.Collections.Generic;
using ClosedXML.Excel;
using Catalogo_de_Vehiculo.Domain.Entities;

namespace Catalogo_de_Vehiculo.Application.Services
{
    /// <summary>
    /// Servicio de exportación a Excel usando ClosedXML.
    /// Aplica SRP: solo se encarga de exportar, nada más.
    /// </summary>
    public class ExportService
    {
        public string ExportarAExcel(List<Vehiculo> vehiculos)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Catálogo de Vehículos");

            // ── Encabezado principal ─────────────────────────────
            ws.Range("A1:I1").Merge();
            ws.Cell("A1").Value = "CATÁLOGO DE VEHÍCULOS";
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 16;
            ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("A1").Style.Fill.BackgroundColor = XLColor.FromArgb(44, 62, 80);
            ws.Cell("A1").Style.Font.FontColor = XLColor.White;

            ws.Cell("A2").Value = $"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}";
            ws.Cell("A2").Style.Font.Italic = true;
            ws.Cell("A2").Style.Font.FontColor = XLColor.Gray;

            // ── Encabezados de columnas ──────────────────────────
            string[] headers = { "Tipo", "Marca", "Modelo", "Año", "Precio",
                                  "Color", "Característica", "Depreciación", "Valor Actual" };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(4, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromArgb(52, 152, 219);
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            }

            // ── Datos ────────────────────────────────────────────
            int row = 5;
            foreach (Vehiculo v in vehiculos)
            {
                double depreciacion = v.CalcularDepreciacion();
                double valorActual = Math.Max(0, v.Precio - depreciacion);

                string caracteristica = "";
                if (v is Camion camion) caracteristica = camion.peso + " Ton";
                else if (v is Automovil auto) caracteristica = auto.cantidadPlasas + " Puertas";
                else if (v is Motocicleta moto) caracteristica = moto.tipoMotosicleta;

                ws.Cell(row, 1).Value = v.GetType().Name;
                ws.Cell(row, 2).Value = v.Marca;
                ws.Cell(row, 3).Value = v.Modelo;
                ws.Cell(row, 4).Value = v.Año;
                ws.Cell(row, 5).Value = v.Precio;
                ws.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";
                ws.Cell(row, 6).Value = v.Color;
                ws.Cell(row, 7).Value = caracteristica;
                ws.Cell(row, 8).Value = depreciacion;
                ws.Cell(row, 8).Style.NumberFormat.Format = "$#,##0.00";
                ws.Cell(row, 9).Value = valorActual;
                ws.Cell(row, 9).Style.NumberFormat.Format = "$#,##0.00";

                // Filas alternas
                if (row % 2 == 0)
                {
                    ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromArgb(245, 247, 250);
                }

                row++;
            }

            // ── Fila de totales ──────────────────────────────────
            ws.Cell(row, 1).Value = "TOTALES";
            ws.Cell(row, 1).Style.Font.Bold = true;
            ws.Cell(row, 5).FormulaA1 = $"=SUM(E5:E{row - 1})";
            ws.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";
            ws.Cell(row, 5).Style.Font.Bold = true;
            ws.Cell(row, 8).FormulaA1 = $"=SUM(H5:H{row - 1})";
            ws.Cell(row, 8).Style.NumberFormat.Format = "$#,##0.00";
            ws.Cell(row, 8).Style.Font.Bold = true;
            ws.Cell(row, 9).FormulaA1 = $"=SUM(I5:I{row - 1})";
            ws.Cell(row, 9).Style.NumberFormat.Format = "$#,##0.00";
            ws.Cell(row, 9).Style.Font.Bold = true;
            ws.Row(row).Style.Fill.BackgroundColor = XLColor.FromArgb(44, 62, 80);
            ws.Row(row).Style.Font.FontColor = XLColor.White;

            // ── Ajustar columnas ─────────────────────────────────
            ws.Columns().AdjustToContents();

            // ── Guardar archivo ──────────────────────────────────
            string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string ruta = System.IO.Path.Combine(escritorio,
                $"Catalogo_Vehiculos_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            workbook.SaveAs(ruta);
            return ruta;
        }
    }
}