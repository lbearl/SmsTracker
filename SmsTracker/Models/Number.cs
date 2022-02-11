using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmsTracker.Models;

public class Number
{
    [Key]
    public int Id { get; set; }
    public string PublicName { get; set; }
    [Phone, Required]
    public string PhoneNumber { get; set; }
    public string? Notes { get; set; }
    
    public int AccountId { get; set; }
    
    [ForeignKey(nameof(AccountId))]
    public Account Account { get; set; }
}