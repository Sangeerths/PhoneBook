using PhoneBook.Services;
using PhoneBook.UI;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Controller
{
    public class ReportController 
    {
        private readonly ReportService _reportService;
        private readonly ContactController _contactController;
        private readonly ConsoleUI _consoleUI;

        public ReportController()
        {
            _reportService = new ReportService();
            _contactController = new ContactController();
            _consoleUI = new ConsoleUI();
        }
        public async Task ExportPdfReportFlowAsync()
        {
            var contacts = await _contactController.ViewAllContacts();
            if (contacts.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No contacts to export.[/]");
                _consoleUI.Pause();
                return;
            }

            var path = _reportService.GenerateContactsReport(contacts); 

            AnsiConsole.MarkupLine($"[green]Report saved to:[/] {Path.GetFullPath(path)}");
            _consoleUI.Pause();
        }
    }
}
