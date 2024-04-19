using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using NewApp.Models;
using PayPal.Api;
using Configuration = NewApp.Clients.Configuration;


namespace NewApp.Controllers
{
    public class LoginCustomerController : Controller
    {
        // GET: LoginCustomer
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            if (Session["uname"] != null)
            {
                List<StylistList> stylists = new List<StylistList>();
                string conString = null;
                MySqlConnection con = null;
                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                MySqlCommand cmd = con.CreateCommand();
                MySqlDataReader reader;
                cmd.CommandText = "select s.Stylist_Id, s.Background_Image, s.Full_Name, COUNT(r.Stylist_ID) AS review_count,left(SUM(r.Rating)/COUNT(r.Stylist_ID),3)  as Rating from stylist as s join stylist_rating as r on s.Stylist_ID = r.stylist_id where s.Status_ID='1' GROUP BY r.Stylist_ID;";
                //,Background_Image
                try
                {

                    con.Open();
                    //MessageBox.show("");
                    reader = cmd.ExecuteReader();
                    string backgroundImage;
                    float rating = 0;

                    string avgRating = "";
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("Background_Image")))
                        {
                            backgroundImage = reader.GetString("Background_Image");
                        }
                        else
                        {
                            backgroundImage = "../Content/img/index/BlankBackground.jpg";
                            //backgroundImage = "~/beardMan.png";
                        }
                        //Get rating fromm database
                        int count = reader.GetInt32("review_count");
                        rating = reader.GetFloat("Rating");
                        if (count == 0)
                        {
                            avgRating = "No ratings";
                        }
                        else
                        {
                            avgRating = "" + rating + "";
                        }

                        StylistList stylist = new StylistList
                        {
                            stylistId = reader.GetInt32("Stylist_ID"),
                            StylistName = reader.GetString("Full_Name"),
                            Rating = avgRating,
                            backgroundImg = backgroundImage

                            //backgroundImg = reader.GetString("Backgound_Image")
                        };
                        stylists.Add(stylist);
                        //idnumber = (int)reader["Stylist_ID"];

                    }
                    con.Close();
                }
                catch (Exception ex)
                {

                }
                //Customer data to view

                ViewBag.paymentStatusDisplay = TempData["paymentStatusDisplay"];
                ViewBag.SuccessCustomerReference = TempData["SuccessCustomerReference"];
                return View(stylists);
        }
            else
            {
                return View("~/Views/Home/Index.cshtml");
    }
   
}
           

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Signout()
        {
            
            Session["uname"] = null;
            Session.Abandon();
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult StylistServices(int stylistKey,float rating,string StylistName)
        {
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
            cmd.CommandText = "SELECT s.Service_ID, s.Service_Name, s.Price, s.Description, st.Background_Image FROM services s INNER JOIN stylist st ON s.stylist_id = st.stylist_id WHERE s.stylist_id = '"+stylistKey+"' AND s.status_ID = '1' ;";
            try
            {

               
                con.Open();
                string Description;
                //MessageBox.show("");
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("Description")))
                    {
                        Description = reader.GetString("Description");
                    }
                    else
                    {
                        Description = "no Description";
                        
                    }
                    if (!reader.IsDBNull(reader.GetOrdinal("Background_Image")))
                    {
                        backgroundImage = reader.GetString("Background_Image");
                    }
                    else
                    {
                        backgroundImage = "../Content/img/index/BlankBackground.jpg";
                        //backgroundImage = "~/beardMan.png";
                    }
                    serviceList services = new serviceList
                    {
                        Price = reader.GetFloat("Price"),
                        Description = Description,
                        ServiceID = reader.GetInt32("Service_ID"),
                        ServiceName = reader.GetString("Service_Name")

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
                stylistName = StylistName,
                stylistID = stylistKey
            };
            return View(servicePage);
        }

    }

    
}