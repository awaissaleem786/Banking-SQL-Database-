using BankingSystem;
using System.Net;

class Transaction
{
     public Transaction(decimal debit, decimal credit)
    {
        Debit = debit;
       Credit = credit;
    }

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }
    public override string ToString()
    {
        return $"Transaction Debit Amount:{Debit} " + $" Transcation Credit Amount:{ Credit}";
    }
}