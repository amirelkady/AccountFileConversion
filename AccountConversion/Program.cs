/*
 * Created By : Amir Elkady
 * Date Created : 01/09/2019
Please review the following assumptions
1. I didn't do data validation as I assumed that the spreadsheet is already checked prior and data loadedd in the specified data types.
2. Not sure why the column headers will matter in case of custom output so I added a function to display data with the original headers.
3. I used only one data item in my collection, otherwise I would have created a list or item collection to handle the entries to display the header on top only once.
4. Created function displayOriginalOutput to display the values with its original headers.
5. Made displayOutput virtual in case you need to override it to display different output for each child class
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountFile normalFile = new AccountFile("AccountCode,Name,Type,OpenDate,Currency", "AbcCode", "MyAccount","RRSP", "2018-01-01", "CAD");
            AccountFileType1 sampleType1 = new AccountFileType1("Identifier,Name,Type,Opened,Currency", "123|AbcCode", "MyAccount", "2", "01-01-2018", "CD");
            AccountFileType2 sampleType2 = new AccountFileType2("MyAccount", "RRSP", "C", "AbcCode");

            normalFile.displayOutput();
            sampleType1.displayOutput();
            sampleType2.displayOutput();
        }
    }

    public class AccountFile
    {
        protected string accountCode { get; set; }
        protected string name = string.Empty;

        protected DateTime? openDate;

        protected string[] header = { "AccountCode", "Name", "Type", "Opendate", "Currency" };
        protected string type;
        protected string currency;

        public AccountFile() { }
        public AccountFile(string Header, string accountCode, string name, string type, string opendate, string currency)
        {
            this.header = Header.Split(',');
            this.accountCode = accountCode;
            this.name = name;
            this.type = type;
            this.openDate = DateTime.Parse(opendate);
            this.currency = currency.ToUpper();                        
        }

        public virtual void displayOutput()
        {
            foreach(string headeritem in header)
            {
                Console.WriteLine(headeritem);
            }

            Console.WriteLine(this.accountCode);
            Console.WriteLine(this.name);
            Console.WriteLine(this.type);
            if (this.openDate != null)
                Console.WriteLine(this.openDate.Value.ToString("yyyy-MM-dd"));
            else Console.WriteLine();
            Console.WriteLine(this.currency);
            Console.WriteLine();
        }
    }

    public class AccountFileType1 : AccountFile
    {
        public string identifier;
        public DateTime openedDate;
        public string [] customHeader;
        public string customType;
        public string customerCurrency;
        
        public AccountFileType1(string header, string identifier, string name, string type, string opened, string currency)
        {
            this.accountCode = identifier.Substring(identifier.IndexOf("|") + 1);
            this.identifier = identifier;
            this.name = name;
            this.openedDate = DateTime.Parse(opened);
            this.openDate = new DateTime(openedDate.Year, openedDate.Month, openedDate.Day);
            this.customHeader = header.Split(',');
            this.customType = type;
            this.type = AccountType.getAccountTypeByID(int.Parse(customType));
            this.customerCurrency = currency;
            this.currency = Currency.getCurrencyName(currency);
        }

        public void displayOriginalOutput()
        {
            foreach (string headeritem in this.customHeader)
            {
                Console.WriteLine(headeritem);
            }

            Console.WriteLine(this.identifier);
            Console.WriteLine(this.name);
            Console.WriteLine(this.type);
            Console.WriteLine(this.openedDate.ToString("dd-MM-yyyy"));
            Console.WriteLine(this.currency);
            Console.WriteLine();
        }
    }

    public class AccountFileType2 : AccountFile
    {
        public string custodianCode;
        public string customCurrency;

        public AccountFileType2(string name, string type, string currency, string custodiancode)
        {
            this.name = name;
            this.type = type;
            this.customCurrency = currency;
            this.currency = Currency.getCurrencyName(currency);
            this.custodianCode = this.accountCode = custodiancode;
        }

        public void displayOriginalOutput()
        {
            Console.WriteLine(this.name);
            Console.WriteLine(this.type);
            Console.WriteLine(this.currency);
            Console.WriteLine(this.custodianCode);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Assume that there is a lookup table for the AccountTpe IDs and Values
    /// </summary>
    public class AccountType
    {
        public int id { get; set; }
        public static string typeName { get; set; }
        
        public static string getAccountTypeByID(int ID)
        {
            switch (ID)
            {
                case 1:
                    return typeName = "Trading";
                case 2:
                    return typeName = "RRSP";
                case 3:
                    return typeName = "RESP";
                case 4: 
                    return typeName = "Fund";
                default:
                    return typeName = "";
            }
        }
    }

    /// <summary>
    /// Assume that there is a table for currency with Name and Value
    /// </summary>
    public class Currency
    {
        static string name = string.Empty;
        
        public Currency() { }
        public static string getCurrencyName(string accountFileCurrencyValue)
        {
            switch (accountFileCurrencyValue.ToUpper())
            {
                case "CD":
                    return name = "CAD";
                case "US":
                    return name = "USD";
                case "C":
                    return name = "CAD";
                case "U":
                    return name = "USD";
                default:
                    return name = "";
            }
        }
    }
}
