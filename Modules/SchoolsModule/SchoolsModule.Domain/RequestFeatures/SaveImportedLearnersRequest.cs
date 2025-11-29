using SchoolsModule.Domain.DataTransferObjects;

namespace SchoolsModule.Domain.RequestFeatures
{
    /// <summary>
    /// Represents a request to save imported learners.
    /// </summary>
    public record class SaveImportedLearnersRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveImportedLearnersRequest"/> class.
        /// </summary>
        public SaveImportedLearnersRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveImportedLearnersRequest"/> class with specified learners.
        /// </summary>
        /// <param name="learners">The list of learners to be saved.</param>
        public SaveImportedLearnersRequest(List<ImportLearnerDto> learners)
        {
            Learners = learners;
        }

        /// <summary>
        /// Gets the list of learners to be saved.
        /// </summary>
        public List<ImportLearnerDto> Learners { get; init; } = [];
    }
}