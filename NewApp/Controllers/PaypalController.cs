using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using NewApp.Clients;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Configuration = NewApp.Clients.Configuration;
using MySql.Data.MySqlClient;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Web.Helpers;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace NewApp.Controllers
{
    public class PaypalController : Controller
    {
        // GET: Paypal
        public static int orderId;
        public static string customerReferenceId;
        public static string paymentKey;
        public static int StylistID;
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        
        public ActionResult PaymentWithCreditCard()
        {
            
            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            Item item = new Item();
            item.name = "Demo Item";
            item.currency = "USD";
            item.price = "5";
            item.quantity = "1";
            item.sku = "sku";

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;

            //Address for the payment
            PayPal.Api.Address billingAddress = new PayPal.Api.Address();
            billingAddress.city = "NewYork";
            billingAddress.country_code = "US";
            billingAddress.line1 = "23rd street kew gardens";
            billingAddress.postal_code = "43210";
            billingAddress.state = "NY";


            //Now Create an object of credit card and add above details to it
            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = "058";
            crdtCard.expire_month = 3;
            crdtCard.expire_year = 2027;
            crdtCard.first_name = "Vidysagar";
            crdtCard.last_name = "Dhasaratha";
            crdtCard.number = "4032035065962953";
            crdtCard.type = "Visa";

            // Specify details of your payment amount.
            Details details = new Details();
            details.shipping = "1";
            details.subtotal = "5";
            details.tax = "1";

            // Specify your total payment amount and assign the details object
            PayPal.Api.Amount amnt = new PayPal.Api.Amount();
            amnt.currency = "USD";
            // Total = shipping tax + subtotal.
            amnt.total = "7";
            amnt.details = details;

            // Now make a trasaction object and assign the Amount object
            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "Description about the payment amount.";
            tran.item_list = itemList;
            tran.invoice_number = "your invoice number which you are generating";

            // Now, we have to make a list of trasaction and add the trasactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            PayPal.Api.Payer payr = new PayPal.Api.Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";

            PayPal.Api.PayerInfo payer_info = new PayPal.Api.PayerInfo();

            payer_info.email = "vidyasagar.dhasaratha@gmail.com";
            

            // finally create the payment object and assign the payer object & transaction list to it
            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            try
            {
                //getting context from the paypal, basically we are sending the clientID and clientSecret key in this function 
                //to the get the context from the paypal API to make the payment for which we have created the object above.

                //Code for the configuration class is provided next

                // Basically, apiContext has a accesstoken which is sent by the paypal to authenticate the payment to facilitator account. An access token could be an alphanumeric string

                APIContext apiContext = Configuration.GetAPIContext();

                // Create is a Payment class function which actually sends the payment details to the paypal API for the payment. The function is passed with the ApiContext which we received above.

                Payment createdPayment = pymnt.Create(apiContext);

                //if the createdPayment.State is "approved" it means the payment was successfull else not

                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
            }
            catch (PayPal.PayPalException ex)
            {
                //Logger.Log("Error: " + ex.Message);
                return View("FailureView");
            }

            return View("SuccessView");
            
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult PaymentWithPaypal(string totalPrice,string serviceId="", string stylistId = "",string paymentkey="")
        {

            //add order to database
            //string TotalPrice = "";
            if(paymentkey == "stylist")
            {
                Random rand = new Random();
                orderId = rand.Next(50000, 100001);
                serviceId = "0";
                paymentKey = "stylist";
                StylistID = int.Parse(stylistId);
            }
            else if (serviceId != ""&& stylistId != "")
            {
               
                string conString = null;
                MySqlConnection con = null;
                int customerId = (int)Session["customerID"];

                conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                con = new MySqlConnection(conString);
                string insertQuery = "INSERT INTO `roamingrazorsdb`.`order` ( `Customer_ID`, `Stylist_ID`, `Amount`, `serviceIdList`) VALUES (@customerId, @stylistId, @totalPrice, @serviceId); Select LAST_INSERT_ID();";
                //Insert logic
                MySqlCommand command = new MySqlCommand(insertQuery, con);

                command.Parameters.AddWithValue("@customerId", customerId);
                command.Parameters.AddWithValue("@stylistId", int.Parse(stylistId));
                command.Parameters.AddWithValue("@totalPrice", float.Parse(totalPrice));
                command.Parameters.AddWithValue("@serviceId", serviceId);
                con.Open();
                orderId = Convert.ToInt32(command.ExecuteScalar());
                con.Close();
            }

             
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist

                    //it is returned by the create function call of the payment class

                    // Creating a payment

                    // baseURL is the url on which paypal sendsback the data.

                    // So we have provided URL of this controller only

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPaypal?";

                    //guid we are generating for storing the paymentID received in session

                    //after calling the create function and it is used in the payment execution
                    //send it to customer for payment reference
                    string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    // Define the length of the random string
                    int length = 10;
                    // Create a StringBuilder to construct the random string
                    StringBuilder builder = new StringBuilder();

                    // Create a Random object
                    Random random = new Random();
                    // Loop to append random characters to the StringBuilder
                    for (int i = 0; i < length; i++)
                    {
                        // Get a random index within the range of the characters
                        int index = random.Next(chars.Length);

                        // Append the character at the random index to the StringBuilder
                        builder.Append(chars[index]);
                    }

                    // Convert the StringBuilder to a string
                    string guid = builder.ToString();
                    //var guid = Convert.ToString((new Random()).Next(100000));
                    customerReferenceId = guid;
                    //var guid = Convert.ToString(orderId);

                    //CreatePayment function gives us the payment approval url

                    //on which payer is redirected for paypal acccount payment

                    //var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid,"4","56");
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, serviceId, totalPrice,Convert.ToString(orderId));

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];
                    int transactionSuccess = 1;
                    string PaypalTransactionID = Session[guid] as string;
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        transactionSuccess = 0;
                        
                        if(paymentKey == "stylist")
                        {
                            TempData["paymentStatusStylistDisplay"] = "block !important";
                            TempData["SuccessStylistReference"] = "Payment Unsuccessful. Reference ID: " + customerReferenceId;
                            return RedirectToAction("StylistSubscription", "Stylist");
                        }
                        TempData["paymentStatusDisplay"] = "block !important";
                        TempData["SuccessCustomerReference"] = "Payment Unsuccessful. Reference ID: " + customerReferenceId;
                        return RedirectToAction("Index", "LoginCustomer");
                    }
                    //add transaction to table
                    if(paymentKey == "stylist") {
                        string conString = null;
                        MySqlConnection con = null;

                        conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                        con = new MySqlConnection(conString);
                        string insertQuery2 = "INSERT INTO `roamingrazorsdb`.`stylist_transaction` (  `Payment_Type_ID`, `Transaction_Successful`, `Transaction_Amount`, `Date_Time`, `Customer_Reference_ID`, `Paypal_Transaction_ID`,`Stylist_ID`) VALUES ('1', '" + transactionSuccess.ToString() + "','11.19',Now(),'" + customerReferenceId + "', '" + PaypalTransactionID + "','"+StylistID + "')";
                        //Insert logic
                        //update stylist subscription due date
                        string insertQuery3 = "UPDATE stylist SET Subscription_Due_Date = DATE_ADD(NOW(), INTERVAL 30 DAY) WHERE Stylist_ID = '"+StylistID+"';";
                        MySqlCommand command = new MySqlCommand(insertQuery2, con);
                        MySqlDataReader reader;
                        MySqlCommand command2 = new MySqlCommand(insertQuery3, con);
                        MySqlDataReader reader2;
                        con.Open();
                        reader = command.ExecuteReader();
                        reader2 = command2.ExecuteReader();
                        con.Close();
                    }
                    else {
                        string conString = null;
                        MySqlConnection con = null;

                        conString = System.Configuration.ConfigurationManager.AppSettings["ConString"];
                        con = new MySqlConnection(conString);
                        string insertQuery2 = "INSERT INTO `roamingrazorsdb`.`transaction` ( `Order_ID`, `Payment_Type_ID`, `Transaction_Successful`, `Stylist_Paid`, `Transaction_Amount`, `Stylist_Amount`, `Date_Time`, `Customer_Reference_ID`, `Paypal_Transaction_ID`) VALUES ( '" + orderId.ToString() + "', '1', '" + transactionSuccess.ToString() + "', '0', (select Amount from roamingrazorsdb.order where order_id = '" + orderId.ToString() + "'), (SELECT (1-Mark_Up_Value) * (select Amount from roamingrazorsdb.order where order_id = '" + orderId.ToString() + "') FROM markup),Now(),'" + customerReferenceId + "', '" + PaypalTransactionID + "')";
                        //Insert logic
                        MySqlCommand command = new MySqlCommand(insertQuery2, con);
                        MySqlDataReader reader;
                        con.Open();
                        reader = command.ExecuteReader();
                        con.Close();
                    }
                    

                }
            }
            catch (Exception ex)
            {
                //Logger.Log("Error" + ex.Message);
                return View("~/Views/Home/Contact.cshtml");
            }
            // add order id to database a
            //success view
           
            if(paymentKey == "stylist")
            {
                return RedirectToAction("StylistPage", "Stylist");
            }
            else
            {
                TempData["paymentStatusDisplay"] = "block !important";
                TempData["SuccessCustomerReference"] = "Payment Successful. Reference ID: " + customerReferenceId;
                return RedirectToAction("Index", "LoginCustomer");
            }
            
        }
        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string serviceId, string totalPrice,string orderId)
        //private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            float TotalPrice = float.Parse(totalPrice);

            double subTotal = Math.Round(TotalPrice / 1.12, 2);
            double tax = subTotal * 0.12;
            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = "service"+serviceId,
                currency = "CAD",
                price = subTotal.ToString(),
                quantity = "1",
                sku = "sku"
            });

            var payer = new PayPal.Api.Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                //tax = tax.ToString(),
                tax = tax.ToString(),
                shipping = "0",
                //subtotal = subTotal.ToString()
                subtotal = subTotal.ToString()
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new PayPal.Api.Amount()
            {
                currency = "CAD",
                total = totalPrice, // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "invoice"+orderId,
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }
    }
}