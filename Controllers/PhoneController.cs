using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PhoneNumberRegister.DTOs;
using PhoneNumberRegister.Services;

namespace PhoneNumberRegister.Controllers;

[ApiController]
[Route("")]
public class PhoneController : ControllerBase
{
    private readonly IPhoneNumberService _service;
    private readonly IValidator<PhoneNumberRequest> _validator;

    public PhoneController(IPhoneNumberService service, IValidator<PhoneNumberRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpPost("check_number")]
    public async Task<IActionResult> CheckNumber([FromBody] PhoneNumberRequest request)
    {
        // Validation
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return BadRequest(new
            {
                errors = validation.Errors.Select(e => e.ErrorMessage)
            });
        }

        // check if num exist
        var exists = await _service.ExistsAsync(request.PhoneNumber);
        if (exists)
        {
            return Conflict(new { message = "Phone number already exists." });
        }

        // add number
        await _service.AddAsync(request.PhoneNumber);
        return StatusCode(201, new { message = "Phone number added successfully." });
    }
}