using ConectOne.Domain.Entities;
using ConectOne.EntityFrameworkCore.Sql;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using GroupingModule.Domain.Entities;
using SchoolsModule.Domain.Entities;
using SchoolsModule.Domain.Interfaces;

namespace SchoolsModule.Infrastructure.Implementation
{
    /// <summary>
    /// Manages repositories for various entities in the SchoolsModule.
    /// </summary>
    public class SchoolsModuleRepoManager(GenericDbContextFactory dbFactory) : ISchoolsModuleRepoManager
    {
        // Lazy initialization of repositories for different entities
        private readonly Lazy<IRepository<Teacher, string>> _teachers = new(() => new Repository<Teacher, string>(dbFactory));
        private readonly Lazy<IRepository<Address<Teacher>, string>> _teacherAddresses = new(() => new Repository<Address<Teacher>, string>(dbFactory));
        private readonly Lazy<IRepository<Parent, string>> _parents = new(() => new Repository<Parent, string>(dbFactory));
        private readonly Lazy<IRepository<ParentEmergencyContact, string>> _parentEmergencyContacts = new(() => new Repository<ParentEmergencyContact, string>(dbFactory));
        private readonly Lazy<IRepository<LearnerParent, string>> _learnerParents = new(() => new Repository<LearnerParent, string>(dbFactory));
        private readonly Lazy<IRepository<Learner, string>> _learners = new(() => new Repository<Learner, string>(dbFactory));
        private readonly Lazy<IRepository<SchoolGrade, string>> _schoolGrades = new(() => new Repository<SchoolGrade, string>(dbFactory));
        private readonly Lazy<IRepository<AgeGroup, string>> _ageGroups = new(() => new Repository<AgeGroup, string>(dbFactory));
        private readonly Lazy<IRepository<SchoolClass, string>> _schoolClasses = new(() => new Repository<SchoolClass, string>(dbFactory));
        private readonly Lazy<IRepository<ActivityGroup, string>> _activityGroups = new(() => new Repository<ActivityGroup, string>(dbFactory));
        private readonly Lazy<IRepository<ActivityGroupTeamMember, string>> _activityGroupTeamMembers = new(() => new Repository<ActivityGroupTeamMember, string>(dbFactory));
        private readonly Lazy<IRepository<SchoolEvent<Category<ActivityGroup>>, string>> _schoolEvents = new(() => new Repository<SchoolEvent<Category<ActivityGroup>>, string>(dbFactory));
        private readonly Lazy<IRepository<SchoolEventViews, string>> _schoolEventViews = new(() => new Repository<SchoolEventViews, string>(dbFactory));
        private readonly Lazy<IRepository<ParticipatingActivityGroupCategory, string>> _participatingActivityGroupCategories = new(() => new Repository<ParticipatingActivityGroupCategory, string>(dbFactory));
        private readonly Lazy<IRepository<ParticipatingActivityGroup, string>> _participatingActivityGroups = new(() => new Repository<ParticipatingActivityGroup, string>(dbFactory));
        private readonly Lazy<IRepository<ParticipatingActitivityGroupTeamMember, string>> _participatingActivityGroupTeamMembers = new(() => new Repository<ParticipatingActitivityGroupTeamMember, string>(dbFactory));
        private readonly Lazy<IRepository<ParentPermission, string>> _parentPermissions = new(() => new Repository<ParentPermission, string>(dbFactory));
        private readonly Lazy<IRepository<AttendanceRecord, string>> _attendanceRecords = new(() => new Repository<AttendanceRecord, string>(dbFactory));
        private readonly Lazy<IRepository<AttendanceGroup, string>> _attendanceGroupRecords = new(() => new Repository<AttendanceGroup, string>(dbFactory));
        private readonly Lazy<IRepository<SeverityScale, string>> _severityScales = new(() => new Repository<SeverityScale, string>(dbFactory));
        private readonly Lazy<IRepository<DisciplinaryAction, string>> _disciplinaryActions = new(() => new Repository<DisciplinaryAction, string>(dbFactory));
        private readonly Lazy<IRepository<DisciplinaryIncident, string>> _disciplinaryIncidents = new(() => new Repository<DisciplinaryIncident, string>(dbFactory));

        /// <summary>
        /// Gets the repository for Teacher entities.
        /// </summary>
        public IRepository<Teacher, string> Teachers => _teachers.Value;

        /// <summary>
        /// Gets the repository for Teacher Address entities.
        /// </summary>
        public IRepository<Address<Teacher>, string> TeacherAddresses => _teacherAddresses.Value;

        /// <summary>
        /// Gets the repository for Parent entities.
        /// </summary>
        public IRepository<Parent, string> Parents => _parents.Value;

        /// <summary>
        /// Gets the repository for parent emergency contact entities.
        /// </summary>
        public IRepository<ParentEmergencyContact, string> ParentEmergencyContacts => _parentEmergencyContacts.Value;

        /// <summary>
        /// Gets the repository for LearnerParent entities.
        /// </summary>
        public IRepository<LearnerParent, string> LearnerParents => _learnerParents.Value;

        /// <summary>
        /// Gets the repository for Learner entities.
        /// </summary>
        public IRepository<Learner, string> Learners => _learners.Value;

        /// <summary>
        /// Gets the repository for SchoolGrade entities.
        /// </summary>
        public IRepository<SchoolGrade, string> SchoolGrades => _schoolGrades.Value;

        /// <summary>
        /// Gets the repository for AgeGroup entities.
        /// </summary>
        public IRepository<AgeGroup, string> AgeGroups => _ageGroups.Value;

        /// <summary>
        /// Gets the repository for SchoolClass entities.
        /// </summary>
        public IRepository<SchoolClass, string> SchoolClasses => _schoolClasses.Value;

        /// <summary>
        /// Gets the repository for ActivityGroup entities.
        /// </summary>
        public IRepository<ActivityGroup, string> ActivityGroups => _activityGroups.Value;

        /// <summary>
        /// Gets the repository for ActivityGroupTeamMember entities.
        /// </summary>
        public IRepository<ActivityGroupTeamMember, string> ActivityGroupTeamMembers => _activityGroupTeamMembers.Value;

        /// <summary>
        /// Gets the repository for SchoolEvent entities.
        /// </summary>
        public IRepository<SchoolEvent<Category<ActivityGroup>>, string> SchoolEvents => _schoolEvents.Value;

        /// <summary>
        /// Gets the repository for SchoolEvent View entities.
        /// </summary>
        public IRepository<SchoolEventViews, string> SchoolEventViews => _schoolEventViews.Value;

        /// <summary>
        /// Gets the repository for ParticipatingActivityGroupCategory entities.
        /// </summary>
        public IRepository<ParticipatingActivityGroupCategory, string> ParticipatingActivityGroupCategories => _participatingActivityGroupCategories.Value;

        /// <summary>
        /// Gets the repository for ParticipatingActivityGroup entities.
        /// </summary>
        public IRepository<ParticipatingActivityGroup, string> ParticipatingActivityGroups => _participatingActivityGroups.Value;

        /// <summary>
        /// Gets the repository for ParticipatingActivityGroupTeamMember entities.
        /// </summary>
        public IRepository<ParticipatingActitivityGroupTeamMember, string> ParticipatingActivityGroupTeamMembers => _participatingActivityGroupTeamMembers.Value;

        /// <summary>
        /// Gets the repository for ParentPermission entities.
        /// </summary>
        public IRepository<ParentPermission, string> ParentPermissions => _parentPermissions.Value;

        /// <summary>
        /// Gets the repository for managing <see cref="SeverityScale"/> entities.
        /// </summary>
        /// <remarks>Use this property to perform CRUD operations on <see cref="SeverityScale"/> entities.
        /// The repository is lazily initialized.</remarks>
        public IRepository<SeverityScale, string> SeverityScales => _severityScales.Value;

        /// <summary>
        /// Gets the repository for managing disciplinary actions.
        /// </summary>
        public IRepository<DisciplinaryAction, string> DisciplinaryActions => _disciplinaryActions.Value;

        /// <summary>
        /// Gets the repository for managing disciplinary incidents.
        /// </summary>
        public IRepository<DisciplinaryIncident, string> DisciplinaryIncidents => _disciplinaryIncidents.Value;

        /// <summary>
        /// Gets the repository for managing attendance records.
        /// </summary>
        /// <remarks>Use this property to perform CRUD operations on attendance records. The repository
        /// provides methods for querying, adding, updating, and deleting attendance data.</remarks>
        public IRepository<AttendanceRecord, string> AttendanceRecords => _attendanceRecords.Value;

        /// <summary>
        /// Gets the repository for managing attendance groups.
        /// </summary>
        public IRepository<AttendanceGroup, string> AttendanceGroups => _attendanceGroupRecords.Value;
    }
}

