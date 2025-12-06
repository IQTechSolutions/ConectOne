using System.ComponentModel;

namespace BeneficiariesModule.Domain.Enums
{
    /// <summary>
    /// Represents the status of a beneficiary in a system.
    /// </summary>
    /// <remarks>This enumeration is used to indicate the current state of a beneficiary. The possible values
    /// are: <list type="bullet"> <item><term><see cref="Inactive"/></term><description>The beneficiary is not active in
    /// the system.</description></item> <item><term><see cref="Pending"/></term><description>The beneficiary is
    /// awaiting approval or activation.</description></item> <item><term><see cref="Active"/></term><description>The
    /// beneficiary is fully active and operational in the system.</description></item> </list></remarks>
    public enum BenificiaryStatus
    {
        /// <summary>
        /// Represents the inactive state in the enumeration.
        /// </summary>
        /// <remarks>This value is typically used to indicate that an entity or process is not currently
        /// active.</remarks>
        [Description("Inactive")] Inactive = 0,

        /// <summary>
        /// Represents a pending state in the workflow or process.
        /// </summary>
        /// <remarks>This value is typically used to indicate that an operation or task is awaiting
        /// completion or further action.</remarks>
        [Description("Pending")] Pending = 1,

        /// <summary>
        /// Represents the active state of an entity.
        /// </summary>
        /// <remarks>This value is typically used to indicate that the entity is currently active or
        /// enabled.</remarks>
        [Description("Active")] Active = 2
    }
}
