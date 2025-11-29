using ConectOne.Domain.Entities;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using GroupingModule.Domain.Entities;
using SchoolsModule.Domain.Entities;

namespace SchoolsModule.Domain.Interfaces
{
    /// <summary>
    /// Interface for managing repositories related to the Schools Module.
    /// Provides access to various repositories for managing entities such as Teachers, Parents, Learners, School Classes, etc.
    /// </summary>
    public interface ISchoolsModuleRepoManager
    {
        /// <summary>
        /// Gets the repository for managing Teacher entities.
        /// </summary>
        IRepository<Teacher, string> Teachers { get; }

        /// <summary>
        /// Gets the repository for managing Teacher Address entities.
        /// </summary>
        IRepository<Address<Teacher>, string> TeacherAddresses { get; }

        /// <summary>
        /// Gets the repository for managing Parent entities.
        /// </summary>
        IRepository<Parent, string> Parents { get; }

        /// <summary>
        /// Gets the repository for managing parent emergency contacts.
        /// </summary>
        IRepository<ParentEmergencyContact, string> ParentEmergencyContacts { get; }

        /// <summary>
        /// Gets the repository for managing Learner-Parent relationship entities.
        /// </summary>
        IRepository<LearnerParent, string> LearnerParents { get; }

        /// <summary>
        /// Gets the repository for managing Learner entities.
        /// </summary>
        IRepository<Learner, string> Learners { get; }

        /// <summary>
        /// Gets the repository for managing School Grade entities.
        /// </summary>
        IRepository<SchoolGrade, string> SchoolGrades { get; }

        /// <summary>
        /// Gets the repository for managing Age Group entities.
        /// </summary>
        IRepository<AgeGroup, string> AgeGroups { get; }

        /// <summary>
        /// Gets the repository for managing School Class entities.
        /// </summary>
        IRepository<SchoolClass, string> SchoolClasses { get; }

        /// <summary>
        /// Gets the repository for managing Activity Group entities.
        /// </summary>
        IRepository<ActivityGroup, string> ActivityGroups { get; }

        /// <summary>
        /// Gets the repository for managing Activity Group Team Member entities.
        /// </summary>
        IRepository<ActivityGroupTeamMember, string> ActivityGroupTeamMembers { get; }

        /// <summary>
        /// Gets the repository for managing School Event entities.
        /// </summary>
        IRepository<SchoolEvent<Category<ActivityGroup>>, string> SchoolEvents { get; }

        /// <summary>
        /// Gets the repository for managing School Event View entities.
        /// </summary>
        IRepository<SchoolEventViews, string> SchoolEventViews { get; }

        /// <summary>
        /// Gets the repository for managing Participating Activity Group Category entities.
        /// </summary>
        IRepository<ParticipatingActivityGroupCategory, string> ParticipatingActivityGroupCategories { get; }

        /// <summary>
        /// Gets the repository for managing Participating Activity Group entities.
        /// </summary>
        IRepository<ParticipatingActivityGroup, string> ParticipatingActivityGroups { get; }

        /// <summary>
        /// Gets the repository for managing Participating Activity Group Team Member entities.
        /// </summary>
        IRepository<ParticipatingActitivityGroupTeamMember, string> ParticipatingActivityGroupTeamMembers { get; }

        /// <summary>
        /// Gets the repository for managing Parent Permission entities.
        /// </summary>
        IRepository<ParentPermission, string> ParentPermissions { get; }

        /// <summary>Repository for <see cref="SeverityScale"/> entities.</summary>
        IRepository<SeverityScale, string> SeverityScales { get; }

        /// <summary>Repository for <see cref="DisciplinaryAction"/> entities.</summary>
        IRepository<DisciplinaryAction, string> DisciplinaryActions { get; }

        /// <summary>Repository for <see cref="DisciplinaryIncident"/> entities.</summary>
        IRepository<DisciplinaryIncident, string> DisciplinaryIncidents { get; }

        /// <summary>
        /// Gets the repository for managing attendance records.
        /// </summary>
        IRepository<AttendanceRecord, string> AttendanceRecords { get; }

        /// <summary>
        /// Gets the repository for managing <see cref="AttendanceGroup"/> entities.
        /// </summary>
        IRepository<AttendanceGroup, string> AttendanceGroups { get; }
    }
}