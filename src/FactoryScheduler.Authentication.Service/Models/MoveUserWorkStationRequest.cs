using System;
using System.ComponentModel.DataAnnotations;

public record MoveUserWorkStationRequest(Guid NewWorkStationId, [Required] Guid UserId);