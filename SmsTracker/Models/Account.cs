using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SmsTracker.Models;

public class Account
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string AccountName { get; set; }
    
    [Phone, Required]
    public string PrimaryPhone { get; set; }
    
    public virtual ICollection<Number> AssociatedNumbers { get; set; } = new List<Number>();
    
    [ForeignKey(nameof(OwnedByUserId))]
    public IdentityUser OwnedByUser { get; set; }
    public string OwnedByUserId { get; set; }
}