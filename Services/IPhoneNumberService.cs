namespace PhoneNumberRegister.Services;

public interface IPhoneNumberService
{
    Task<bool> ExistsAsync(string phoneNumber);
    Task AddAsync(string phoneNumber);
}