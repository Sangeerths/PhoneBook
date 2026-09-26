using PhoneBook.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Services
{
    public class ReportService
    {
        public ReportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public string GenerateContactsReport(List<Contact> contacts)
        {
           
            var projectFolder = Directory.GetParent(AppContext.BaseDirectory)!
                                          .Parent!.Parent!.Parent!.FullName;

            var reportsFolder = Path.Combine(projectFolder, "Reports");

            Directory.CreateDirectory(reportsFolder);

            var fileName = $"PhoneBookReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var filePath = Path.Combine(reportsFolder, fileName);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Phone Book Contact Report")
                            .FontSize(18).SemiBold();

                        col.Item().Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);

                        col.Item()
                            .PaddingTop(5)
                            .LineHorizontal(1)
                            .LineColor(Colors.Grey.Lighten1);
                    });

                    page.Content().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderStyle).Text("Id");
                            header.Cell().Element(HeaderStyle).Text("Name");
                            header.Cell().Element(HeaderStyle).Text("Organization");
                            header.Cell().Element(HeaderStyle).Text("Job Title");
                            header.Cell().Element(HeaderStyle).Text("Phone(s)");
                            header.Cell().Element(HeaderStyle).Text("Email(s)");

                            static IContainer HeaderStyle(IContainer c) =>
                                c.DefaultTextStyle(x => x.SemiBold().FontColor(Colors.White))
                                 .Background(Colors.Blue.Darken1)
                                 .Padding(5);
                        });

                        foreach (var contact in contacts)
                        {
                            var phones = contact.PhoneNumbers.Count > 0
                                ? string.Join("\n",
                                    contact.PhoneNumbers.Select(p => $"{p.Label}: {p.Number}"))
                                : "-";

                            var emails = contact.Emails.Count > 0
                                ? string.Join("\n",
                                    contact.Emails.Select(e => $"{e.Label}: {e.EmailAddress}"))
                                : "-";

                            table.Cell().Element(CellStyle).Text(contact.Id.ToString());
                            table.Cell().Element(CellStyle)
                                .Text($"{contact.FirstName} {contact.LastName}");
                            table.Cell().Element(CellStyle)
                                .Text(contact.OrganizationName ?? "-");
                            table.Cell().Element(CellStyle)
                                .Text(contact.JobTitle ?? "-");
                            table.Cell().Element(CellStyle).Text(phones);
                            table.Cell().Element(CellStyle).Text(emails);

                            static IContainer CellStyle(IContainer c) =>
                                c.BorderBottom(1)
                                 .BorderColor(Colors.Grey.Lighten2)
                                 .Padding(5);
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            })
            .GeneratePdf(filePath);

            return filePath;
        }
    }
}
