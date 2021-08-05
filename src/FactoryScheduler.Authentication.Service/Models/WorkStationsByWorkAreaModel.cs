using System;
using FactoryScheduler.Authentication.Service.Dtos;

namespace FactoryScheduler.Authentication.Service.Models
{
    public class WorkStationsByWorkAreaModel
    {
        public Guid WorkAreaId { get; init; }

        public WorkStationDto[] WorkStationDtos { get; init; }
    }

}