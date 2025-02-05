using Microsoft.AspNetCore.Identity;
using Expense_Tracker.Models;

public class User : IdentityUser
{
    

    // Navigation property for transactions
    public ICollection<Transaction> Transactions { get; set; }
}