namespace DataAccess.Models;

public class Payment
{
    public int PaymentId { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string? PaymentMethod { get; set; } // CreditCard, BankTransfer, Cash
    public string? TransactionId { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
    public DateTime? PaymentDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation property
    public virtual Booking Booking { get; set; } = null!;
}
