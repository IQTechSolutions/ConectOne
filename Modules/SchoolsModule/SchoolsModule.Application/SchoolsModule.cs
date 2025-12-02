using FilingModule.Application;
using Microsoft.Extensions.DependencyInjection;
using SchoolsModule.Application.RestServices;
using SchoolsModule.Application.RestServices.ActivityGroups;
using SchoolsModule.Application.RestServices.Disciplinary;
using SchoolsModule.Application.RestServices.Learners;
using SchoolsModule.Application.RestServices.SchoolEvents;
using SchoolsModule.Domain.Interfaces;
using SchoolsModule.Domain.Interfaces.ActivityGroups;
using SchoolsModule.Domain.Interfaces.Discipline;
using SchoolsModule.Domain.Interfaces.Learners;
using SchoolsModule.Domain.Interfaces.SchoolEvents;

namespace SchoolsModule.Application
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
        public static IServiceCollection AddSchoolsModule(this IServiceCollection services, string baseAddress)
        {
            // Core managers
            services.AddFilingModule();

            // Domain-specific services
            services.AddScoped<IAgeGroupService, AgeGroupRestService>();
            services.AddScoped<ITeacherService, TeacherRestService>();
            services.AddScoped<ISchoolGradeService, SchoolGradeRestService>();
            services.AddScoped<ISchoolClassService, SchoolClassRestService>();

            // Parent services
            services.AddScoped<IParentService, ParentRestService>();

            // Learner services
            services.AddScoped<ILearnerQueryService, LearnerQueryRestService>();
            services.AddScoped<ILearnerCommandService, LearnerCommandRestService>();
            services.AddScoped<ILearnerNotificationService, LearnerNotificationRestService>();
            services.AddScoped<ILearnerExportService, LearnerExportRestService>();

            // Activity Group services
            services.AddScoped<IActivityGroupCategoryService, ActivityGroupCategoryRestService>();
            services.AddScoped<IActivityGroupQueryService, ActivityGroupQueryRestService>();
            services.AddScoped<IActivityGroupCommandService, ActivityGroupCommandRestService>();
            services.AddScoped<IActivityGroupNotificationService, ActivityGroupNotificationRestService>();
            services.AddScoped<IActivityGroupExportService, ActivityGroupExportRestService>();

            // School Event services
            services.AddScoped<ISchoolEventCategoryService, SchoolEventCategoryRestService>();
            services.AddScoped<ISchoolEventNotificationService, SchoolEventNotificationRestService>();
            services.AddScoped<ISchoolEventPermissionService, SchoolEventPermissionRestService>();
            services.AddScoped<ISchoolEventExportService, SchoolEventExportRestService>();
            services.AddScoped<ISchoolEventQueryService, SchoolEventQueryRestService>();
            services.AddScoped<ISchoolEventCommandService, SchoolEventCommandRestService>();

            // Attendance services
            services.AddScoped<IAttendanceService, AttendanceRestService>();
            services.AddScoped<IAttendanceGroupService, AttendanceGroupRestService>();

            // Discipline services
            services.AddScoped<IDisciplinaryIncidentService, DisciplinaryIncidentRestService>();
            services.AddScoped<IDisciplinaryActionService, DisciplinaryActionRestService>();

            return services;
        }
    }

}
