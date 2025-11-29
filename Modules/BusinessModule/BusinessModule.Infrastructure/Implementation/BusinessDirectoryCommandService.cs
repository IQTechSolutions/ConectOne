using BusinessModule.Domain.DataTransferObjects;
using BusinessModule.Domain.Entities;
using BusinessModule.Domain.Interfaces;
using FilingModule.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeuralTech.Base.Enums;
using System.Text;
using BusinessModule.Domain.RequestFeatures;
using ConectOne.Domain.Enums;
using ConectOne.Domain.Extensions;
using ConectOne.Domain.Mailing.Entities;
using ConectOne.Domain.Mailing.Services;
using ConectOne.Domain.ResultWrappers;
using ConectOne.EntityFrameworkCore.Sql.Implimentation;
using ConectOne.EntityFrameworkCore.Sql.Interfaces;
using FilingModule.Domain.RequestFeatures;
using GroupingModule.Domain.Entities;
using IdentityModule.Domain.DataTransferObjects;
using IdentityModule.Domain.Interfaces;
using MessagingModule.Domain.DataTransferObjects;
using MessagingModule.Domain.Interfaces;

namespace BusinessModule.Infrastructure.Implementation;

/// <summary>
/// Provides command operations for managing business directory listings.
/// </summary>
public class BusinessDirectoryCommandService(IRepository<BusinessListing, string> repository, IRepository<EntityImage<BusinessListing, string>, string> imageRepository,
    IRepository<EntityVideo<BusinessListing, string>, string> videoRepository, IRepository<EntityImage<ListingService, string>, string> listingServiceImageRepository,
    IRepository<EntityImage<ListingProduct, string>, string> listingProductImageRepository, IRepository<ListingService, string> listingServiceRepo,
    IRepository<ListingProduct, string> listingProductRepo, IUserService userService, IPushNotificationService pushNotificationService,
    EmailQueue emailQueue, IConfiguration configuration) : IBusinessDirectoryCommandService
{
    /// <summary>
    /// Asynchronously creates a new business listing and persists it to the repository.
    /// </summary>
    /// <remarks>This method validates and creates a new business listing based on the provided <paramref
    /// name="dto"/>. If the creation or saving process fails, the result will include the corresponding error
    /// messages.</remarks>
    /// <param name="dto">The data transfer object containing the details of the business listing to create.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/> with
    /// the created <see cref="BusinessListingDto"/> if the operation succeeds, or error messages if it fails.</returns>
    public async Task<IBaseResult<BusinessListingDto>> CreateAsync(BusinessListingDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new BusinessListing
        {
            Id = dto.Id,
            UserId = dto.UserId,
            Heading = dto.Heading,
            Slogan = dto.Slogan,
            Address = dto.Address,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Description = dto.Description,
            WebsiteUrl = dto.WebsiteUrl,
            ListingTierId = dto.Tier?.Id,
            Status = dto.Status,
            Categories = dto.Categories.Select(c => new EntityCategory<BusinessListing>
            {
                CategoryId = c.CategoryId,
                EntityId = dto.Id
            }).ToList() ?? [],
            Services = dto.Services.Select(s => new ListingService
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                ListingId = dto.Id
            }).ToList() ?? new List<ListingService>(),
            Products = dto.Products.Select(p => new ListingProduct
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ListingId = dto.Id
            }).ToList() ?? []
        };

        entity.ActivateForDefaultPeriod(DateTime.UtcNow);

        var result = await repository.CreateAsync(entity, cancellationToken);
        if (!result.Succeeded)
            return await Result<BusinessListingDto>.FailAsync(result.Messages);

        var save = await repository.SaveAsync(cancellationToken);
        if (!save.Succeeded)
            return await Result<BusinessListingDto>.FailAsync(save.Messages);

        return await Result<BusinessListingDto>.SuccessAsync(new BusinessListingDto(result.Data));
    }

    /// <summary>
    /// Sends an enquiry message to the owner of a business listing by queuing an email and push notification.
    /// </summary>
    /// <param name="request">The enquiry payload submitted by the visitor.</param>
    /// <param name="cancellationToken">A token that can cancel the operation.</param>
    /// <returns>A result describing whether any delivery mechanism was successfully queued.</returns>
    public async Task<IBaseResult> ContactListingOwnerAsync(ListingContactRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
            return await Result.FailAsync("Request payload cannot be null.");

        if (string.IsNullOrWhiteSpace(request.ListingId))
            return await Result.FailAsync("A listing identifier must be provided.");

        var listingSpec = new LambdaSpec<BusinessListing>(listing => listing.Id == request.ListingId);
        var listingResult = await repository.FirstOrDefaultAsync(listingSpec, false, cancellationToken);
        if (!listingResult.Succeeded || listingResult.Data is null)
            return await Result.FailAsync(listingResult.Messages.FirstOrDefault() ?? "The requested listing could not be found.");

        var listing = listingResult.Data;

        var ownerEmails = new List<string>();
        if (!string.IsNullOrWhiteSpace(listing.Email))
            ownerEmails.Add(listing.Email);

        UserInfoDto? ownerInfo = null;
        if (!string.IsNullOrWhiteSpace(listing.UserId))
        {
            var ownerResult = await userService.GetUserInfoAsync(listing.UserId, cancellationToken);
            if (ownerResult.Succeeded && ownerResult.Data is not null)
            {
                ownerInfo = ownerResult.Data;
                if (!string.IsNullOrWhiteSpace(ownerInfo.EmailAddress))
                    ownerEmails.Add(ownerInfo.EmailAddress);
            }
        }

        ownerEmails = ownerEmails
            .Where(email => !string.IsNullOrWhiteSpace(email))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var warnings = new List<string>();
        var hasSuccessfulDelivery = false;

        var senderEmail = configuration["ApplicationConfiguration:DoNotReplyEmailAddress"] ?? configuration["EmailConfiguration:From"];
        var senderName = configuration["ApplicationConfiguration:AppliactionName"] ?? "Schools Enterprise";

        if (ownerEmails.Any())
        {
            if (string.IsNullOrWhiteSpace(senderEmail))
            {
                warnings.Add("Unable to queue the email because no sender address is configured.");
            }
            else
            {
                var toName = ownerInfo?.DisplayName ?? ownerInfo?.FirstName ?? listing.Heading ?? "Listing owner";
                var subject = $"New enquiry for {listing.Heading}";
                var body = BuildEnquiryEmailBody(listing, request);

                foreach (var ownerEmail in ownerEmails)
                {
                    emailQueue.Enqueue(new EmailDetails(
                        toName,
                        ownerEmail,
                        subject,
                        senderName,
                        senderEmail,
                        body));
                }

                hasSuccessfulDelivery = true;
            }
        }
        else
        {
            warnings.Add("No email address is configured for the listing owner.");
        }

        var recipients = new List<RecipientDto>();
        if (ownerEmails.Any())
        {
            var firstName = ownerInfo?.FirstName;
            var lastName = ownerInfo?.LastName;

            if (string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(ownerInfo?.DisplayName))
            {
                var parts = ownerInfo.DisplayName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    firstName = parts[0];
                    if (parts.Length > 1)
                        lastName = string.Join(' ', parts.Skip(1));
                }
            }

            recipients.Add(new RecipientDto(
                ownerInfo?.UserId ?? listing.UserId ?? Guid.NewGuid().ToString(),
                firstName ?? listing.Heading ?? "Owner",
                lastName ?? string.Empty,
                ownerEmails,
                ownerInfo?.ReceiveNotifications ?? true,
                ownerInfo?.ReceiveEmails ?? true));
        }

        if (recipients.Any())
        {
            var visitorMessage = string.IsNullOrWhiteSpace(request.Message)
                ? $"You received a new enquiry from {request.FullName}."
                : request.Message!;

            var notification = new NotificationDto
            {
                NotificationId = Guid.NewGuid().ToString(),
                EntityId = listing.Id,
                MessageType = MessageType.None,
                Title = $"New enquiry for {listing.Heading}",
                ShortDescription = visitorMessage.TruncateLongString(55),
                Message = visitorMessage,
                NotificationUrl = $"/listingDetails/{listing.Id}"
            };

            var notifyResult = await pushNotificationService.EnqueueNotificationsAsync(recipients, notification);
            if (!notifyResult.Succeeded)
            {
                warnings.AddRange(notifyResult.Messages);
            }
            else
            {
                hasSuccessfulDelivery = true;
            }
        }
        else
        {
            warnings.Add("No registered user could be resolved to receive a push notification.");
        }

        if (!hasSuccessfulDelivery)
        {
            return await Result.FailAsync(warnings.Any()
                ? warnings
                : new List<string> { "Unable to deliver the enquiry." });
        }

        var response = (Result)Result.Success("Enquiry submitted successfully.");
        foreach (var warning in warnings.Where(message => !string.IsNullOrWhiteSpace(message)))
        {
            response.Messages.Add(warning);
        }

        return response;
    }

    private string BuildEnquiryEmailBody(BusinessListing listing, ListingContactRequest request)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"<p>You have received a new enquiry for <strong>{listing.Heading}</strong>.</p>");
        builder.AppendLine("<p><strong>Contact details:</strong></p><ul>");

        if (!string.IsNullOrWhiteSpace(request.FullName))
            builder.AppendLine($"<li>Name: {request.FullName}</li>");

        if (!string.IsNullOrWhiteSpace(request.Email))
            builder.AppendLine($"<li>Email: <a href=\"mailto:{request.Email}\">{request.Email}</a></li>");

        builder.AppendLine("</ul>");

        if (!string.IsNullOrWhiteSpace(request.Message))
        {
            builder.AppendLine("<p><strong>Message:</strong></p>");
            builder.AppendLine($"<p>{request.Message.Replace("\r\n", "<br/>").Replace("\n", "<br/>")}</p>");
        }

        var listingUrl = configuration["ApplicationConfiguration:WebAddress"];
        if (!string.IsNullOrWhiteSpace(listingUrl))
        {
            var absoluteUrl = $"{listingUrl.TrimEnd('/')}/listingDetails/{listing.Id}";
            builder.AppendLine($"<p><a href=\"{absoluteUrl}\">View listing details</a></p>");
        }

        builder.AppendLine("<p>This message was generated automatically by the business directory.</p>");

        return builder.ToString();
    }

    /// <summary>
    /// Updates an existing business listing with the provided data.
    /// </summary>
    /// <remarks>The method attempts to locate an existing business listing by the ID specified in the
    /// <paramref name="dto"/>. If the listing is found, its properties are updated with the values from the <paramref
    /// name="dto"/>. If the listing is not found, the method returns a failure result with an appropriate
    /// message.</remarks>
    /// <param name="dto">The data transfer object containing the updated information for the business listing.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation.</returns>
    public async Task<IBaseResult> UpdateAsync(BusinessListingDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<BusinessListing>(l => l.Id == dto.Id);
        spec.AddInclude(c => c.Include(g => g.Categories));

        var query = await repository.FindByConditionAsync(l => l.Id == dto.Id, true, cancellationToken);
        var entity = query.Succeeded ? query.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Listing not found.");

        entity.Heading = dto.Heading;
        entity.Slogan = dto.Slogan;
        if (dto.ActiveUntil.HasValue)
            entity.ActiveUntil = dto.ActiveUntil;
        entity.Address = dto.Address;
        entity.Email = dto.Email;
        entity.PhoneNumber = dto.PhoneNumber;
        entity.Description = dto.Description;
        entity.WebsiteUrl = dto.WebsiteUrl;
        entity.ListingTierId = dto.Tier?.Id;
        entity.Status = dto.Status;
        entity.Categories.Clear();
        entity.Categories = dto.Categories.Select(c => new EntityCategory<BusinessListing>
        {
            CategoryId = c.CategoryId,
            EntityId = dto.Id
        }).ToList() ?? [];


        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);

        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Renews a business listing for an additional active period.
    /// </summary>
    /// <param name="listingId">The identifier of the listing to renew.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The updated listing details if the renewal succeeds.</returns>
    public async Task<IBaseResult<BusinessListingDto>> RenewAsync(string listingId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(listingId))
            return await Result<BusinessListingDto>.FailAsync("A listing identifier must be provided.");

        var query = await repository.FindByConditionAsync(l => l.Id == listingId, true, cancellationToken);
        if (!query.Succeeded)
            return await Result<BusinessListingDto>.FailAsync(query.Messages);

        var entity = query.Data.FirstOrDefault();
        if (entity == null)
            return await Result<BusinessListingDto>.FailAsync("Listing not found.");

        entity.Renew(DateTime.UtcNow);

        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result<BusinessListingDto>.FailAsync(update.Messages);

        var save = await repository.SaveAsync(cancellationToken);
        if (!save.Succeeded)
            return await Result<BusinessListingDto>.FailAsync(save.Messages);

        return await Result<BusinessListingDto>.SuccessAsync(new BusinessListingDto(entity));
    }

    /// <summary>
    /// Removes a listing identified by the specified <paramref name="listingId"/>.
    /// </summary>
    /// <remarks>If the operation fails, the returned result will include error messages describing the
    /// failure.</remarks>
    /// <param name="listingId">The unique identifier of the listing to be removed. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation, along with any associated messages.</returns>
    public async Task<IBaseResult> RemoveAsync(string listingId, CancellationToken cancellationToken = default)
    {
        var result = await repository.DeleteAsync(listingId, cancellationToken);
        if (!result.Succeeded)
            return await Result.FailAsync(result.Messages);
        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Approves a listing by its unique identifier.
    /// </summary>
    /// <remarks>If the listing is not found, the operation fails with an appropriate error message.  If the
    /// listing is successfully approved, the changes are persisted to the repository.</remarks>
    /// <param name="listingId">The unique identifier of the listing to approve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an  IBaseResult indicating the
    /// success or failure of the operation.</returns>
    public async Task<IBaseResult> ApproveAsync(string listingId, CancellationToken cancellationToken = default)
    {
        var spec = await repository.FindByConditionAsync(l => l.Id == listingId, true, cancellationToken);
        var entity = spec.Succeeded ? spec.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Listing not found.");
        entity.Status = ReviewStatus.Approved;
        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);
        return await repository.SaveAsync(cancellationToken);
    }

    /// <summary>
    /// Rejects a listing by its identifier and updates its status to <see cref="ReviewStatus.Rejected"/>.
    /// </summary>
    /// <remarks>This method attempts to find the listing by its identifier. If the listing is found, its
    /// status is updated to <see cref="ReviewStatus.Rejected"/> and the changes are saved to the repository. If the
    /// listing is not found or the update operation fails, the result will indicate failure with relevant error
    /// messages.</remarks>
    /// <param name="listingId">The unique identifier of the listing to reject.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. If the listing is not found, the result will indicate failure with an
    /// appropriate message.</returns>
    public async Task<IBaseResult> RejectAsync(string listingId, CancellationToken cancellationToken = default)
    {
        var spec = await repository.FindByConditionAsync(l => l.Id == listingId, true, cancellationToken);
        var entity = spec.Succeeded ? spec.Data.FirstOrDefault() : null;
        if (entity == null)
            return await Result.FailAsync("Listing not found.");
        entity.Status = ReviewStatus.Rejected;
        var update = repository.Update(entity);
        if (!update.Succeeded)
            return await Result.FailAsync(update.Messages);
        return await repository.SaveAsync(cancellationToken);
    }

    #region Images

    /// <summary>
    /// Adds an image to the specified entity with the provided details.
    /// </summary>
    /// <remarks>This method creates an association between an image and an entity, saving the image details
    /// to the repository. The operation will fail if the repository operations (create or save) are
    /// unsuccessful.</remarks>
    /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> AddImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityImage<BusinessListing, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

        var addResult = await imageRepository.CreateAsync(image, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await imageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes an image with the specified identifier from the repository.
    /// </summary>
    /// <remarks>This method first attempts to delete the image from the repository. If the deletion succeeds,
    /// it then saves the changes to the repository. If either operation fails, the method returns a failure result with
    /// the associated error messages.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> RemoveImage(string imageId, CancellationToken cancellationToken = default)
    {
        var addResult = await imageRepository.DeleteAsync(imageId, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await imageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    #endregion

    #region Videos

    /// <summary>
    /// Adds a video associated with a specific entity to the repository.
    /// </summary>
    /// <remarks>This method attempts to add a video to the repository and save the changes. If the
    /// operation fails at any step, the result will contain the failure messages.</remarks>
    /// <param name="request">The request containing the video details, including the video ID and the associated entity ID.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation.</returns>
    public async Task<IBaseResult> AddVideo(AddEntityVideoRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityVideo<BusinessListing, string> { VideoId = request.VideoId, EntityId = request.EntityId };

        var addResult = await videoRepository.CreateAsync(image, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await videoRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes a video with the specified identifier from the repository.
    /// </summary>
    /// <remarks>This method performs two operations: it deletes the video from the repository and
    /// then saves the changes. If either operation fails, the method returns a failure result containing the
    /// associated error messages.</remarks>
    /// <param name="videoId">The unique identifier of the video to be removed. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
    /// cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the outcome of the operation. If the operation succeeds, the result will indicate success. If the
    /// operation fails, the result will contain error messages.</returns>
    public async Task<IBaseResult> RemoveVideo(string videoId, CancellationToken cancellationToken = default)
    {
        var addResult = await videoRepository.DeleteAsync(videoId, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await videoRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    #endregion

    #region Listing Services

    /// <summary>
    /// Adds a new listing service to the repository.
    /// </summary>
    /// <remarks>This method creates a new listing service using the provided <paramref name="dto"/> and saves
    /// it to the repository. If the operation fails at any stage, the returned result will contain the failure
    /// messages.</remarks>
    /// <param name="dto">The data transfer object containing the details of the listing service to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> AddListingService([FromBody] ListingServiceDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.ListingId))
        {
            return await Result.FailAsync("A listing identifier is required to add a service.");
        }

        var service = new ListingService
        {
            Id = string.IsNullOrWhiteSpace(dto.Id) ? Guid.NewGuid().ToString() : dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ListingId = dto.ListingId
        };

        var result = await listingServiceRepo.CreateAsync(service, cancellationToken);

        if (!result.Succeeded) return await Result.FailAsync(result.Messages);
        var saveResult = await listingServiceRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Updates an existing listing service with the provided details.
    /// </summary>
    /// <remarks>The method attempts to find an existing listing service by the ID provided in the <paramref
    /// name="dto"/>. If the service is not found, the operation fails with an appropriate error message. If the service
    /// is found, its details are updated with the values from the <paramref name="dto"/>. The operation succeeds only
    /// if the update is successfully persisted.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the listing service.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation.</returns>
    public async Task<IBaseResult> UpdateListingService([FromBody] ListingServiceDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ListingService>(c => c.Id == dto.Id);
        var result = await listingServiceRepo.FirstOrDefaultAsync(spec, true, cancellationToken);

        if (!result.Succeeded || result.Data == null)
            return await Result.FailAsync("Service not found.");

        var entity = result.Data;

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Price = dto.Price;

        var updateResult = listingServiceRepo.Update(entity);
        if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

        var saveResult = await listingServiceRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes a listing service identified by the specified ID.
    /// </summary>
    /// <remarks>This method attempts to delete the listing service and save the changes to the repository. If
    /// the operation fails at any step,  the returned result will contain the failure messages.</remarks>
    /// <param name="listingServiceId">The unique identifier of the listing service to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> RemoveListingService(string listingServiceId, CancellationToken cancellationToken = default)
    {
        var result = await listingServiceRepo.DeleteAsync(listingServiceId, cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);
        var saveResult = await listingServiceRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
        return await Result.SuccessAsync();
    }

    #endregion

    #region Listing Service Images

    /// <summary>
    /// Adds an image to the specified entity with the provided details.
    /// </summary>
    /// <remarks>This method creates an association between an image and an entity, saving the image details
    /// to the repository. The operation will fail if the repository operations (create or save) are
    /// unsuccessful.</remarks>
    /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> AddListingServiceImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityImage<ListingService, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

        var addResult = await listingServiceImageRepository.CreateAsync(image, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await listingServiceImageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes an image with the specified identifier from the repository.
    /// </summary>
    /// <remarks>This method first attempts to delete the image from the repository. If the deletion succeeds,
    /// it then saves the changes to the repository. If either operation fails, the method returns a failure result with
    /// the associated error messages.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> RemoveListingServiceImage(string imageId, CancellationToken cancellationToken = default)
    {
        var addResult = await listingServiceImageRepository.DeleteAsync(imageId, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await listingServiceImageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    #endregion

    #region Listing Products

    /// <summary>
    /// Adds a new listing product to the repository.
    /// </summary>
    /// <remarks>This method creates a new listing product using the provided <paramref name="dto"/> and saves
    /// it to the repository. If the operation fails, the returned result will contain error messages describing the
    /// failure.</remarks>
    /// <param name="dto">The data transfer object containing the details of the listing product to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation.</returns>
    public async Task<IBaseResult> AddListingProduct([FromBody] ListingProductDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.ListingId))
        {
            return await Result.FailAsync("A listing identifier is required to add a product.");
        }

        var product = new ListingProduct
        {
            Id = string.IsNullOrWhiteSpace(dto.Id) ? Guid.NewGuid().ToString() : dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ListingId = dto.ListingId
        };

        var result = await listingProductRepo.CreateAsync(product, cancellationToken);

        if (!result.Succeeded) return await Result.FailAsync(result.Messages);
        var saveResult = await listingProductRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Updates an existing listing product with the provided details.
    /// </summary>
    /// <remarks>The method updates the name, description, and price of the listing product identified by the
    /// <c>Id</c> property in the <paramref name="dto"/>. If the specified product is not found, the operation fails
    /// with an appropriate error message.</remarks>
    /// <param name="dto">The data transfer object containing the updated details of the listing product.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the update operation.</returns>
    public async Task<IBaseResult> UpdateListingProduct([FromBody] ListingProductDto dto, CancellationToken cancellationToken = default)
    {
        var spec = new LambdaSpec<ListingProduct>(c => c.Id == dto.Id);
        var result = await listingProductRepo.FirstOrDefaultAsync(spec, true, cancellationToken);

        if (!result.Succeeded || result.Data == null)
            return await Result.FailAsync("Service not found.");

        var entity = result.Data;

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Price = dto.Price;

        var updateResult = listingProductRepo.Update(entity);
        if (!updateResult.Succeeded) return await Result.FailAsync(updateResult.Messages);

        var saveResult = await listingProductRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes a listing product identified by the specified ID.
    /// </summary>
    /// <remarks>This method attempts to delete the specified listing product and persist the changes to the
    /// repository. If the deletion or save operation fails, the returned result will indicate failure with the
    /// corresponding error messages.</remarks>
    /// <param name="listingProductId">The unique identifier of the listing product to be removed. This value cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> RemoveListingProduct(string listingProductId, CancellationToken cancellationToken = default)
    {
        var result = await listingProductRepo.DeleteAsync(listingProductId, cancellationToken);
        if (!result.Succeeded) return await Result.FailAsync(result.Messages);
        var saveResult = await listingProductRepo.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);
        return await Result.SuccessAsync();
    }

    #endregion

    #region Listing Product Images

    /// <summary>
    /// Adds an image to the specified entity with the provided details.
    /// </summary>
    /// <remarks>This method creates an association between an image and an entity, saving the image details
    /// to the repository. The operation will fail if the repository operations (create or save) are
    /// unsuccessful.</remarks>
    /// <param name="request">The request containing the image details, including the image ID, entity ID, selector, and order.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> AddListingProductImage(AddEntityImageRequest request, CancellationToken cancellationToken = default)
    {
        var image = new EntityImage<ListingProduct, string>(request.ImageId, request.EntityId) { Selector = request.Selector, Order = request.Order };

        var addResult = await listingProductImageRepository.CreateAsync(image, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await listingProductImageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    /// <summary>
    /// Removes an image with the specified identifier from the repository.
    /// </summary>
    /// <remarks>This method first attempts to delete the image from the repository. If the deletion succeeds,
    /// it then saves the changes to the repository. If either operation fails, the method returns a failure result with
    /// the associated error messages.</remarks>
    /// <param name="imageId">The unique identifier of the image to be removed. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IBaseResult"/>
    /// indicating the success or failure of the operation. If the operation fails, the result includes error messages.</returns>
    public async Task<IBaseResult> RemoveListingProductImage(string imageId, CancellationToken cancellationToken = default)
    {
        var addResult = await listingProductImageRepository.DeleteAsync(imageId, cancellationToken);
        if (!addResult.Succeeded) return await Result.FailAsync(addResult.Messages);

        var saveResult = await listingProductImageRepository.SaveAsync(cancellationToken);
        if (!saveResult.Succeeded) return await Result.FailAsync(saveResult.Messages);

        return await Result.SuccessAsync();
    }

    #endregion
}
