using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewApp.Models
{
    public class AdminApprovalRequests
    {
        public List<StylistApprovalList> ListOfStylist { get; set; }
        public List<ServiceApprovalList> ListOfServices { get; set; }
        public List<TransactionList> ListOfTransaction { get; set; }
    }

    public class StylistApprovalList
    {
        public int StylistId { get; set; }
        public string StylistName { get; set; }
        public string QualificationCertificate { get; set; }
        public string BackgroundCheck { get; set; }
    }
    public class ServiceApprovalList
    {
        public int ServiceId { get; set; }
        public string StylistName { get; set; }
        public string ServiceName { get; set; }
        public string Description{ get; set; }
        public float Price { get; set; }
    }

    public class TransactionList
    {
        public int TransactionId { get; set; }
        public int OrderId { get; set; }

        public string CustomerName {  get; set; }
        public string StylistName { get; set; }
        public string TransactionSuccessfull {  get; set; }
        public float TransactionAmount { get; set; }
        public float StylistAmount { get; set; }
        public float CompanyAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CustomerTransactionId { get; set; }
        public string PaypalTransactionId { get; set; }
    }
}