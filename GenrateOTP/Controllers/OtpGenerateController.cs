using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GenrateOTP.Controllers
{
    public class OtpGenerateController : Controller
    {
        private object userrating;

        // GET: OtpGenerate
        public ActionResult Index()
        {
            return View();
        }


        //Post method
        [HttpPost]

        public ActionResult RatingandReview(UserRatingAndReviewsVM rating)
        {
            Random r = new Random();
            string OTP = r.Next(1000, 9999).ToString();
            SendMessage(rating.Phone, OTP);
            Session["OTP"] = OTP;
            TempData["allData"] = rating;        
            return RedirectToAction("Index", "Rating");
        }

        private void SendMessage(object phone, string oTP)
        {
            throw new NotImplementedException();
        }
         

        //This function is use to Send Otp on user phone
        public void SendMessage(string mobileNumber, string message)
        {
            string HostURI = "http://182.18.138.53/api.php?username=traviyotxn&password=121246&sender=TRVIYO&sendto=" + mobileNumber + "&message=" + message + "";
            StringBuilder uriBulider = new StringBuilder();
            uriBulider.AppendFormat(HostURI, mobileNumber, message);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uriBulider.ToString());
            request.Method = "GET";
            request.GetResponseAsync();
        }


        [HttpPost]
        public ActionResult ReadOtp(ReadOtp otp, UserRatingAndReviewsVM rating)
        {

            if (Session["OTP"].ToString() == otp.CurrentOtp)
            {
                try
                {

                    long ClientId = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["ClientId"]);
                    rating.ClientId = ClientId;
                    if (ModelState.IsValid)
                    {
                        var result = userrating.SaveUserRatings(rating);
                        if (result != null)
                        {
                            ModelState.Clear();
                        }
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                Session["OTP"] = null;
            }

            return View();
        }


    }

    public class UserRatingAndReviewsVM
    {
        public object Phone { get; internal set; }
        public long ClientId { get; internal set; }
    }
}