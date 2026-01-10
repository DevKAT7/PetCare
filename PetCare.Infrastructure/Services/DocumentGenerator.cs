using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PetCare.Infrastructure.Services
{
    public class DocumentGenerator : IDocumentGenerator
    {
        private readonly string _webRootPath;

        public DocumentGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public (byte[] Content, string ContentType, string FileName) GeneratePrescription(PrescriptionReadModel data, string templateId)
        {
            if (templateId == "nfz_word")
            {
                var fileContent = GenerateWordFromTemplate(data, "templates/template_nfz.docx");
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

                    page.Header()
                        .Text("PET CARE CLINIC")
                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

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
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", templateRelativePath);

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
    }
}
