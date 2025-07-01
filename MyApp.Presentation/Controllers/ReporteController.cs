using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using MyApp.Business.Services;
using MyApp.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace MyApp.Presentation.Controllers
{
    public class ReporteController : Controller
    {
        private readonly ArtículoService _artículoService;
        private readonly PréstamoService _préstamoService;

        public ReporteController(ArtículoService artículoService, PréstamoService préstamoService)
        {
            _artículoService = artículoService;
            _préstamoService = préstamoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Acción para descargar el PDF
        public IActionResult ExportarPDF()
        {
            // Obtener todos los artículos desde el servicio
            var artículos = _artículoService.ObtenerTodos();

            // Generar el PDF
            var pdfBytes = GenerarPDF(artículos);

            // Devolver el archivo PDF como respuesta
            return File(pdfBytes, "application/pdf", "ListadoArtículos.pdf");
        }

        // Método para generar el PDF
        private byte[] GenerarPDF(IEnumerable<Artículo> artículos)
        {
            // Generar el PDF y capturar los bytes
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Encabezado
                    page.Header().Column(column =>
                    {
                        column.Spacing(5);

                        // Título principal
                        column.Item().Text("Listado de Artículos").FontSize(24).Bold().AlignCenter().FontColor("#2E86C1");
                    });

                    // Contenido principal
                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        column.Spacing(10);

                        // Tabla de artículos
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Código
                                columns.RelativeColumn(3); // Nombre
                                columns.RelativeColumn(2); // Categoría
                                columns.RelativeColumn(2); // Estado
                                columns.RelativeColumn(3); // Ubicación
                            });

                            // Encabezados de la tabla con estilo
                            table.Header(header =>
                            {
                                header.Cell().Background("#F0F8FF").Border(1).BorderColor("#D3D3D3").Padding(5).Text("Código").Bold().AlignCenter();
                                header.Cell().Background("#F0F8FF").Border(1).BorderColor("#D3D3D3").Padding(5).Text("Nombre").Bold().AlignCenter();
                                header.Cell().Background("#F0F8FF").Border(1).BorderColor("#D3D3D3").Padding(5).Text("Categoría").Bold().AlignCenter();
                                header.Cell().Background("#F0F8FF").Border(1).BorderColor("#D3D3D3").Padding(5).Text("Estado").Bold().AlignCenter();
                                header.Cell().Background("#F0F8FF").Border(1).BorderColor("#D3D3D3").Padding(5).Text("Ubicación").Bold().AlignCenter();
                            });

                            // Filas de la tabla
                            foreach (var artículo in artículos)
                            {
                                table.Cell().Border(1).BorderColor("#D3D3D3").Padding(5).Text(artículo.Código).AlignCenter();
                                table.Cell().Border(1).BorderColor("#D3D3D3").Padding(5).Text(artículo.Nombre);
                                table.Cell().Border(1).BorderColor("#D3D3D3").Padding(5).Text(artículo.Categoría).AlignCenter();
                                table.Cell().Border(1).BorderColor("#D3D3D3").Padding(5).Text(artículo.Estado).AlignCenter();
                                table.Cell().Border(1).BorderColor("#D3D3D3").Padding(5).Text(artículo.Ubicación);
                            }
                        });
                    });



                    // Pie de página
                    page.Footer().AlignCenter().Row(row =>
                    {
                        row.RelativeItem().Text(text =>
                        {
                            text.Span("Página ")
                                .FontSize(10)
                                .FontColor("#696969");

                            text.CurrentPageNumber()
                                .FontSize(10)
                                .FontColor("#696969");

                            text.Span(" de ")
                                .FontSize(10)
                                .FontColor("#696969");

                            text.TotalPages()
                                .FontSize(10)
                                .FontColor("#696969");
                        });

                        row.RelativeItem().AlignRight().Text($"Generado el {DateTime.Now.ToShortDateString()}")
                            .FontSize(10)
                            .FontColor("#696969");
                    });
                });
            })
            .GeneratePdf(); // Generar el PDF

            return pdfBytes;
        }

        // Acción para descargar el Excel
        public IActionResult ExportarExcel()
        {
            // Obtener todos los préstamos desde el servicio
            var préstamos = _préstamoService.ObtenerTodos();

            // Generar el Excel
            var excelBytes = GenerarExcel(préstamos);

            // Devolver el archivo Excel como respuesta
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListadoPréstamos.xlsx");
        }

        // Método para generar el Excel
        private byte[] GenerarExcel(IEnumerable<Préstamo> préstamos)
        {
            using (var workbook = new XLWorkbook())
            {
                // Crear una hoja de trabajo
                var worksheet = workbook.Worksheets.Add("Listado de Préstamos");

                // Encabezados de la tabla
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Artículo";
                worksheet.Cell(1, 3).Value = "Usuario";
                worksheet.Cell(1, 4).Value = "Fecha Solicitud";
                worksheet.Cell(1, 5).Value = "Fecha Respuesta";
                worksheet.Cell(1, 6).Value = "Fecha Devolución";
                worksheet.Cell(1, 7).Value = "Estado";

                // Aplicar estilo a los encabezados
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Agregar filas de datos
                int rowNumber = 2;
                foreach (var préstamo in préstamos)
                {
                    worksheet.Cell(rowNumber, 1).Value = préstamo.Id;
                    worksheet.Cell(rowNumber, 2).Value = préstamo.Artículo.Nombre;
                    worksheet.Cell(rowNumber, 3).Value = préstamo.Usuario.Nombre;
                    worksheet.Cell(rowNumber, 4).Value = préstamo.FechaSolicitud.ToShortDateString();
                    worksheet.Cell(rowNumber, 5).Value = préstamo.FechaRespuesta?.ToShortDateString() ?? "No disponible";
                    worksheet.Cell(rowNumber, 6).Value = préstamo.FechaDevolución?.ToShortDateString() ?? "No disponible";
                    worksheet.Cell(rowNumber, 7).Value = préstamo.Estado;

                    rowNumber++;
                }

                // Ajustar el ancho de las columnas
                worksheet.Columns().AdjustToContents();

                // Guardar el archivo en memoria
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();

                }
            }
        }
    }
}