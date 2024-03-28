using Microsoft.AspNetCore.Http;
using SharedKernal.Data.Enums;
using SharedKernal.Models.Profile;

namespace Application.Profile.Common;

public record CreateProfileRequest(string ProfileName,
								   string StartDepot, 
								   string EndDepot, 
								   int OnOffLoadTime, 
								   int SlackTime, 
								   IFormFile CsvFile,
								   Vehicle[] Vehicles,
								   string Status = nameof(ProfileStatus.InProgress));