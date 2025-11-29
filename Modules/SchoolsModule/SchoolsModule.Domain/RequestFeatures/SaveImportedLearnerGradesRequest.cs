using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to save imported learners.
    /// </summary>
    public record class SaveImportedLearnerGradesRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveImportedLearnerGradesRequest"/> class.
        /// </summary>
        public SaveImportedLearnerGradesRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveImportedLearnerGradesRequest"/> class with specified learners.
        /// </summary>
        /// <param name="learners">The list of learners to be saved.</param>
        public SaveImportedLearnerGradesRequest(List<ImportLearnerGradeDto> learners)
        {
            Learners = learners;
        }

        /// <summary>
        /// Gets the list of learners to be saved.
        /// </summary>
        public List<ImportLearnerGradeDto> Learners { get; init; } = [];
    }
}