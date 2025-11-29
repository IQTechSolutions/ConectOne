using System.Globalization;
using ConectOne.Domain.CsvGenerator;
using FilingModule.Blazor.Managers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsModule.Blazor.Pages.Learners
{
    /// <summary>
    /// A Blazor component that provides functionality to:
    /// - Upload CSV files of learners and their parents.
    /// - Parse and display the imported learner data.
    /// - Import learners into the system via an HTTP API.
    /// - Upload and process another CSV for learner grades, and check for existing learners by name.
    /// </summary>
    public partial class ImportedLearners
    {
        /// <summary>
        /// Gets or sets the service used to query learner information.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to execute learner-related commands.
        /// </summary>
        [Inject] public ILearnerCommandService LearnerCommandService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service used to export learner data.
        /// </summary>
        [Inject] public ILearnerExportService LearnerExportService { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Blazor download file manager, which handles file downloads.
        /// </summary>
        [Inject] public IBlazorDownloadFileManager BlazorDownloadFileManager { get; set; } = null!;

        /// <summary>
        /// Injected dialog service for displaying dialogs.
        /// </summary>
        [Inject] public IDialogService DialogService { get; set; } = null!;

        /// <summary>
        /// Injected snack bar service for displaying notifications.
        /// </summary>
        [Inject] public ISnackbar SnackBar { get; set; } = null!;

        /// <summary>
        /// Injected JavaScript runtime for invoking JavaScript functions.
        /// </summary>
        [Inject] public IJSRuntime JsRuntime { get; set; } = null!;

        /// <summary>
        /// Injected navigation manager for navigating between pages.
        /// </summary>
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private List<ImportLearnerDto> _learners = new();          // Holds imported learners from the main CSV.
        private List<ImportLearnerGradeDto> _learnerGrades = new(); // Holds imported learners from the grade CSV.

        private List<ImportLearnerGradeDto> _unallocatedLearners = new(); // Holds learners not matched by name/surname check.
        private List<ImportLearnerGradeDto> _allocatedLearners = new(); // Holds learners not matched by name/surname check.

        private string _checkLearnerButtonText = "Check By Name"; // Button text that updates after checking learners.

        // Breadcrumb navigation items to show user their location in the app.
        private readonly List<BreadcrumbItem> _items =
        [
            new("Dashboard", href: "/dashboard", icon: Icons.Material.Filled.Dashboard),
            new("Learners", href: "/learners", icon: Icons.Material.Filled.People),
            new BreadcrumbItem("Import Learners", href: null, disabled: true, icon: Icons.Material.Filled.ImportContacts)
        ];

        /// <summary>
        /// Handles the uploading and parsing of the main CSV file for learners.
        /// Extracts learners and their parents from the CSV and populates the _learners list.
        /// </summary>
        /// <param name="file">The CSV file selected by the user.</param>
        private async Task UploadFiles(IBrowserFile? file)
        {
            try
            {
                if (file == null)
                {
                    SnackBar.Add("Please select a file to upload.", Severity.Error);
                    return;
                }

                // Save the uploaded file to a temporary file path
                var tempFilePath = Path.GetTempFileName();
                await using (var stream = file.OpenReadStream())
                await using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                };

                var csvReader = new CsvReader<ImportNewLearnerTemplateRecord>();
                var records = csvReader.Read(tempFilePath, config);

                _learners.Clear();

                // Process each record, grouping parents under their respective learners
                foreach (var record in records)
                {
                    var parent = new ImportParentDto
                    {
                        Id = string.IsNullOrEmpty(record.ParentIdNumber) ? Guid.NewGuid().ToString() : record.ParentIdNumber,
                        Name = record.ParentName,
                        Surname = record.ParentSurname,
                        MobileNo = record.ParentMobileNo,
                        Email = record.ParentEmail,
                        IdNumber = record.ParentIdNumber,
                        AddressLine1 = record.ParentAddressLine1
                    };

                    var learner = _learners.Find(l => l.IdNumber == record.LearnerIdPassportNumber);

                    if (learner != null)
                    {
                        learner.Parents.Add(parent);
                    }
                    else
                    {
                        // Create a new learner entry if not already present
                        learner = new ImportLearnerDto
                        {
                            Id = record.LearnerIdPassportNumber,
                            Name = record.LearnerName,
                            Surname = record.LearnerSurname,
                            IdNumber = record.LearnerIdPassportNumber,
                            MobileNo = "", // Not provided in CSV
                            Email = "",    // Not provided in CSV
                            AddressLine1 = "", // Not provided in CSV
                            Grade = record.Grade,
                            Class = record.Class,
                            Parents = new List<ImportParentDto> { parent }
                        };
                        _learners.Add(learner);
                    }
                }

                SnackBar.Add($"Successfully imported {_learners.Count} learners.", Severity.Success);
            }
            catch (Exception ex)
            {
                SnackBar.Add(ex.Message, Severity.Error);
            }
        }

        /// <summary>
        /// Handles the uploading and parsing of a separate CSV file containing learner grades.
        /// Parses them into _learnerGrades for further processing.
        /// </summary>
        /// <param name="file">The CSV file selected by the user.</param>
        private async Task UploadGrades(IBrowserFile? file)
        {
            try
            {
                if (file == null)
                {
                    SnackBar.Add("Please select a file to upload.", Severity.Error);
                    return;
                }

                var tempFilePath = Path.GetTempFileName();
                await using (var stream = file.OpenReadStream())
                await using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                };

                var csvReader = new CsvReader<ImportLearnerGradeTemplateModel>();
                var records = csvReader.Read(tempFilePath, config);

                _learnerGrades.Clear();

                // Populate _learnerGrades from the CSV
                foreach (var record in records)
                {
                    var learnerGrade = new ImportLearnerGradeDto()
                    {
                        ImportNr = record.ImportNr,
                        AccessionNr = record.AccessionNr,
                        Name = record.Name,
                        Surname = record.Surname,
                        Gender = record.Gender,
                        Class = record.Class,
                        Grade = record.Grade,
                        BirthDate = record.BirthDate,
                        IDNumber = record.IDNumber
                    };

                    _learnerGrades.Add(learnerGrade);
                }

                SnackBar.Add($"Successfully imported {_learnerGrades.Count} learners.", Severity.Success);
            }
            catch (Exception ex)
            {
                SnackBar.Add(ex.Message, Severity.Error);
            }
        }

        /// <summary>
        /// Sends the _learners list to the server for importing into the system's database.
        /// Uses the "learners/import/new" endpoint.
        /// </summary>
        private async Task Import()
        {
            var result = await LearnerExportService.ImportNewLearnersAndParents(new SaveImportedLearnersRequest(_learners));
            if (result.Succeeded)
                SnackBar.Add("Successfully imported learners.", Severity.Success);
            else
                SnackBar.Add("Failed to import learners.", Severity.Error);
        }

        /// <summary>
        /// Sends the _learners list to the server for importing learner grades.
        /// Uses the "learners/import/grades" endpoint.
        /// </summary>
        private async Task ImportLearnerGradesList()
        {
            var result = await LearnerExportService.ImportNewLearnersAndParents(new SaveImportedLearnersRequest(_learners));
            if (result.Succeeded)
                SnackBar.Add("Successfully imported learner grades.", Severity.Success);
            else
                SnackBar.Add("Failed to import learner grades.", Severity.Error);
        }

        /// <summary>
        /// Checks if each learner in _learnerGrades exists in the system by comparing their name and surname.
        /// Colors matched learners and identifies unallocated learners.
        /// </summary>
        private async Task CheckLearnerCountByNameAndSurname()
        {
            var allLearnerCount = 0;
            var allLearnersResult = await LearnerQueryService.AllLearnersAsync(new LearnerPageParameters());
            if (allLearnersResult.Succeeded)
            {
                foreach (var tt in _learnerGrades)
                {
                    if (allLearnersResult.Data.Any(g => g.FirstName.ToLower().Contains(tt.Name.ToLower()) && g.LastName.ToLower() == tt.Surname.ToLower()))
                    {
                        _learnerGrades.FirstOrDefault(c => c.ImportNr == tt.ImportNr).Color = "#49cc90";
                        _allocatedLearners.Add(tt);
                        allLearnerCount++;
                    }
                    else
                    {
                        // Unmatched learners are considered unallocated
                        _unallocatedLearners.Add(tt);
                    }
                }

                if (_learnerGrades.Count != allLearnerCount)
                    SnackBar.Add("Some learners were not accounted for.", Severity.Error);

                _checkLearnerButtonText = $"{allLearnerCount} learners available";
            }
            StateHasChanged();
        }

        /// <summary>
        /// Checks the list of learners against their ID numbers to determine allocation status.
        /// </summary>
        /// <remarks>This method retrieves all learners from the server and compares their ID numbers 
        /// with the local list of learner grades. Learners with matching ID numbers are marked  as allocated, while
        /// unmatched learners are added to the unallocated list. The method  also updates the UI state to reflect the
        /// results of the allocation check.</remarks>
        /// <returns></returns>
        private async Task CheckLearnersByIdNumber()
        {
            var allLearnerCount = 0;
            var allLearnersResult = await LearnerQueryService.AllLearnersAsync(new LearnerPageParameters());
            if (allLearnersResult.Succeeded)
            {
                foreach (var tt in _learnerGrades)
                {
                    if (allLearnersResult.Data.Any(g => g.IdNumber == tt.IDNumber))
                    {
                        _learnerGrades.FirstOrDefault(c => c.ImportNr == tt.ImportNr).Color = "#49cc90";
                        _allocatedLearners.Add(tt);
                        allLearnerCount++;
                    }
                    else
                    {
                        // Unmatched learners are considered unallocated
                        _unallocatedLearners.Add(tt);
                    }
                }

                if (_learnerGrades.Count != allLearnerCount)
                    SnackBar.Add("Some learners were not accounted for.", Severity.Error);

                _checkLearnerButtonText = $"{allLearnerCount} learners available";
            }
            StateHasChanged();
        }

        /// <summary>
        /// Imports learner grades by sending the allocated learners' data to the server.
        /// </summary>
        /// <remarks>This method sends a request to import grades for the allocated learners.  If the
        /// operation succeeds, a success message is displayed; otherwise, an error message is shown.</remarks>
        /// <returns></returns>
        private async Task ImportGrades()
        {
            var result = await LearnerExportService.ImportNewLearnersGradesById(new SaveImportedLearnerGradesRequest(_allocatedLearners));
            if (result.Succeeded)
                SnackBar.Add("Successfully imported learners.", Severity.Success);
            else
                SnackBar.Add("Failed to import learners.", Severity.Error);
        }

        /// <summary>
        /// Toggles the visibility of parent details for a given learner in the UI.
        /// </summary>
        /// <param name="learner">The learner whose parent details are toggled.</param>
        private void ShowParentDetails(ImportLearnerDto learner)
        {
            learner.ShowParentDetails = !learner.ShowParentDetails;
            StateHasChanged();
        }

        /// <summary>
        /// Initiates the download of a template file for importing learners.
        /// Uses JavaScript interop to prompt the browser to download the file.
        /// </summary>
        private async Task DownloadTemplate()
        {
            var templateUrl = Navigation.ToAbsoluteUri("/ImportNewLearnerTemplate.xlsx");
            await JsRuntime.InvokeVoidAsync("downloadFileFromUrl", "ImportNewLearnerTemplate.xlsx", templateUrl.AbsoluteUri);
        }

        
    }
}