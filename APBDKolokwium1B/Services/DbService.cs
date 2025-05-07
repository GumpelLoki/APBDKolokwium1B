using System.Data.Common;
using APBDKolokwium1B.Models.DTOs;

namespace APBDKolokwium1B.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;
    public DbService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }

    public async Task<VisitCustomerdataDto> GetVisitDataByIdAsync(int visitid)
    {
        var query = @"SELECT date, c.first_name, c.last_name, c.date_of_birth, m.mechanic_id, m.licence_number, vs.service_fee, s.name
        FROM Visit v
        JOIN Client c ON v.client_id = c.client_id
        JOIN Mechanic m ON v.mechanic_id = m.mechanic_id
        JOIN Visit_Service vs ON v.visit_id = vs.visit_id
        JOIN Service s ON vs.service_id = s.service_id
        WHERE v.visit_id = @visitid;";
        
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = query;
        await connection.OpenAsync();
        
        command.Parameters.AddWithValue("@visitid", visitid);
        var reader = await command.ExecuteReaderAsync();
        
        VisitCustomerdataDto? visits = null;

        while (await reader.ReadAsync())
        {
            if (visits is null)
            {
                visits = new VisitCustomerdataDto
                {
                    Date = reader.GetDateTime(0),
                    Client = new ClientDetailsDto()
                    {
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        DateofBirth = reader.GetDateTime(3),
                    },
                    Mechanic = new MechanicDetailsDto()
                    {
                        MechanicId = reader.GetInt32(4),
                        LicenceNumber = reader.GetString(5),
                    },
                    VisitServices = new List<ServicesDetailsDto>()
                };
            }
            string servicename = reader.GetString(7);
            var service = visits.VisitServices.FirstOrDefault(e => e.Name.Equals(servicename));
            if (service is null)
            {
                service = new ServicesDetailsDto()
                {
                    Name = servicename,
                    ServiceFee = reader.GetDecimal(6),
                };
                visits.VisitServices.Add(service);
            }
        }

        if (visits is null)
        {
            throw new NotFoundException("No visits found for this id");
        }




        return visits;
    }

    public async Task AddNewVisitDataAsync(string licencenumber, VisitCustomerdataDto visitCustomerdataDto)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;
        try
        {
            command.Parameters.Clear();
            command.CommandText = "Select 1 from Mechanic where licence_number = @licencenumber";
            command.Parameters.AddWithValue("@licencenumber", licencenumber);
            
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


}