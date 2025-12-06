using AccomodationModule.Domain.Entities;

namespace AccomodationModule.Domain.DataTransferObjects
{
    /// <summary>
    /// Data transfer object for a child policy rule, used to transfer child policy rule-related data between layers of the application.
    /// </summary>
    public class ChildPolicyRuleDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildPolicyRuleDto"/> class.
        /// </summary>
        public ChildPolicyRuleDto() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildPolicyRuleDto"/> class from a <see cref="ChildPolicyRule"/> entity.
        /// </summary>
        /// <param name="modal">The child policy rule entity.</param>
        public ChildPolicyRuleDto(ChildPolicyRule modal)
        {
            ChildPolicyRuleId = modal.Id;
            MinAge = modal.MinAge;
            MaxAge = modal.MaxAge;
            Allowed = modal.Allowed;
            UseSpecialRate = modal.UseSpecialRate;
            CustomDescription = modal.CustomDescription;
            Ammount = modal.ChildPolicyFormualaValue;
            Rule = modal.ChildPolicyFormualaType;
            RoomId = modal.RoomId == null ? 0 : modal.RoomId.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildPolicyRuleDto"/> class with specified parameters.
        /// </summary>
        /// <param name="roomId">The room ID.</param>
        /// <param name="allowed">Indicates whether the rule is allowed.</param>
        /// <param name="rule">The rule code.</param>
        /// <param name="description">The custom description of the rule.</param>
        /// <param name="specialRate">The special rate amount.</param>
        /// <param name="minAge">The minimum age for the rule.</param>
        /// <param name="maxAge">The maximum age for the rule.</param>
        public ChildPolicyRuleDto(int roomId, bool allowed, string? rule, string description, double specialRate = 0, int minAge = 0, int maxAge = 0)
        {
            ChildPolicyRuleId = Guid.NewGuid().ToString();
            MinAge = minAge;
            MaxAge = maxAge;
            Allowed = allowed;
            UseSpecialRate = specialRate == 0 ? false : true;
            CustomDescription = description;
            Ammount = specialRate;
            Rule = rule;
            RoomId = roomId;
        }

        #endregion

        /// <summary>
        /// Gets or sets the child policy rule ID.
        /// </summary>
        public string? ChildPolicyRuleId { get; init; }

        /// <summary>
        /// Gets or sets the room ID.
        /// </summary>
        public int RoomId { get; init; }

        /// <summary>
        /// Gets or sets the minimum age for the rule.
        /// </summary>
        public int MinAge { get; init; }

        /// <summary>
        /// Gets or sets the maximum age for the rule.
        /// </summary>
        public int MaxAge { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the rule is allowed.
        /// </summary>
        public bool Allowed { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether a special rate is used.
        /// </summary>
        public bool UseSpecialRate { get; init; }

        /// <summary>
        /// Gets or sets the amount for the rule.
        /// </summary>
        public double Ammount { get; init; }

        /// <summary>
        /// Gets or sets the rule code.
        /// </summary>
        public string? Rule { get; init; } = null!;

        /// <summary>
        /// Gets or sets the custom description of the rule.
        /// </summary>
        public string? CustomDescription { get; init; }

        /// <summary>
        /// Gets or sets the custom description of the rule.
        /// </summary>
        public string DescriptionAlt
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
                                "R" => $"Children ages {MinAge} - {MaxAge} years pay a fixed ammount of R{Ammount}",
                                "P" => $"Children ages {MinAge} - {MaxAge} years pay {Ammount}% of price",
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

        /// <summary>
        /// Converts the current instance to a <see cref="ChildPolicyRule"/> entity.
        /// </summary>
        /// <returns>A new <see cref="ChildPolicyRule"/> entity.</returns>
        public ChildPolicyRule ToChildPolicyRule()
        {
            return new ChildPolicyRule()
            {
                Id = string.IsNullOrEmpty(this.ChildPolicyRuleId) ? Guid.NewGuid().ToString() : this.ChildPolicyRuleId,
                MinAge = this.MinAge,
                MaxAge = this.MaxAge,
                Allowed = this.Allowed,
                UseSpecialRate = this.UseSpecialRate,
                CustomDescription = this.CustomDescription,
                ChildPolicyFormualaType = this.Rule,
                ChildPolicyFormualaValue = this.Ammount,
                RoomId = this.RoomId
            };
        }
    }
}
