using System.ComponentModel.DataAnnotations.Schema;
using ConectOne.Domain.Entities;

namespace IdentityModule.Domain.Entities
{
    /// <summary>
    /// Represents a relationship where one user follows another user.
    /// </summary>
    /// <remarks>This class models the association between a user who is following another user  and the user
    /// being followed. It includes references to both users and their respective identifiers.</remarks>
    public class Follower : EntityBase<string>
    {
        /// <summary>
        /// Gets or sets the identifier of the user being followed.
        /// </summary>
        [ForeignKey(nameof(UserFollowing))] public string UserFollowingId { get; set; }

        /// <summary>
        /// Gets or sets the user being followed.
        /// </summary>
        public ApplicationUser UserFollowing { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user being followed.
        /// </summary>
        public string UserBeingFollowedId { get; set; } 

        /// <summary>
        /// Gets the user who is being followed.
        /// </summary>
        public ApplicationUser UserBeingFollowed { get;}
    }
}
