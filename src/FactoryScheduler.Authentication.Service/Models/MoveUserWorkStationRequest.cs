using System;
using System.ComponentModel.DataAnnotations;

public record MoveUserWorkStationRequest([Required] Guid NewWorkStationId, [Required] Guid UserId);