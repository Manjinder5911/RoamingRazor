using NewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Web.Services.Description;

namespace NewApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult AdminPage()
        {
            if (Session["uname"] != null)
            {
                List<StylistApprovalList> StylistList = new List<StylistApprovalList>();
                List<ServiceApprovalList> ServiceList = new List<ServiceApprovalList>();
                List<TransactionList> TransactionList = new List<TransactionList>();
                string conString = null;
                MySqlConnection con = null;
                //conString = "server=localhost;database=roamingrazorsdb;uid=root;pwd=manjiRoot@26;";
                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                MySqlCommand cmd = con.CreateCommand();
                MySqlDataReader reader;
                cmd.CommandText = "Select Stylist_ID,Full_Name,Certificate_Of_Qualification,Criminal_Record_Check from stylist where Status_ID = '2';";
                try
                { 
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        StylistApprovalList stylists = new StylistApprovalList
                        {
                            StylistId = reader.GetInt32("Stylist_ID"),
                            StylistName = reader.GetString("Full_Name"),
                            QualificationCertificate = reader.GetString("Certificate_Of_Qualification"),
                            BackgroundCheck = reader.GetString("Criminal_Record_Check")
                        };
                        StylistList.Add(stylists);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }
                
                MySqlDataReader reader1;
                cmd.CommandText = "SELECT s.*, st.Full_Name FROM services s INNER JOIN stylist st ON s.Stylist_ID = st.Stylist_ID WHERE s.Status_ID = '2';";
                try
                { 
                    con.Open();
                    string Description;
                    reader1 = cmd.ExecuteReader();
                    while (reader1.Read())
                    {
                        if (!reader1.IsDBNull(reader1.GetOrdinal("Description")))
                        {
                            Description = reader1.GetString("Description");
                        }
                        else
                        {
                            Description = "no Description";

                        }
                        ServiceApprovalList services = new ServiceApprovalList
                        {
                            ServiceId = reader1.GetInt32("Service_ID"),
                            StylistName = reader1.GetString("Full_Name"),
                            ServiceName = reader1.GetString("Service_Name"),
                            Description = Description,
                            Price = reader1.GetFloat("Price")
                        };
                        ServiceList.Add(services);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }
                
                MySqlDataReader reader2;
                cmd.CommandText = "SELECT t.Transaction_ID,t.Order_ID,t.Transaction_Successful,t.Transaction_Amount,t.Stylist_Amount,t.Date_Time,t.Customer_Reference_ID,t.Paypal_Transaction_ID,c.Full_Name as customerName,s.Full_Name as stylistName FROM transaction t JOIN roamingrazorsdb.order o ON t.Order_ID = o.Order_ID JOIN customer c ON o.Customer_ID = c.Customer_ID JOIN stylist s ON o.Stylist_ID = s.Stylist_ID;";
                try
                { 
                    con.Open();
                    string TransactionSuccess="";
                    float companyAmount;
                    reader2 = cmd.ExecuteReader();
                    while (reader2.Read())
                    {
                        if (reader2.GetInt32("Transaction_Successful") == 0)
                        {
                            TransactionSuccess = "Failed";
                        }
                        else if(reader2.GetInt32("Transaction_Successful") == 1)
                        {
                            TransactionSuccess = "Success";
                        }
                        else
                        {
                            TransactionSuccess = "Error";
                        }
                        companyAmount = (float)Math.Round(reader2.GetFloat("Transaction_Amount") - reader2.GetFloat("Stylist_Amount"),2);
                        TransactionList Transaction = new TransactionList
                        {
                            TransactionId = reader2.GetInt32("Transaction_ID"),
                            OrderId = reader2.GetInt32("Order_ID"),
                            CustomerName = reader2.GetString("customerName"),
                            StylistName = reader2.GetString("stylistName"),
                            TransactionSuccessfull = TransactionSuccess,
                            TransactionAmount = reader2.GetFloat("Transaction_Amount"),
                            StylistAmount = reader2.GetFloat("Stylist_Amount"),
                            CompanyAmount = companyAmount,
                            TransactionDate = reader2.GetDateTime("Date_Time"),
                            CustomerTransactionId = reader2.GetString("Customer_Reference_ID"),
                            PaypalTransactionId = reader2.GetString("Paypal_Transaction_ID")
                        };
                        TransactionList.Add(Transaction);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }
                AdminApprovalRequests ApprovalRequest = new AdminApprovalRequests
                {
                    ListOfStylist = StylistList,
                    ListOfServices = ServiceList,
                    ListOfTransaction = TransactionList
                };
                return View(ApprovalRequest);


            }
            else
            {
                return View("~/Views/Home/AdminLogin.cshtml");
            }
            
        }
        [HttpPost]
        public ActionResult ApproveStylist(string StylistId)
        {
            if (!string.IsNullOrEmpty(StylistId))
            {
                string conString = null;
                MySqlConnection con = null;

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                string insertQuery = "UPDATE stylist SET Status_ID = '1' WHERE Stylist_ID = '" + int.Parse(StylistId) + "';";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();
            }

            return RedirectToAction("AdminPage", "Admin");
        }
        
        [HttpPost]
        public ActionResult RejectStylist(string StylistId)
        {
            if (!string.IsNullOrEmpty(StylistId))
            {
                string conString = null;
                MySqlConnection con = null;

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                string insertQuery = "UPDATE stylist SET Status_ID = '5' WHERE Stylist_ID = '" + int.Parse(StylistId) + "';";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();
            }

            return RedirectToAction("AdminPage", "Admin");
        }

        [HttpPost]
        public ActionResult ApproveService(string serviceId)
        {
            if (!string.IsNullOrEmpty(serviceId))
            {
                string conString = null;
                MySqlConnection con = null;

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                string insertQuery = "UPDATE services SET Status_ID = '1' WHERE Service_ID = '" + int.Parse(serviceId) + "';";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();
            }

            return RedirectToAction("AdminPage", "Admin");
        }

        [HttpPost]
        public ActionResult RejectService(string serviceId)
        {
            if (!string.IsNullOrEmpty(serviceId))
            {
                string conString = null;
                MySqlConnection con = null;

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                string insertQuery = "UPDATE services SET Status_ID = '5' WHERE Service_ID = '" + int.Parse(serviceId) + "';";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();
            }

            return RedirectToAction("AdminPage", "Admin");
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Signout()
        {
            Session["uname"] = null;
            Session.Abandon();

            return View("~/Views/Home/AdminLogin.cshtml");
        }
    }
}