using APBDKolokwium1B.Models.DTOs;

namespace APBDKolokwium1B.Services;

public interface IDbService
{
    Task<VisitCustomerdataDto> GetVisitDataByIdAsync(int visitid);
    Task AddNewVisitDataAsync(string licencenumber,VisitCustomerdataDto visitCustomerdataDto);
}