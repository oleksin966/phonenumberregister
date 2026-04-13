using FluentValidation;
using PhoneNumbers;
using PhoneNumberRegister.DTOs;

namespace PhoneNumberRegister.Validators;

public class PhoneNumberValidator : AbstractValidator<PhoneNumberRequest>
{
    private readonly PhoneNumberUtil _phoneUtil = PhoneNumberUtil.GetInstance();

    public PhoneNumberValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Must(BeValidPhoneNumber)
            .WithMessage("Phone number format is invalid.")
            .Must(BeAmericanMobileNumber)
            .WithMessage("Only US mobile numbers are accepted.");
    }

    private bool BeValidPhoneNumber(string input)
    {
        try
        {
            var number = _phoneUtil.Parse(input, null);
            return _phoneUtil.IsValidNumber(number);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }

    private bool BeAmericanMobileNumber(string input)
    {
        try
        {
            var number = _phoneUtil.Parse(input, null);
            var region = _phoneUtil.GetRegionCodeForNumber(number);
            var type = _phoneUtil.GetNumberType(number);

            return region == "US" &&
                   (type == PhoneNumberType.MOBILE || type == PhoneNumberType.FIXED_LINE_OR_MOBILE);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}