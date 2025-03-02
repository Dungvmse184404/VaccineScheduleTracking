using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Services.Record;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineRecordController : ControllerBase
    {
        private readonly IVaccineRecordService vaccineRecordService;
        private readonly IMapper mapper;

        public VaccineRecordController(IVaccineRecordService vaccineRecordService, IMapper mapper)
        {
            this.vaccineRecordService = vaccineRecordService;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpGet("get-child-vaccine-record/{childID}")]
        public async Task<IActionResult> GetChildVaccineRecord([FromRoute] int childID)
        {
            var records = await vaccineRecordService.GetRecordsAsync(childID);
            if (records == null)
            {
                return BadRequest(new
                {
                    Message = "Không tìm thấy thông tin tiêm vaccine của trẻ"
                });
            }

            return Ok(mapper.Map<List<VaccineRecordDto>>(records));
        }

        [Authorize]
        [HttpGet("get-record/{recordID}")] 
        public async Task<IActionResult> GetVaccineRecordByID([FromRoute] int recordID)
        {
            var record = await vaccineRecordService.GetRecordByIDAsync(recordID);
            if (record == null)
            {
                return BadRequest(new
                {
                    Message = "Không tìm thấy bản ghi hợp lệ"
                });
            }
            return Ok(mapper.Map<VaccineRecordDto>(record));
        }

        [Authorize]
        [HttpPost("update-record")]
        public async Task<IActionResult> UpdateVaccineRecord([FromBody] UpdateVaccineRecordDto updateVaccine)
        {
            var updatedRecord = await vaccineRecordService.UpdateVaccineRecordAsync(updateVaccine);
            if (updatedRecord == null)
            {
                return BadRequest(new
                {
                    Message = "Không tìm thấy bản ghi hợp lệ"
                });
            }
            return Ok(mapper.Map<VaccineRecordDto>(updatedRecord));
        }

        [Authorize]
        [HttpDelete("delete-record/{recordID}")]
        public async Task<IActionResult> DeleteVaccineRecord([FromRoute] int recordID)
        {
            var deletedRecord = await vaccineRecordService.DeleteRecordAsync(recordID);
            if (deletedRecord == null)
            {
                return BadRequest(new
                {
                    Message = "Không tìm thấy bản ghi hợp lệ"
                });
            }
            return Ok(mapper.Map<VaccineRecordDto>(deletedRecord));
        } 
    }
}
