using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Services.Children;
using VaccineScheduleTracking.API_Test.Services.Record;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineRecordController : ControllerBase
    {
        private readonly IVaccineRecordService vaccineRecordService;
        private readonly IMapper mapper;
        private readonly IChildService childService;

        public VaccineRecordController(IVaccineRecordService vaccineRecordService, IMapper mapper, IChildService childService)
        {
            this.vaccineRecordService = vaccineRecordService;
            this.mapper = mapper;
            this.childService = childService;
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

        [Authorize(Roles = "Doctor")]
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

        [Authorize(Roles = "Parent")]
        [HttpPost("update-child-vaccine-history")]
        public async Task<IActionResult> UpdateVaccineHistory([FromBody] UpdateVaccineHistoryDto updateVaccineHistory)
        {
            var record = await vaccineRecordService.GetRecordByIDAsync(updateVaccineHistory.VaccineRecordID);
            if(record == null)
            {
                return BadRequest(new
                {
                    Message = "Không tìm thấy bản ghi hợp lệ"
                });
            }
            var accountID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var child = await childService.GetChildByIDAsync(record.ChildID);
            if (child != null && child.ParentID.ToString() == accountID)
            {
                var updatedRecord = await vaccineRecordService.UpdateVaccineHistoryAsync(updateVaccineHistory);
                return Ok(mapper.Map<VaccineRecordDto>(updatedRecord));
            }
            return BadRequest(new
            {
                Message = "Bạn không có quyền chỉnh sửa bản ghi này!"
            });
        }

        [Authorize]
        [HttpDelete("delete-record/{recordID}")]
        public async Task<IActionResult> DeleteVaccineRecord([FromRoute] int recordID)
        {
            var role = User.FindFirst("role")?.Value;
            var record = await vaccineRecordService.GetRecordByIDAsync(recordID);
            if (record == null)
            {
                return BadRequest(new
                {
                    Message = "Không tìm thấy bản ghi hợp lệ"
                });

            }
            if (role == "Parent" && record.AppointmentID != null)
            {
                return BadRequest(new
                {
                    Message = "Bạn không có quyền xóa bản ghi này!"
                });
            }
            await vaccineRecordService.DeleteRecordAsync(recordID);
            return Ok(mapper.Map<VaccineRecordDto>(record));


        }

        [Authorize(Roles = "Parent")]
        [HttpPost("create-vaccine-history")]
        public async Task<IActionResult> CreateVaccineHistory([FromBody] ChildVaccineHistoryDto childVaccineHistory)
        {
            var accountID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var child = await childService.GetChildByIDAsync(childVaccineHistory.ChildID);
            if (child != null && child.ParentID.ToString() == accountID)
            {
                var record = await vaccineRecordService.AddVaccineHistoryAsync(childVaccineHistory);
                return Ok(mapper.Map<VaccineRecordDto>(record));
            }
            return BadRequest($"Thông tin của đứa trẻ không hợp lệ!");
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("create-vaccine-record")]
        public async Task<IActionResult> CreateVaccineRecord([FromBody] CreateVaccineRecordDto createVaccineRecord)
        {
            var record = await vaccineRecordService.AddVaccineRecordAsync(createVaccineRecord);
            return Ok(mapper.Map<VaccineRecordDto>(record));
        }
    }
}
