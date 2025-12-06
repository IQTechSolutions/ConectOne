using System.ComponentModel;
using System.Text;
using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the policies and rules associated with a lodging, including age cutoffs, deposit policies,  terms and
    /// conditions, and cancellation rules.
    /// </summary>
    /// <remarks>This view model is designed to encapsulate various policies related to lodging
    /// accommodations,  such as child age cutoffs, deposit requirements, and cancellation rules. It can be initialized 
    /// using data from a <see cref="LodgingDto"/> or <see cref="ContentResponse"/> object.</remarks>
    public class LodgingPoliciesViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPoliciesViewModel"/> class.
        /// </summary>
        public LodgingPoliciesViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LodgingPoliciesViewModel"/> class using the specified lodging
        /// data.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="LodgingDto"/> object
        /// to the corresponding properties of the <see cref="LodgingPoliciesViewModel"/> instance. It also transforms
        /// the cancellation rules into a collection of <see cref="CancellationRuleViewModel"/> objects.</remarks>
        /// <param name="lodging">The lodging data used to populate the policies and rules for this view model. This parameter must not be
        /// <c>null</c>.</param>
        public LodgingPoliciesViewModel(LodgingDto lodging)
        {
            ChildPolicy = lodging.ChildPolicy;
            LowestGuestAgeCutOff = lodging.LowestGuestAgeCutOff;
            MiddleGuestAgeCutOff = lodging.MiddleGuestAgeCutOff;
            HighestGuestAgeCutOff = lodging.HighestGuestAgeCutOff;
            DepositPolicy = lodging.DepositPolicy;
            TermsAndConditions = lodging.TermsAndConditions;
            CancellationRules = lodging.CancellationRules is not null ? lodging.CancellationRules.Select(c => new CancellationRuleViewModel(c)).ToList() : [];
        }

        #endregion

        /// <summary>
        /// Gets or sets the minimum age cutoff for a guest to be considered a child.
        /// </summary>
        [DisplayName("First Child Age Cut Off")] public int LowestGuestAgeCutOff { get; set; }

        /// <summary>
        /// Gets or sets the age cutoff for determining the middle guest category.
        /// </summary>
        [DisplayName("Second Child Age Cut Off")] public int MiddleGuestAgeCutOff { get; set; }

        /// <summary>
        /// Gets or sets the maximum age cutoff for a guest to be considered a child. Guests older than this age are
        /// classified as adults.
        /// </summary>
        [DisplayName("Adult Staring Age")] public int HighestGuestAgeCutOff { get; set; }

        /// <summary>
        /// Gets or sets the policy applied to child entities.
        /// </summary>
        [DisplayName("Child Policy")] public string ChildPolicy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the deposit policy associated with the entity.
        /// </summary>
        [DisplayName("Deposit Policy")] public string? DepositPolicy { get; set; } 

        /// <summary>
        /// Gets or sets the terms and conditions associated with the entity.
        /// </summary>
        [DisplayName("Terms & Conditions")] public string? TermsAndConditions { get; set; }

        /// <summary>
        /// Gets the description of the cancellation policy based on the defined cancellation rules.
        /// </summary>
        [DisplayName("Cancellation Policy")] public string CancellationPolicyDescription {
            get
            {
                if (CancellationRules.Count==0)
                    return string.Empty;

                var returnString = new StringBuilder();

                foreach (var rule in CancellationRules.OrderBy(c => c.CancellationDays))
                {
                    returnString.AppendLine($"If cancelling {rule.CancellationDays} days before arrival, forfeit " +
                        $"{rule.Rule.GetCancellationRuleAbbreviation(rule.Ammount)}");
                }

                return returnString.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the collection of cancellation rules.
        /// </summary>
        public List<CancellationRuleViewModel> CancellationRules { get; set; } = [];

        /// <summary>
        /// Updates the current lodging policies with the values provided in the specified view model.
        /// </summary>
        /// <remarks>This method replaces the current lodging policy values, such as age cutoffs, deposit
        /// policy,  and terms and conditions, with those specified in the <paramref name="policies"/> parameter. Ensure
        /// that the <paramref name="policies"/> object contains valid data before calling this method.</remarks>
        /// <param name="policies">A <see cref="LodgingPoliciesViewModel"/> instance containing the updated policy values.</param>
        public void Update(LodgingPoliciesViewModel policies)
        {
            LowestGuestAgeCutOff = policies.LowestGuestAgeCutOff;
            MiddleGuestAgeCutOff = policies.MiddleGuestAgeCutOff;
            HighestGuestAgeCutOff = policies.HighestGuestAgeCutOff;
            DepositPolicy = policies.DepositPolicy;
            TermsAndConditions = policies.TermsAndConditions;
        }
    }
}
