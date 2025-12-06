namespace BeneficiariesModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Represents the data required to create a beneficiary for a user.
    /// </summary>
    /// <remarks>This record is used to encapsulate the identifiers of a user and their beneficiary during the
    /// creation process. Both <see cref="UserId"/> and <see cref="BenificiaryId"/> must be provided and cannot be
    /// null.</remarks>
    public record UserBeneficiaryCreationDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBeneficiaryCreationDto"/> class.
        /// </summary>
        public UserBeneficiaryCreationDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBeneficiaryCreationDto"/> class with the specified user ID
        /// and beneficiary ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user associated with the beneficiary.</param>
        /// <param name="benificiaryId">The unique identifier of the beneficiary being created.</param>
        public UserBeneficiaryCreationDto(string userId, string benificiaryId)
        {
            UserId = userId;
            BenificiaryId = benificiaryId;
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public string UserId { get; init; } = null!;

        /// <summary>
        /// Gets the unique identifier of the beneficiary.
        /// </summary>
        public string BenificiaryId { get; init; } = null!;
    }
}
