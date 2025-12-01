using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SchoolsModule.Domain.DataTransferObjects;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.RequestFeatures;

namespace SchoolsEnterprise.Blazor.Shared.Maui.Components.SchoolClasses
{
    /// <summary>
    /// The SchoolClassAndLearnerInfoListComponent component displays information about a specific school class
    /// and the learners associated with that class. It fetches the data from the server and displays it in the UI.
    /// </summary>
    public partial class SchoolClassAndLearnerInfoListComponent
    {
        #region Injected Services

        /// <summary>
        /// Gets or sets the service used to manage school class data.
        /// </summary>
        [Inject] public ISchoolClassService SchoolClassService { get; set; }

        /// <summary>
        /// Gets or sets the service used to query learner information.
        /// </summary>
        [Inject] public ILearnerQueryService LearnerQueryService { get; set; }

        #endregion

        #region Parameters

        /// <summary>
        /// Gets or sets the ID of the school class to display.
        /// </summary>
        [Parameter, EditorRequired] public string SchoolClassId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the parent entity associated with the learners.
        /// </summary>
        [Parameter, EditorRequired] public string ParentId { get; set; } = null!;

        #endregion

        #region Private Fields

        /// <summary>
        /// Holds the school class data.
        /// </summary>
        private SchoolClassDto? _schoolClass;

        /// <summary>
        /// Holds the list of learners associated with the school class.
        /// </summary>
        private IEnumerable<LearnerDto> _learners = Enumerable.Empty<LearnerDto>();

        /// <summary>
        /// Provides localized strings for the class and learner labels.
        /// </summary>
        [Inject]
        public IStringLocalizer<SchoolClassAndLearnerInfoListComponent> Localizer { get; set; } = null!;

        /// <summary>
        /// Gets the text representation of the school class and learners.
        /// </summary>
        private string Text
        {
            get
            {
                if (_schoolClass is null)
                {
                    return string.Empty;
                }

                var text = Localizer["ClassWithName", _schoolClass.SchoolClass].Value;

                if (_learners.Any())
                {
                    var learnerNames = string.Join(", ", _learners);
                    var learnerKey = _learners.Count() > 1 ? "LearnersPlural" : "LearnerSingular";
                    text += Localizer[learnerKey, learnerNames].Value;
                }

                return text;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes after the component has rendered and handles the initial data load on the first render.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first time the component is rendered.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var schoolClassResult = await SchoolClassService.SchoolClassAsync(SchoolClassId);
                if (schoolClassResult.Succeeded)
                {
                    _schoolClass = schoolClassResult.Data;

                    var learnerResult = await LearnerQueryService.PagedLearnersAsync(new LearnerPageParameters() { SchoolClassId = SchoolClassId, ParentId = ParentId });
                    if (learnerResult.Succeeded)
                    {
                        _learners = learnerResult.Data;
                    }
                }

                StateHasChanged();
            }
        }

        #endregion
    }
}
