using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using PhoneNumberRegister.Data;
using PhoneNumberEntity = PhoneNumberRegister.Models.PhoneNumber; // ← alias

namespace PhoneNumberRegister.Services;

public class PhoneNumberService : IPhoneNumberService
{
    private readonly AppDbContext _context;
    private readonly PhoneNumberUtil _phoneUtil = PhoneNumberUtil.GetInstance();

    public PhoneNumberService(AppDbContext context)
    {
        _context = context;
    }

    private string Normalize(string phoneNumber)
    {
        var number = _phoneUtil.Parse(phoneNumber, null);
        return _phoneUtil.Format(number, PhoneNumberFormat.E164);
    }

    public async Task<bool> ExistsAsync(string phoneNumber)
    {
        var normalized = Normalize(phoneNumber);
        return await _context.PhoneNumbers
            .AnyAsync(p => p.Number == normalized);
    }

    public async Task AddAsync(string phoneNumber)
    {
        var normalized = Normalize(phoneNumber);

        var entity = new PhoneNumberEntity // ← use alias
        {
            Number = normalized,
            CreatedAt = DateTime.UtcNow
        };

        _context.PhoneNumbers.Add(entity);
        await _context.SaveChangesAsync();
    }
}