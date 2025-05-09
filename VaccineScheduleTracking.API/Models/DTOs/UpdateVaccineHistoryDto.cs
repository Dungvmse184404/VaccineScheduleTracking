﻿using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class UpdateVaccineHistoryDto
    {
        [Required]
        public int VaccineRecordID { get; set; }
        [Required]
        public int VaccineTypeID { get; set; }
        public int? VaccineID { get; set; }
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
    }
}
