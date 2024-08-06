using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    class Account
    {
        //list of transactions
        public string Name;
        public int AccountNumber;
        public int Balance;

        //List of transactions

        LinkedList<Transaction> transactionslist;

        public Account(string name, int accountNumber, int balance)
        {
            Name = name;
            AccountNumber = accountNumber;
            Balance = balance;
            transactionslist = new LinkedList<Transaction>();
        }

        public  void AddTrascation(int AccountNumber, int  amount,string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {

                string query = "INSERT INTO Transactions(AccountNumber,Debit,Credit) VALUES (@AccountNumber,@Debit,@Credit)";
                 SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                cmd.Parameters.AddWithValue("@Debit", amount);
                cmd.Parameters.AddWithValue("@Credit", 0);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Transtions added successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);  
                }
            }
           
            

        }
        public void Withdraw(int AccountNumber, int amount, string connectionString)
        {

            using (var conn = new SqlConnection(connectionString))
            {

                string query = "INSERT INTO Transactions(AccountNumber,Debit,Credit) VALUES (@AccountNumber,@Debit,@Credit)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber);
                cmd.Parameters.AddWithValue("@Debit", 0);
                cmd.Parameters.AddWithValue("@Credit", amount);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Transtions added successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
 
        

    }
}
