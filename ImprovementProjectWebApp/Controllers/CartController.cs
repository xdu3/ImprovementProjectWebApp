using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ImprovementProjectWebApp.Data;
using ImprovementProjectWebApp.Models;
using ImprovementProjectWebApp.Models.WeChatPay;
using ImprovementProjectWebApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QRCoder;
using RestSharp;
using Stripe;

namespace ImprovementProjectWebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public String resptext;

        private RestClient client;
        private RestRequest request;
        private IRestResponse response;

        //private static String key = "XXXyourSignKeyXXX"; //using your SignKey provided by OTTPAY;
        private static string key = "11F71F07C8B73175"; //using your SignKey provided by OTTPAY;
        private static string url = "https://frontapi.ottpay.com/processV2";
        //private static String merchantId = "ON0000XXXX"; //using your Merchant ID provided by OTTPAY;
        private static string merchantId = "AB00004335"; //using your Merchant ID provided by OTTPAY;
        private static string type = "ACTIVEPAY";
        private static string version = "1.0";

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Summary(int? planId)
        {
            if(planId == null)
            {
                return NotFound();
            }

            AppUserPlan userPlan = new AppUserPlan();

            userPlan.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();
            var profile = await _db.CustomerProfile.Where(c => c.ApplicationUserId == applicationUser.Id).FirstOrDefaultAsync();

            if(applicationUser == null)
            {
                return NotFound();
            }
            if(profile == null)
            {
                return NotFound();
            }

            userPlan.Phone = profile.PhoneNumber;
            userPlan.UserName = profile.Name;
            userPlan.ApplicationUserId = applicationUser.Id;

            var plan = await _db.PlanPackage.FindAsync(planId);

            if(plan == null)
            {
                return NotFound();
            }

            userPlan.PlanPackage = plan;
            userPlan.PlanPackageId = plan.Id;



            return View(userPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(string stripeEmail, string stripeToken, AppUserPlan userPlan, string payMethod)
        {
            var plan = await _db.PlanPackage.FindAsync(userPlan.PlanPackageId);

            if (payMethod == "credit")
            {  
                userPlan.OrderTotal = plan.Price;
                userPlan.PaymentType = "Stripe";
                userPlan.PaymentDate = DateTime.Now;

                await _db.AppUserPlans.AddAsync(userPlan);

                await _db.SaveChangesAsync();

                //Stripe Logic
                if (stripeToken != null)
                {
                    var customers = new CustomerService();
                    var charges = new ChargeService();

                    var customer = customers.Create(new CustomerCreateOptions
                    {
                        Email = stripeEmail,
                        SourceToken = stripeToken
                    });

                    var charge = charges.Create(new ChargeCreateOptions
                    {
                        Amount = Convert.ToInt32(userPlan.OrderTotal * 100),
                        Description = "Order ID:" + userPlan.Id,
                        Currency = "cad",
                        CustomerId = customer.Id
                    });

                    userPlan.TransactionId = charge.BalanceTransactionId;
                    if (charge.Status.ToLower() == "succeeded")
                    {

                        //eamil for successful order
                        //await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == claim.Value).FirstOrDefault().Email, "Spice - Order Created " + detailCart.OrderHeader.Id.ToString(), "Order has been submitted successfully!");

                        userPlan.PaymentStatus = SD.PaymentStatusApproved;
                        userPlan.Status = SD.StatusSubmitted;

                        if (userPlan.StartDate.DayOfWeek == DayOfWeek.Monday)
                        {
                            int period = ((TimeSpan)(userPlan.EndDate - userPlan.StartDate)).Days / 7;
                            DateTime TheDate = userPlan.StartDate;
                            for (int i = 0; i < period; i++)
                            {
                                UserCheckInDate userCheckInDate = new UserCheckInDate();
                                userCheckInDate.AppUserPlanId = userPlan.Id;
                                if (i == 0)
                                {
                                    userCheckInDate.CheckInDate = TheDate.AddDays(6);
                                    TheDate = TheDate.AddDays(6);
                                }
                                else
                                {
                                    userCheckInDate.CheckInDate = TheDate.AddDays(7);
                                    TheDate = TheDate.AddDays(7);
                                }
                                await _db.UserCheckInDate.AddAsync(userCheckInDate);

                            }
                        }
                    }
                    else
                    {
                        userPlan.PaymentStatus = SD.PaymentStatusRejected;
                    }

                }
                else
                {
                    userPlan.PaymentStatus = SD.PaymentStatusRejected;
                }

                await _db.SaveChangesAsync();

                return RedirectToAction("Confirm", "Order", new { id = userPlan.Id });
            }
            else if(payMethod == "wechat")
            {
                userPlan.OrderTotal = plan.Price;
                userPlan.PaymentType = "Wechat";
                userPlan.PaymentDate = DateTime.Now;
                userPlan.PaymentStatus = SD.PaymentStatusPending;
                userPlan.Status = SD.StatusSubmitted;
                

                await _db.AppUserPlans.AddAsync(userPlan);

                await _db.SaveChangesAsync(); 

                var planId = userPlan.Id; 

                //return File(bytes, "image/Png");
                return RedirectToAction("WeChatPay", "Cart", new { planId = planId});

            }
            else
            {
                return NotFound();
            }

           
        }

        public async Task<IActionResult> WeChatPay(int planId)
        {
            AppUserPlan userPlan = new AppUserPlan();
            userPlan = await _db.AppUserPlans.Include(a => a.PlanPackage).Include(a => a.ApplicationUser).FirstOrDefaultAsync(a => a.Id == planId);

            var client = new RestClient(url);
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");

            Paydata bo = new Paydata();
            bo.order_id = ("ACT" + (DateTime.UtcNow - Jan1st1970).TotalMilliseconds).Replace(".", "");
            bo.call_back_url = "http://www.ottpay.com";  //using your call back url
            bo.biz_type = "WECHATPAY";
            bo.operator_id = "0000015825";  //using your 10-digital operator number provided by OTTPAY;
            //bo.amount = userPlan.PlanPackage.Price.ToString();
            bo.amount = "1";

            string md5 = signByMD5(bo);
            string data_str = JsonConvert.SerializeObject(bo);

            String encrypted = EncryptAes(data_str, key, md5);
            Console.WriteLine(encrypted);

            Payrequest vo = new Payrequest();
            vo.action = type;
            vo.version = version;
            vo.merchant_id = merchantId;
            vo.data = encrypted;
            vo.md5 = md5;

            string reqstr = JsonConvert.SerializeObject(vo);

            request.AddParameter("undefined", reqstr, ParameterType.RequestBody);
            response = client.Execute(request);

            Payresponse results = JsonConvert.DeserializeObject<Payresponse>(response.Content);
            resptext = DecryptAes(results.data, key, results.md5);


            JObject jObject = JObject.Parse(resptext);
            string codeUrl = (string)jObject.SelectToken("code_url");

            //QR code generate

            QRCodeGenerator generator = new QRCodeGenerator();

            QRCodeData codeData = generator.CreateQrCode(codeUrl, QRCodeGenerator.ECCLevel.M, true);

            QRCoder.QRCode qrcode = new QRCoder.QRCode(codeData);

            Bitmap qrImage = qrcode.GetGraphic(100, Color.Black, Color.White, true);

            MemoryStream ms = new MemoryStream();

            qrImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            byte[] bytes = ms.GetBuffer();

            ms.Close();

            userPlan.TransactionId = (string)jObject.SelectToken("order_id");
            userPlan.QrImage = bytes;
            await _db.SaveChangesAsync();

            //if (userPlan.StartDate.DayOfWeek == DayOfWeek.Monday)
            //{
            //    int period = ((TimeSpan)(userPlan.EndDate - userPlan.StartDate)).Days / 7;
            //    DateTime TheDate = userPlan.StartDate;
            //    for (int i = 0; i < period; i++)
            //    {
            //        UserCheckInDate userCheckInDate = new UserCheckInDate();
            //        userCheckInDate.AppUserPlanId = userPlan.Id;
            //        if (i == 0)
            //        {
            //            userCheckInDate.CheckInDate = TheDate.AddDays(6);
            //            TheDate = TheDate.AddDays(6);
            //        }
            //        else
            //        {
            //            userCheckInDate.CheckInDate = TheDate.AddDays(7);
            //            TheDate = TheDate.AddDays(7);
            //        }
            //        await _db.UserCheckInDate.AddAsync(userCheckInDate);

            //    }
            //}

            return View(userPlan);
        }

        public static String sortStringByMap(Object map)
        {
            Type myType = map.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            IEnumerable<PropertyInfo> sortedEnum = props.OrderBy(f => f.Name);
            IList<PropertyInfo> sortedList = sortedEnum.ToList();

            String sb = "";
            foreach (PropertyInfo prop in sortedList)
            {
                object propValue = prop.GetValue(map, null);
                sb += propValue.ToString();
                // Do something with propValue
            }
            return sb;
        }

        public static String signByMD5(Object paydata)
        {
            String result = sortStringByMap(paydata);
            String sign = GetMd5Hash(result).ToUpper();
            return sign;
        }

        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string EncryptAes(string plainText, string key, string md5)
        {
            byte[] Keybytes;
            byte[] encrypted;
            byte[] IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            string aes_key = GetMd5Hash(md5 + key).Substring(8, 16).ToUpper();

            using (Aes aesAlg = Aes.Create())
            {
                Keybytes = Encoding.UTF8.GetBytes(aes_key);
                aesAlg.Key = Keybytes;

                aesAlg.IV = IV;

                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] input = Encoding.UTF8.GetBytes(plainText);

                encrypted = encryptor.TransformFinalBlock(input, 0, input.Length);

                string rtn_str = Convert.ToBase64String(encrypted);

                // Return the encrypted string from the memory stream. 
                return rtn_str;

            }

        }

        public static string DecryptAes(string decipher_str, string key, string md5)
        {

            byte[] Keybytes;
            byte[] cipherTextCombined;
            byte[] decipher;
            byte[] IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 

            string aes_key = GetMd5Hash(md5 + key).Substring(8, 16).ToUpper();

            using (Aes aesAlg = Aes.Create())
            {
                Keybytes = Encoding.UTF8.GetBytes(aes_key);
                aesAlg.Key = Keybytes;

                cipherTextCombined = Convert.FromBase64String(decipher_str);

                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.ECB;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                decipher = decryptor.TransformFinalBlock(cipherTextCombined, 0, cipherTextCombined.Length);

                plaintext = Encoding.UTF8.GetString(decipher);
            }

            return plaintext;
        }
    }
}