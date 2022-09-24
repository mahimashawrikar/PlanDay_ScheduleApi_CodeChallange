using Microsoft.AspNetCore.Mvc;
using Planday.Schedule.Api.Services;
using Newtonsoft.Json;
using Microsoft.Data.Sqlite;

namespace Planday.Schedule.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        
        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;          
        }

        /// <summary>
        /// Get All Shifts
        /// </summary>
        /// <returns>Returns all the shifts (This has been created for testing purpose to see all the data)</returns>
        [HttpGet]
        [Route("/getAllShifts")]
        public async Task<IActionResult> GetAllShifts()
        {
            var shifts = await _shiftService.getAllShifts();
            if (shifts==null) 
                return NotFound();

            return Ok(shifts);
        }

        /// <summary>
        /// Get Shift by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Shift by the supplied id if found, otherwise returns NotFound</returns>
        [HttpGet]
        [Route("/getShiftById")]

        public async Task<IActionResult> GetShiftById(long shiftId)
        {
            var shifts = await _shiftService.getAllShifts();
            var shift = shifts.FirstOrDefault(s => s.Id == shiftId);

            if (shift==null) 
                return BadRequest($"Shift with ID {shiftId} does not exist");

            //Task 4
            var client = new HttpClient();
            var _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string key = _configuration.GetValue<string>("Key");
            string url = _configuration.GetValue<string>("URL");
            client.DefaultRequestHeaders.Add("Authorization", key);
            var response = await client.GetAsync(url+shift?.EmployeeId);
            var employeeDto = JsonConvert.DeserializeObject<EmployeeDto>(await response.Content.ReadAsStringAsync());

            return employeeDto==null ? Ok(shift) : Ok(new ShiftEmployeeDto(shift.Id, shift.EmployeeId, shift.Start, shift.End, employeeDto.Email));
        }

        /// <summary>
        /// Creates an open Shift, An open shift is a Shift without an employee assigned
        /// </summary>
        /// <param name="shift"></param>
        /// <returns>If the Open Shift created successfully, returns sucess message, otherwise returns Bad Request</returns>
        [HttpPost]
        [Route("/createOpenShift")]
        public IActionResult CreateOpenShift(Shift shift)
        {
            if (shift.Start > shift.End) 
                return BadRequest("The start time must not be greater than the end time");
            if (shift.Start.Date != shift.End.Date) 
                return BadRequest("Start and end time should be in the same day");
            try
            {
                _shiftService.postOpenShift(shift);
                return Ok("Shift created successfully");
            }
            catch (SqliteException e)
            {
                return Conflict(e.Message);
            }
        }

        /// <summary>
        /// Assign a Shift To Employee
        /// </summary>
        /// <param name="shiftId,employeeId"></param>
        /// <returns>If the Open Shift created successfully, returns sucess message, otherwise returns Bad Request</returns>
        [HttpPost]
        [Route("/assignShiftToEmployee")]
        public async Task<IActionResult> AssignShiftToEmployee(long shiftId, long employeeId)
        {
            var employees = await _shiftService.getAllEmployees();
            var employee = employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee==null) 
                return BadRequest($"Employee with ID {employeeId} does not exist");
            var shifts = await _shiftService.getAllShifts();
            var shift = shifts.FirstOrDefault(s => s.Id == shiftId);
            if (shift==null) 
                return BadRequest($"Shift with ID {shiftId} does not exist");

            foreach (var element in shifts)
            {
                if (element.EmployeeId == employeeId)
                {
                    if (element.Id == shiftId) 
                        return BadRequest($"EmployeeId {employee.Id} has already assigned this Shift");
                    if (!(shift.End <= element.Start || shift.Start>=element.End))
                         return BadRequest("Shifts are overlapping");                   
                }
                if (shift.EmployeeId!=null && element.EmployeeId == shift.EmployeeId)
                    return BadRequest("You cannot assign the same shift to two or more employees");
            }

            try
            {               
                    _shiftService.updateShift(shiftId, employeeId);
                       return Ok("Shift assigned successfully");
                
            }
            catch (SqliteException e)
            {
                return Conflict(e.Message);
            }
        }

        private record EmployeeDto(string Name, string Email);

        private record ShiftEmployeeDto(long Id, long? EmployeeId, DateTime Start, DateTime End, string Email);
    }
}



