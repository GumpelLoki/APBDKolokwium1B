namespace APBDKolokwium1B.Models.DTOs;

public class CreateVisitDto
{
    public int VisitId { get; set; }
    public int ClientId { get; set; }
    public string MechanicLicenceNumber { get; set; } = String.Empty;
    public List<ServicesInputDto> Services { get; set; }
}

public class ServicesInputDto
{
    public string ServiceName { get; set; } = String.Empty;
    public decimal ServiceFee { get; set; }
}