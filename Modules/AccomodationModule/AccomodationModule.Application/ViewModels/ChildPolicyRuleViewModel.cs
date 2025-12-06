using AccomodationModule.Domain.DataTransferObjects;
using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Application.ViewModels
{
    /// <summary>
    /// Represents the view model for child policy rules, including age restrictions, pricing rules, and custom
    /// descriptions.
    /// </summary>
    /// <remarks>This class is typically used to define and manage child policy rules for a specific room,
    /// including whether children  are allowed, applicable age ranges, and pricing adjustments. It provides properties
    /// for configuring these rules and  a computed description summarizing the policy.</remarks>
    public class ChildPolicyRuleViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildPolicyRuleViewModel"/> class.
        /// </summary>
        public ChildPolicyRuleViewModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildPolicyRuleViewModel"/> class using the specified data
        /// transfer object.
        /// </summary>
        /// <remarks>This constructor maps the properties of the provided <see cref="ChildPolicyRuleDto"/>
        /// to the corresponding properties of the <see cref="ChildPolicyRuleViewModel"/> instance.</remarks>
        /// <param name="modal">A <see cref="ChildPolicyRuleDto"/> object containing the data used to initialize the view model.</param>
        public ChildPolicyRuleViewModel(ChildPolicyRuleDto modal)
        {
            ChildPolicyRuleId = modal.ChildPolicyRuleId;
            MinAge = modal.MinAge;
            MaxAge = modal.MaxAge;
            Allowed = modal.Allowed;
            UseSpecialRate = modal.UseSpecialRate;
            CustomDescription = modal.CustomDescription;
            Amount = modal.Ammount;
            Rule = modal.Rule;
            RoomId = modal.RoomId;
        }

        #endregion

        /// <summary>
        /// Gets or sets the identifier of the child policy rule.
        /// </summary>
        public string? ChildPolicyRuleId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for a room.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the minimum age required for eligibility.
        /// </summary>
        public int MinAge { get; set; }

        /// <summary>
        /// Gets or sets the maximum age, in years, allowed for a given operation or entity.
        /// </summary>
        public int MaxAge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation is allowed.
        /// </summary>
        public bool Allowed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a special rate should be applied.
        /// </summary>
        public bool UseSpecialRate { get; set; }

        /// <summary>
        /// Gets or sets a custom description associated with the object.
        /// </summary>
        public string? CustomDescription { get; set; }
        
        /// <summary>
        /// Gets or sets the monetary amount associated with the transaction.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets the rule associated with the current operation or configuration.
        /// </summary>
        public string? Rule { get; set; } = null!;

        /// <summary>
        /// Gets the description of the child policy based on the specified rules, age range, and custom settings.
        /// </summary>
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(CustomDescription))
                {

                    if (Allowed)
                    {
                        if (UseSpecialRate)
                        {
                            return Rule switch
                            {
                                "N" => $"Children ages {MinAge} - {MaxAge} years are not allowed",
                                "R" => $"Children ages {MinAge} - {MaxAge} years gets a fixed ammount of R{Amount} added to the room charge",
                                "P" => $"Children ages {MinAge} - {MaxAge} years pay {Amount}% of price",
                                _ => "No child policy rule implemented",
                            };
                        }
                        else
                        {
                            return $"Children ages {MinAge} - {MaxAge} years pay full rate";
                        }
                    }
                    return $"Children ages {MinAge} - {MaxAge} years are not allowed";
                }
                else
                {
                    return CustomDescription;
                }
            }
        }

        #region Methods

        /// <summary>
        /// Converts the current instance of <see cref="ChildPolicyRule"/> to a <see cref="ChildPolicyRuleDto"/>.
        /// </summary>
        /// <returns>A <see cref="ChildPolicyRuleDto"/> object that represents the current instance,  including all relevant
        /// property values.</returns>
        public ChildPolicyRuleDto ToDto()
        {
            return new ChildPolicyRuleDto
            {
                ChildPolicyRuleId = ChildPolicyRuleId,
                MinAge = MinAge,
                MaxAge = MaxAge,
                Allowed = Allowed,
                UseSpecialRate = UseSpecialRate,
                CustomDescription = CustomDescription,
                Ammount = Amount,
                Rule = Rule,
                RoomId = RoomId
            };
        }

        #endregion
    }
}
