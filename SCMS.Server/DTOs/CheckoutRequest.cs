namespace SCMS.Server.DTOs;

public class CheckoutRequest
{
    public int StudentId { get; set; }
    public int? LoanDays { get; set; }
}
