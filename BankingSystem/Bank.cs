using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace BankingSystem
{
    class Bank
    {


        LinkedList<Account> accountlist;

        public Bank()
        {
            accountlist = new LinkedList<Account>();
        }

        public string connectionString = "Server=localhost;Database=BankingSystem;Integrated Security=True;";
        public static void MainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("======================= *** WELCOME TO HBL BANK *** =======================");
            Console.WriteLine();
            Console.WriteLine("=============================== *** ENTER YOUR CHOICE *** ===============================");
            Console.WriteLine();
            Console.WriteLine("1]. ADD Account \t\t\t 2].Delete Account \t\t\t 3].Check Balance");
            Console.WriteLine();
            Console.WriteLine("4]. ADD Funds \t\t\t 5].Withdraw funds      \t\t\t 6].print Transtions");
            Console.WriteLine();
            Console.WriteLine("=============================== *** ENTER 0 TO EXIT *** ===============================");
        }

        public void AddAccount()
        {
            Console.WriteLine("Enter the Name:");
            string Name = Console.ReadLine();
            Console.WriteLine("Enter the Account number:");
            int AccountNumber = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Balance:");
            int Balance = int.Parse(Console.ReadLine());


            var conn = new SqlConnection(connectionString);

            string query = "INSERT INTO Accounts(AccountNumber,Name,Balance) VALUES (@AccountNumber,@Name,@Balance)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Balance", Balance);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Account added successfully!");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }


            Start();

        }
        public void RemoveAccount(int number)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Accounts WHERE AccountNumber=@AccountNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", number);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Account has successfull remove");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Start();
        }



        public void AddFundsTransaction(int accountNumber, int amount)
        {


            using (var conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Accounts SET Balance=Balance+@Amount WHERE AccountNumber=@AccountNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                cmd.Parameters.AddWithValue("@Amount", amount);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("UPdated Successfully:");
                    Account account = new Account("", 0, 0);
                    account.AddTrascation(accountNumber, amount, connectionString);
                    Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Start();
                }


            }



        }
        public void WithDrawTransaction(int accountNumber, int amount)
        {

            using (var conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Accounts SET Balance=Balance-@Amount WHERE AccountNumber=@AccountNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                cmd.Parameters.AddWithValue("@Amount", amount);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("UPdated Successfully:");
                    Account account = new Account("", 0, 0);
                    account.Withdraw(accountNumber, amount, connectionString);
                    Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Start();
                }

            }
        }
        public void CheckBalance(int userAccountNumber)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = " SELECT Balance from Accounts WHERE AccountNumber=@userAccountNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userAccountNumber", userAccountNumber);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"Account Balance: {reader["Balance"]}");
                        Start();
                    }
                    else
                    {
                        Console.WriteLine("Account Not Found:");
                        Start();
                    }
                }

            }
        }

        public void PrintTransaction(int accountNumber)
        {
            using(var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT  *FROM Transactions WHERE AccountNumber =@AccountNumber ";
                SqlCommand cmd=new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    foreach(var reade in reader)
                    {
                        int debit = (int)reader["Debit"];
                        int credit = (int)reader["Credit"];
                        Console.WriteLine($"Debit:{debit} Credit:{credit}");
                    }

                }
            }
        }
        public void Start()
        {
            MainMenu();
            Console.WriteLine("Enter the choice:");
            int choice = int.Parse(Console.ReadLine());
            while (true)
            {
                switch (choice)
                {
                    case 1:
                        AddAccount();
                        break;
                    case 2:
                        Console.WriteLine("Enter the Account Number to Remove:");
                        int number = int.Parse(Console.ReadLine());
                        RemoveAccount(number);
                        break;
                    case 3:
                        Console.WriteLine("Enter the Account Number to Check Balance:");
                        int balance = int.Parse(Console.ReadLine());
                        CheckBalance(balance);
                        break;
                    case 4:
                        Console.WriteLine("Enter the Account Number to Add Funds:");
                        int accountNumber = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the amount to add:");
                        int amount = int.Parse(Console.ReadLine());
                        AddFundsTransaction(accountNumber, amount);
                        break;
                    case 5:
                        Console.WriteLine("Enter the Account Number to withdraw Funds:");
                        int accountNum = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the amount to withdraw:");
                        int totalamount = int.Parse(Console.ReadLine());
                        WithDrawTransaction(accountNum, totalamount);
                        break;
                        case 6:
                        Console.WriteLine("Enter the Account Number to Print Transtions:");
                        int accountNumb = int.Parse(Console.ReadLine());
                        PrintTransaction(accountNumb);
                        break;


                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }


    }
}
