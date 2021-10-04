using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using SyncEngine.Core;

namespace SyncEngine.Data
{
    public class Record
    {
        [Position(0)]
        public string JobNumber { get; set; }
        [Position(1, "FTCToBoolean")]
        public bool FTC { get; set; }
        [Position(8, "StringToFormattedString")]
        public string  CustomerName { get; set; }   
        [Position(10, "StringToFormattedString")]
        public string JobName { get; set; }
        [Position(9, "StringToFormattedString")]
        public string Address { get; set; }
        [Position(19, "StringToFormattedString")]
        public string City { get; set; }
        [Position(20, "StringToFormattedString")]
        public string State { get; set; }
        [Position(21, "StringToFormattedString")]
        public string Zip { get; set; }
        [Position(12, "StringToAmount")]
        public decimal Total { get; set; }
        [Position(2, "StringToBoolean")]
        public bool QQ { get; set; }
        [Position(17, "StringToFormattedString")]
        public string Contact { get; set; }
        public string ContactFirstName { get; set; } = String.Empty;
        public string ContactLastName { get; set; } = String.Empty;
        [Position(18, "StringToContactNumber")]
        public string ContactNumber { get; set; }
        [Position(11, "StringToFormattedString")]
        public string ContactEmail { get; set; }
        [Position(5, "StringToDateTime")]
        public DateTime ScheduledDelivery { get; set; }
        public string ScheduledDeliveryString { get; set; }
        [Position(14, "StringToFormattedString")]
        public string Product { get; set; }
        [Position(3, "StringToDateTime")]
        public DateTime QuoteIn { get; set; }
        public string QuoteInString { get; set; }
        [Position(4, "StringToDateTime")]
        public DateTime QuoteDone { get; set; }
        public string QuoteDoneString { get; set; }
        [Position(13, "StringToRoundedInt")]
        public int BDFT { get; set; }
        [Position(6, "StringToDateTime")]
        public DateTime Delivered { get; set; }
        [Position(16, "StringToDateTime")]
        public DateTime CanceledDate { get; set; }
        [Position(15, "StringToDateTime")]
        public DateTime LostDate { get; set; }
        public string DeliveredString { get; set; }
        public string Description { get; set; } = String.Empty;
        [Position(7, "StringToFormattedString")]
        public string SalesPerson { get; set; }
        private void SetQuoteIn() => QuoteInString = ConvertDate(QuoteIn);

        private void SetQuoteDone() => QuoteDoneString = ConvertDate(QuoteDone);

        private void SetDelivered() => DeliveredString = ConvertDate(Delivered);

        private void SetScheduledDelivery() => ScheduledDeliveryString = ConvertDate(ScheduledDelivery);

        private void SetDescription()
        {
            Description += String.IsNullOrEmpty(Address) ? "" : Address.Trim() + ", ";
            Description += String.IsNullOrEmpty(City) ? "" : City.Trim() + ", ";
            Description += String.IsNullOrEmpty(State) ? "" : State.Trim() + " ";
            Description += String.IsNullOrEmpty(Zip) ? "" : Zip.Trim();
            Description = Description.Trim();
        } 

        private void SetContactFirstName() 
        {
            if (!String.IsNullOrEmpty(Contact) && Contact.Contains(" "))
            {
                ContactFirstName = Contact.Split(" ")[0]; 
            }
            else 
            {
                ContactFirstName = Contact;
            }
        }

        private void SetContactLastName()
        {
            if (!String.IsNullOrEmpty(Contact) && Contact.Trim().Contains(" "))
            {
                var nameList = Contact.Split(" ").ToList();
                ContactLastName = nameList.Last();
            }
            else if (!String.IsNullOrEmpty(Contact))
            {
                ContactLastName = "";
            }
            else 
                ContactLastName = "NoName";
        } 

        public string ConvertDate(DateTime date) => date != DateTime.MinValue ? Convert.ToDateTime(date).ToString("yyyy-MM-dd") : "";

        public bool StringToBoolean(string data) => (data.ToLower().Trim() == "true") ? true : false;
        public bool FTCToBoolean(string data) => (data.ToLower().Trim() == "true" || data.ToLower().Trim() == "ftc") ? true : false;
        public decimal StringToAmount(string data)
        {
            var parsed = Decimal.Parse(data);
            return Decimal.Parse(parsed.ToString("0.##"));
        }

        public int StringToRoundedInt(string data)
            =>  Convert.ToInt32(Math.Round(Decimal.Parse(data)));
        
        public DateTime StringToDateTime(string data)
            =>  (String.IsNullOrEmpty(data.Trim())) ? DateTime.MinValue : Convert.ToDateTime(data.Trim());

        public string StringToFormattedString(string data)
            => data.Replace("|", ",").Replace("  ", " ").Trim();

        public string StringToContactNumber(string data)
        {
            var phone = StringToFormattedString(data).Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            var index = phone.ToLower().LastIndexOf("e");
            if (index > 0)
                return phone.Substring(0, index);
            else 
                return phone;
        }
            
        public void Initialize()
        {
            SetDescription();
            SetQuoteDone();
            SetQuoteIn();
            SetScheduledDelivery();
            SetDelivered();
            SetContactFirstName();
            SetContactLastName();
        }
    }
}