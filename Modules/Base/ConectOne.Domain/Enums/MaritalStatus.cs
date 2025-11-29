namespace ConectOne.Domain.Enums
{
    /// <summary>
    /// Specifies the marital status of an individual.
    /// </summary>
    /// <remarks>Use this enumeration to represent a person's marital status in applications such as user
    /// profiles, demographic data, or forms. The values include common statuses such as single, married, engaged,
    /// divorced, and widowed. The Unknown value indicates that the marital status is not specified or cannot be
    /// determined.</remarks>
    public enum MaritalStatus
    {
        Unknown = 0,
        Single = 1,
        Married = 2,
        Engaged = 3,
        Divorced = 4,
        Widowed = 5
    }
}