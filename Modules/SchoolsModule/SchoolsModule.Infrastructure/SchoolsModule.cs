using FilingModule.Domain.Implementation;
using FilingModule.Domain.Interfaces;
using FilingModule.Infrastucture.Implementation;
using Microsoft.Extensions.DependencyInjection;
using SchoolsModule.Infrastructure.Implementation;
using SchoolsModule.Infrastructure.Implementation.ActivityGroups;
using SchoolsModule.Infrastructure.Implementation.Learners;
using SchoolsModule.Infrastructure.Implementation.Parents;
using SchoolsModule.Infrastructure.Implementation.SchoolEvents;
using SchoolsModule.Infrastructure.Implementation.Disciplinary;
using MessagingModule.Domain.Interfaces;
using MessagingModule.Infrastructure.Implementation;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Discipline;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Interfaces.Parents;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Infrastructure
{
    /// <summary>
    /// Registers all repository managers, services, and utilities related to the Schools module into the dependency
    /// injection container.
    /// </summary>
    /// <remarks>This method registers a wide range of services, including core managers, authentication
    /// services, domain-specific services (e.g., for teachers, learners, and school events), and utility services
    /// (e.g., for notifications and exports). It is designed to be called during application startup to configure the
    /// Schools module's dependencies.</remarks>
    public static class SchoolsModule
    {
        /// <summary>
        /// Registers all repository managers, services, and utilities related to the Schools module
        /// into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to which the services will be added.</param>
        /// <returns>The updated IServiceCollection instance.</returns>
        public static IServiceCollection AddSchoolsModule(this IServiceCollection services)
        {
            // Core managers
            services.AddScoped<ISchoolsModuleRepoManager, SchoolsModuleRepoManager>();
            services.AddScoped<IExcelService, ExcelService>();

            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<IImageProcessingService, ImageProcessingService>();

            // Domain-specific services
            services.AddScoped<IAgeGroupService, AgeGroupService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ISchoolGradeService, SchoolGradeService>();
            services.AddScoped<ISchoolClassService, SchoolClassService>();

            // Parent services
            services.AddScoped<IParentQueryService, ParentQueryService>();
            services.AddScoped<IParentCommandService, ParentCommandService>();

            // Learner services
            services.AddScoped<ILearnerQueryService, LearnerQueryService>();
            services.AddScoped<ILearnerCommandService, LearnerCommandService>();
            services.AddScoped<ILearnerNotificationService, LearnerNotificationService>();
            services.AddScoped<ILearnerExportService, LearnerExportService>();

            // Activity Group services
            services.AddScoped<IActivityGroupCategoryService, ActivityGroupCategoryService>();
            services.AddScoped<IActivityGroupQueryService, ActivityGroupQueryService>();
            services.AddScoped<IActivityGroupCommandService, ActivityGroupCommandService>();
            services.AddScoped<IActivityGroupNotificationService, ActivityGroupNotificationService>();
            services.AddScoped<IActivityGroupExportService, ActivityGroupExportService>();

            // School Event services
            services.AddScoped<ISchoolEventCategoryService, SchoolEventCategoryService>();
            services.AddScoped<ISchoolEventNotificationService, SchoolEventNotificationService>();
            services.AddScoped<ISchoolEventPermissionService, SchoolEventPermissionService>();
            services.AddScoped<ISchoolEventExportService, SchoolEventExportService>();
            services.AddScoped<ISchoolEventQueryService, SchoolEventQueryService>();
            services.AddScoped<ISchoolEventCommandService, SchoolEventCommandService>();

            // Attendance services
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IAttendanceGroupService, AttendanceGroupService>();

            // Discipline services
            services.AddScoped<IDisciplinaryIncidentService, DisciplinaryIncidentService>();
            services.AddScoped<IDisciplinaryActionService, DisciplinaryActionService>();

            return services;
        }
    }

}
