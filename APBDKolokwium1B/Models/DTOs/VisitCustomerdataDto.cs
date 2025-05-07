namespace APBDKolokwium1B.Models.DTOs;

public class VisitCustomerdataDto
{
    public DateTime Date { get; set; }
    public ClientDetailsDto Client { get; set; }
    public MechanicDetailsDto Mechanic { get; set; } 
    public List<ServicesDetailsDto> VisitServices { get; set; } = [];
}

public class ClientDetailsDto
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public DateTime DateofBirth { get; set; }
}

public class MechanicDetailsDto
{
    public int MechanicId { get; set; }
    public string LicenceNumber { get; set; } = String.Empty;
}

public class ServicesDetailsDto
{
    public string Name { get; set; } = String.Empty;
    public decimal ServiceFee { get; set; }
}