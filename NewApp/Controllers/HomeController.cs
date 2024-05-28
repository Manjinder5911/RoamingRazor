using MySql.Data.MySqlClient;
using NewApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Pluralization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NewApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Help page.";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();  
        }

        

        [HttpPost]

      
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult SigninCustomer(string userName, string loginPassword)
        {
            int idnumber = 0;
          
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(loginPassword))
                {
                string conString = null;
                MySqlConnection con = null;
               
                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                MySqlCommand cmd = con.CreateCommand();
                MySqlDataReader reader;
                cmd.CommandText = "SELECT Customer_ID,Full_Name,Address,Email,Phone_Number from customer WHERE Customer_Name='" + userName + "' and Customer_Pwd='" + loginPassword + "'";
                
                try
                {
                    con.Open();
                    //MessageBox.show("");
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        idnumber = (int)reader["Customer_ID"];
                        if(idnumber != 0)
                        {
                            Session["Fullname"] = reader.GetString("Full_Name");
                            Session["Address"] = reader.GetString("Address");
                            Session["Email"] = reader.GetString("Email");
                            Session["PhoneNumber"] = reader.GetString("Phone_Number");
                           
                        }
                        
                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }

            }

            if (idnumber == 0)
            {
                ViewBag.SlideOutClass = "animateSlideOut";
                ViewBag.SlideInClass = "animateSlideIn";
                ViewBag.loginCustomerDisplay = "block";
                ViewBag.wrongPasswordCustomer = "Wrong Password";
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                Session["user"] = "customer";
                Session["uname"] = userName;
                Session["customerID"] = idnumber;
                return RedirectToAction("Index", "LoginCustomer");
            }

        } 
        [HttpPost]
        
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult SigninStylist(string userName, string loginPassword)
        {
            int idnumber = 0;

            //string uname = userName.Trim();
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(loginPassword))
                {
                string conString = null;
                MySqlConnection con = null;
               
                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                MySqlCommand cmd = con.CreateCommand();
                MySqlDataReader reader;
                cmd.CommandText = "SELECT Stylist_ID,Full_Name,Address,Email,Phone_Number,Status_ID,Background_Image from stylist WHERE Stylist_Name='" + userName + "' and Stylist_Pwd='" + loginPassword + "'";
                
                try
                {
                    con.Open();
                    //MessageBox.show("");
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        idnumber = (int)reader["Stylist_ID"];
                        //gather other info if user exists
                        if (idnumber != 0)
                        {
                            int statusId = (int)reader["Status_ID"];
                            
                            //if status not approved.Send to waiting Page
                            if (statusId != 1)
                            {
                                return View("~/Views/Home/PendingStylistStatusPage.cshtml");
                               
                            }
                            
                          
                            Session["Fullname"] = reader.GetString("Full_Name");
                            Session["Address"] = reader.GetString("Address");
                            Session["Email"] = reader.GetString("Email");
                            Session["PhoneNumber"] = reader.GetString("Phone_Number");
                        }
                        

                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }

            }

            if (idnumber == 0)
            {
                ViewBag.SlideOutClass = "animateSlideOut";
                ViewBag.SlideInClass = "animateSlideIn";
                ViewBag.loginStylistDisplay = "block";
                ViewBag.wrongPasswordStylist = "Wrong Password";
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                Session["user"] = "stylist";
                Session["uname"] = userName;
                Session["stylistId"] = idnumber;
                if (Session["uname"] != null)
                {
                    return RedirectToAction("StylistPage", "Stylist");
                }
                else
                {
                    return View("~/Views/Home/Index.cshtml");
                }
                
            }

        }

        

        [HttpPost]
        public ActionResult Signup_cust(string fullName, string email, string password, string phoneNumber, string customerUserName, string stNumber,string city, string zipCode,string state,string country)
        {
            int idnumber = 0;

            string uname = customerUserName.Trim();
            string pwd = password.Trim();


                string conString = null;
                MySqlConnection con = null;
                
                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                MySqlCommand cmd = con.CreateCommand();
                MySqlDataReader reader;
                //cmd.CommandText = "select Customer_ID from customer where Customer_Name= '" + uname + "'";
                cmd.CommandText = "SELECT Customer_ID, CASE WHEN Customer_Name = '"+uname+"' THEN Customer_Name WHEN Email = '"+email+"' THEN Email END AS matched_column FROM customer WHERE Customer_Name = '"+uname+ "' OR Email = '"+email+"'";

           
                try
                {
                    con.Open();
                    //MessageBox.show("");
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                    if (reader.GetString("matched_column") == uname) { ViewBag.custUserNameError = "Username already in use (required for login)"; }
                    if (reader.GetString("matched_column") == email) { ViewBag.custEmailError = "Email already in use"; }
                        idnumber = (int)reader["Customer_ID"];

                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }

           

            if (idnumber == 0)
            {
                string insertQuery = "INSERT INTO `roamingrazorsdb`.`customer` ( `Customer_Name`, `Customer_Pwd`, `Full_Name`, `Address`, `Email`, `Phone_Number`, `Status_ID`) VALUES ( '" + uname + "', '" + pwd + "', '" + fullName + "', '" + stNumber + "," + city + "," + state + "," + country + "," + zipCode + "', '" + email + "', '" + phoneNumber + "', '1');";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open() ;
                reader1=command.ExecuteReader();
                con.Close();
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                ViewBag.SlideOutClass = "animateSlideOut";
                ViewBag.SlideInCustSignUp = "animateSlideIn";
                ViewBag.displayCustSignUp = "block";
                ViewBag.displayStylistSignUp = "none";
                return View("~/Views/Home/Index.cshtml");
            }

            
        }
        [HttpPost]
        public ActionResult Signup_stylist(string fullName, string email, string password, string phoneNumber, string address, string stylistUserName, string stylistCertificate, string stylistCriminalCheck)
        {
           
            int idnumber = 0;

            string uname = stylistUserName.Trim();
            string pwd = password.Trim();


            string conString = null;
            MySqlConnection con = null;

            conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
            con = new MySqlConnection(conString);
            MySqlCommand cmd = con.CreateCommand();
            MySqlDataReader reader;
            cmd.CommandText = "SELECT Stylist_ID, CASE WHEN Stylist_Name = '" + uname + "' THEN Stylist_Name WHEN Email = '" + email + "' THEN Email END AS matched_column FROM stylist WHERE Stylist_Name = '" + uname + "' OR Email = '" + email + "'";


            try
            {
                con.Open();
               
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString("matched_column") == uname) { ViewBag.stylUserNameError = "Username already in use (required for login)"; }
                    if (reader.GetString("matched_column") == email) { ViewBag.stylEmailError = "Email already in use"; }
                    idnumber = (int)reader["Stylist_ID"];

                }
                con.Close();
            }
            catch (Exception ex)
            {

            }



            if (idnumber == 0)
            {
                string insertQuery = "INSERT INTO `roamingrazorsdb`.`stylist` ( `Stylist_Name`, `Stylist_Pwd`, `Full_Name`, `Email`,`Address`,`Phone_Number`, `Status_ID`,`Certificate_Of_Qualification`,`Criminal_Record_Check`,`Subscription_Due_Date`) VALUES ( '" + uname + "', '" + pwd + "', '" + fullName + "', '" + email + "','" + address + "','" + phoneNumber + "', '2','" + stylistCertificate + "','" + stylistCriminalCheck + "',Now()); ";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                //Throw error - username duplicate
                
                ViewBag.SlideOutClass = "animateSlideOut";
                ViewBag.SlideInCustSignUp = "animateSlideIn";
                ViewBag.displayCustSignUp = "none";
                ViewBag.displayStylistSignUp = "block";
                return View("~/Views/Home/Index.cshtml");
            }


        }

        public ActionResult PendingStylistStatusPage()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult SigninAdmin(string userName,string loginPassword)
        {
            int idnumber = 0;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(loginPassword))
            {
                string conString = null;
                MySqlConnection con = null;
                
                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                MySqlCommand cmd = con.CreateCommand();
                MySqlDataReader reader;
                cmd.CommandText = "SELECT Admin_ID from admin WHERE Admin_Name='" + userName + "' and Admin_Pwd='" + loginPassword + "'";

                try
                {
                    con.Open();
                    //MessageBox.show("");
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        idnumber = (int)reader["Admin_ID"];
                        //if (idnumber != 0)
                        //{
                        //    Session["Fullname"] = reader.GetString("Full_Name");
                        //    Session["Address"] = reader.GetString("Address");
                        //    Session["Email"] = reader.GetString("Email");
                        //    Session["PhoneNumber"] = reader.GetInt32("Phone_Number");

                        //}

                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }

            }

            if (idnumber == 0)
            {
                ViewBag.wrongPasswordAdmin = "Wrong Password";
                return View("~/Views/Home/AdminLogin.cshtml");
            }
            else
            {
                Session["user"] = "admin";
                Session["uname"] = userName;
                Session["customerID"] = idnumber;
                return RedirectToAction("AdminPage", "Admin");
            }
            
        }




    }
}
