using NewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Reflection.Emit;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Web.Services.Description;

namespace NewApp.Controllers
{
    public class StylistController : Controller
    {
        // GET: Stylist
        public static string visibililtyResult;

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult StylistPage()
        {

            if (Session["uname"] != null)
            {
                //int id = (int)Session["stylistId"];
                DateTime? subscriptionDueDate = DateTime.Now;
                float rating = 0;
                string backgroundImage = "";
                List<serviceList> servicesList = new List<serviceList>();
                string conString = null;
                MySqlConnection con = null;
                //conString = "server=localhost;database=roamingrazorsdb;uid=root;pwd=manjiRoot@26;";
                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                MySqlCommand cmd = con.CreateCommand();
                MySqlDataReader reader;
                //cmd.CommandText = "SELECT s.Full_Name, s.Background_Image, se.Service_Name, se.Price,se.Description FROM stylist s JOIN services se ON s.Stylist_ID = se.Stylist_ID WHERE s.stylist_id = '"+stylistKey+"' AND se.Status_ID = '1'";
                cmd.CommandText = "select s.Background_Image,s.Subscription_Due_Date, sv.Service_Name,sv.Price,sv.Service_ID,sv.Description,st.Status AS Status_Name, COUNT(r.Stylist_ID) AS review_count,left(SUM(r.Rating)/COUNT(r.Stylist_ID),3)  as Rating from stylist as s join stylist_rating as r on s.Stylist_ID = r.stylist_id join services as sv on s.Stylist_ID = sv.Stylist_ID JOIN roamingrazorsdb.status as st ON sv.Status_ID = st.Status_ID where s.Stylist_ID='" + (int)Session["stylistId"] + "' GROUP BY r.Stylist_ID,sv.Service_ID;";
                try
                {

                    con.Open();
                    string Description;
                    
                    //MessageBox.show("");
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        subscriptionDueDate = reader.GetDateTime("Subscription_Due_Date");
                        //subscriptionDueDate = reader["Subscription_Due_Date"] as DateTime?;
                        


                        if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                        {
                            Description = reader.GetString("Description");
                        }
                        else
                        {
                            Description = "no Description";

                        }
                        //check bg image
                        if (!reader.IsDBNull(reader.GetOrdinal("Background_Image")))
                        {
                            backgroundImage = reader.GetString("Background_Image");
                        }
                        else
                        {
                            backgroundImage = "../Content/img/index/BlankBackground.jpg";
                            //backgroundImage = "~/beardMan.png";
                        }
                        rating = reader.GetFloat("Rating");

                        serviceList services = new serviceList
                        {
                            Price = reader.GetFloat("Price"),
                            Description = Description,
                            ServiceID = reader.GetInt32("Service_ID"),
                            ServiceName = reader.GetString("Service_Name"),
                            StatusName = reader.GetString("Status_Name")

                            //backgroundImg = reader.GetString("Backgound_Image")
                        };
                        servicesList.Add(services);
                        //servicesList.Add(new serviceList
                        //{
                        //    Price = reader.GetFloat("Price"),
                        //    Description = reader.GetString("Description"),
                        //    ServiceID = reader.GetInt32("Service_ID"),
                        //    ServiceName = reader.GetString("Service_Name")
                        //});
                        //servicesList.Add(services);
                    }
                    con.Close();

                    //StylistServiceList.Add(services);
                }
                catch (Exception ex)
                {

                }

                StylistServiceList servicePage = new StylistServiceList
                {
                    ListOfServices = servicesList,
                    rating = rating,
                    backgroundImg = backgroundImage,
                    stylistName = (string)Session["Fullname"]
                };
                //if (subscriptionDueDate == null || subscriptionDueDate <= DateTime.Now)
                //{
                //    return View("~/Views/Stylist/StylistSubscription.cshtml");
                //    //return RedirectToAction("StylistSubscription", "Stylist");
                //}

                if (visibililtyResult == "1")
                {
                    ViewBag.visibilityResult = true;
                }
                else
                {
                    ViewBag.visibilityResult = false;
                }
                return View(servicePage);
            }
            else
            {
                return View("~/Views/Home/Index.cshtml");
            }
            //return View();
        }

        [HttpPost]
        public ActionResult RemoveService(string serviceID)
        {
            if (!string.IsNullOrEmpty(serviceID)) {
                string conString = null;
                MySqlConnection con = null;

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                string insertQuery = "DELETE FROM services WHERE Service_ID = '"+int.Parse(serviceID)+"';";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();
            }
            return RedirectToAction("StylistPage","Stylist");
        }

        [HttpPost]
        public ActionResult AddService(string serviceName,string Price,string stylistkey, string description = "")
        {
        
            if (!string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(Price)) {
                string conString = null,insertQuery = null;
                MySqlConnection con = null;

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                if(description != null && description != "") {
                    insertQuery = "INSERT INTO `roamingrazorsdb`.`services` ( `Stylist_ID`, `Service_Name`, `Price`, `Description`, `Status_ID`) VALUES ('"+stylistkey+"','"+serviceName+"','"+Price+"','"+description+"','2');";
                }
                else
                {
                    insertQuery = "INSERT INTO `roamingrazorsdb`.`services` ( `Stylist_ID`, `Service_Name`, `Price`, `Status_ID`) VALUES ('"+stylistkey+"','"+serviceName+"','"+Price+"','2');";
                }
                
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();

            }
            return RedirectToAction("StylistPage", "Stylist");
        }

        [HttpPost]  
        public ActionResult ChangeBgImage(string stylistkey,string newBackgroundImage="")
        {
            if (!string.IsNullOrEmpty(stylistkey) && newBackgroundImage!="")
            {
                string conString = null;
                MySqlConnection con = null;

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                string insertQuery = "UPDATE stylist SET Background_Image = '"+newBackgroundImage+"' WHERE Stylist_ID = '"+int.Parse(stylistkey)+"';";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();
            }
            
            return RedirectToAction("StylistPage", "Stylist");
        }
        
        [HttpPost]  
        public ActionResult ChangeVisibility(string stylistkey,string switchStylistValue)
        {
            if (!string.IsNullOrEmpty(stylistkey))
            {
                string conString = null;
                MySqlConnection con = null;

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                string insertQuery = "UPDATE stylist SET Is_Active = '"+int.Parse(switchStylistValue)+"' WHERE Stylist_ID = '"+int.Parse(stylistkey)+"';";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);
                MySqlDataReader reader1;
                con.Open();
                reader1 = command.ExecuteReader();
                con.Close();
            }
            visibililtyResult = switchStylistValue;

            return RedirectToAction("StylistPage", "Stylist");
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Signout()
        {
            string conString = null;
            MySqlConnection con = null;

            conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
            con = new MySqlConnection(conString);
            string insertQuery = "UPDATE stylist SET Is_Active = '0' WHERE Stylist_ID = '" + Session["stylistId"] + "';";
            //Insert logic
            MySqlCommand command = new MySqlCommand(insertQuery, con);
            MySqlDataReader reader1;
            con.Open();
            reader1 = command.ExecuteReader();
            con.Close();

            Session["uname"] = null;
            Session.Abandon();

            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult StylistSubscription()
        {
            //if (TempData["paymentStatusStylistDisplay"] != null)
            //{
            //    ViewBag.paymentStatusStylistDisplay = TempData["paymentStatusStylistDisplay"];
            //    ViewBag.SuccessStylistReference = TempData["SuccessStylistReference"];
            //}
            //else
            //{
            //    ViewBag.paymentStatusStylistDisplay = "none";
            //    ViewBag.SuccessStylistReference = "No ID";         
            //}

            return View();
        }

    }
}