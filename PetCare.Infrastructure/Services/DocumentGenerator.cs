using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using PetCare.Application.Features.Invoices.Dtos;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace PetCare.Infrastructure.Services
{
    public class DocumentGenerator : IDocumentGenerator
    {
        private readonly string _webRootPath;

        public DocumentGenerator(IWebHostEnvironment environment)
        {
            _webRootPath = environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public (byte[] Content, string ContentType, string FileName) GeneratePrescription(PrescriptionReadModel data, string templateId)
        {
            if (templateId == "with_logo_word")
            {
                var fileContent = GenerateWordFromTemplate(data, "templates/template_logo.docx");
                return (fileContent, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Prescription_{data.PrescriptionId}.docx");
            }
            else if (templateId == "simple_word")
            {
                var fileContent = GenerateWordFromTemplate(data, "templates/template_simple.docx");
                return (fileContent, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Prescription_{data.PrescriptionId}.docx");
            }
            else if (templateId == "standard_pdf")
            {
                var fileContent = GeneratePdf(data);
                return (fileContent, "application/pdf", $"Prescription{data.PrescriptionId}.pdf");
            }
            else
            {
                var txt = System.Text.Encoding.UTF8.GetBytes($"Prescription ID: {data.PrescriptionId}\nDoc: {data.MedicationName}");
                return (txt, "text/plain", "prescription.txt");
            }
        }

        private byte[] GeneratePdf(PrescriptionReadModel data)
        {
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Row(row =>
                    {
                        var logoPath = Path.Combine(_webRootPath, "images", "logo-petcare-transparent3.png");

                        if (File.Exists(logoPath))
                        {
                            row.ConstantItem(130).Image(logoPath);
                            row.ConstantItem(5);
                        }
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Text($"Date: {data.CreatedDate:dd.MM.yyyy}").AlignRight();

                        column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                        column.Item().Text(text =>
                        {
                            text.Span("Veterinarian: ").SemiBold();
                            text.Span(data.CreatedBy);
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Prescription ID: ").SemiBold();
                            text.Span($"RX-{data.PrescriptionId}");
                        });

                        column.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(med =>
                        {
                            med.Item().Text("Rp.").FontSize(14).Bold();
                            med.Item().Text(data.MedicationName).FontSize(14).Bold();
                            med.Item().Text($"{data.PacksToDispense} op.");
                            med.Item().Text($"S. {data.Dosage}, {data.Frequency}").Italic();
                        });

                        if (!string.IsNullOrEmpty(data.Instructions))
                        {
                            column.Item().PaddingTop(10).Text("Instructions:").SemiBold();
                            column.Item().Text(data.Instructions);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }

        private byte[] GenerateWordFromTemplate(PrescriptionReadModel data, string templateRelativePath)
        {
            var templatePath = Path.Combine(_webRootPath, templateRelativePath);

            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template not found at {templatePath}");

            byte[] byteArray = File.ReadAllBytes(templatePath);

            using (var stream = new MemoryStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);

                using (var wordDoc = WordprocessingDocument.Open(stream, true))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;
                    var textElements = body.Descendants<Text>();

                    var replacements = new Dictionary<string, string>
                    {
                        { "{{PrescriptionId}}", data.PrescriptionId.ToString() },
                        { "{{Date}}", data.CreatedDate.ToString("dd.MM.yyyy") },
                        { "{{CreatedBy}}", data.CreatedBy },
                        { "{{MedicationName}}", data.MedicationName },
                        { "{{Dosage}}", data.Dosage },
                        { "{{Frequency}}", data.Frequency },
                        { "{{Packs}}", data.PacksToDispense.ToString() },
                        { "{{Instructions}}", data.Instructions },
                        { "{{StartDate}}", data.StartDate.ToString("dd.MM.yyyy") },
                        { "{{EndDate}}", data.EndDate.ToString("dd.MM.yyyy") },
                        { "{{PetName}}", data.PetName },
                        { "{{OwnerName}}", data.OwnerName }
                    };

                    foreach (var text in textElements)
                    {
                        foreach (var entry in replacements)
                        {
                            if (text.Text.Contains(entry.Key))
                            {
                                text.Text = text.Text.Replace(entry.Key, entry.Value);
                            }
                        }
                    }

                    wordDoc.MainDocumentPart.Document.Save();
                }

                return stream.ToArray();
            }
        }

        public (byte[] Content, string ContentType, string FileName) GenerateInvoice(InvoiceReadModel data)
        {
            var pdfBytes = GenerateInvoicePdf(data);
            return (pdfBytes, "application/pdf", $"Invoice_{data.InvoiceNumber}.pdf");
        }

        private byte[] GenerateInvoicePdf(InvoiceReadModel data)
        {
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(header =>
                    {
                        header.Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                var logoPath = Path.Combine(_webRootPath, "images", "logo-petcare-transparent3.png");
                                if (File.Exists(logoPath))
                                {
                                    column.Item().Height(60).Image(logoPath);
                                }
                                column.Item().Text("PetCare Clinic").SemiBold().FontSize(14).FontColor(Colors.Blue.Medium);
                                column.Item().Text("123 Vet Street, Animal City");
                            });

                            row.RelativeItem().AlignRight().Column(column =>
                            {
                                column.Item().Text("INVOICE").FontSize(24).SemiBold().FontColor(Colors.Grey.Darken3);
                                column.Item().Text($"#{data.InvoiceNumber}").FontSize(14);

                                var statusColor = data.IsPaid ? Colors.Green.Medium : Colors.Red.Medium;
                                var statusText = data.IsPaid ? "PAID" : "UNPAID";
                                column.Item().PaddingTop(5).Text(statusText).FontColor(statusColor).SemiBold();
                            });
                        });
                    });

                    page.Content().PaddingVertical(20).Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c => {
                                c.Item().Text("Bill To:").FontColor(Colors.Grey.Medium);
                                c.Item().Text(data.PetOwnerName).SemiBold();
                            });
                            row.RelativeItem().AlignRight().Column(c => {
                                c.Item().Text($"Issue Date: {data.InvoiceDate:dd MMM yyyy}");
                                c.Item().Text($"Due Date: {data.DueDate:dd MMM yyyy}");
                            });
                        });

                        column.Item().PaddingTop(20).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Description").SemiBold();
                                header.Cell().AlignRight().Text("Qty").SemiBold();
                                header.Cell().AlignRight().Text("Price").SemiBold();
                                header.Cell().AlignRight().Text("Total").SemiBold();
                            });

                            foreach (var item in data.Items)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(5).Text(item.Description);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(5).AlignRight().Text(item.Quantity.ToString());
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(5).AlignRight().Text($"{item.UnitPrice:C}");
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten4).PaddingVertical(5).AlignRight().Text($"{item.Total:C}");
                            }
                        });

                        column.Item().PaddingTop(10).AlignRight().Text(text =>
                        {
                            text.Span("Total: ").FontSize(14);
                            text.Span($"{data.TotalAmount:C}").FontSize(14).SemiBold().FontColor(Colors.Blue.Darken2);
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
