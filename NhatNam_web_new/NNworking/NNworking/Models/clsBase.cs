using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NNworking.Models
{
    public class clsBase
    {
        public int MyProperty { get; set; }
        public clsBase()
        {

        }

        public void ReCreateSession()
        {
            if (HttpContext.Current.Session.Count == 0)
            {
                if (HttpContext.Current.Request.Cookies["Session"] == null)
                {
                    HttpContext.Current.Response.Redirect(clsSession.Login);
                }
                else
                {
                    HttpContext.Current.Session["SecID"] = HttpContext.Current.Request.Cookies["Session"]["SecID"];
                    HttpContext.Current.Session["StaffID"] = HttpContext.Current.Request.Cookies["Session"]["StaffID"];
                    HttpContext.Current.Session["Department"] = HttpContext.Current.Request.Cookies["Session"]["Department"];
                    HttpContext.Current.Session["Group"] = HttpContext.Current.Request.Cookies["Session"]["Group"];
                    HttpContext.Current.Session["GroupID"] = HttpContext.Current.Request.Cookies["Session"]["GroupID"];
                    HttpContext.Current.Session["Level"] = HttpContext.Current.Request.Cookies["Session"]["Level"];
                    HttpContext.Current.Session["user"] = HttpContext.Current.Request.Cookies["Session"]["user"];
                    //HttpContext.Current.Session["pass"] = HttpContext.Current.Request.Cookies["Session"]["pass"];
                    HttpContext.Current.Session["BP"] = HttpContext.Current.Request.Cookies["Session"]["BP"];
                    HttpContext.Current.Session.Timeout = 1000;
                }
            }
        }

        public string EncryptPassword(string strEnCrypt, string key)
        {
            try
            {
                byte[] keyArr;
                byte[] EnCryptArr = Encoding.UTF8.GetBytes(strEnCrypt);
                MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider();
                keyArr = MD5Hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider();
                tripDes.Key = keyArr;
                tripDes.Mode = CipherMode.ECB;
                tripDes.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripDes.CreateEncryptor();
                byte[] arrResult = transform.TransformFinalBlock(EnCryptArr, 0, EnCryptArr.Length);
                return Convert.ToBase64String(arrResult, 0, arrResult.Length);
            }
            catch (Exception ex) { }
            return "";
        }

        public void CheckPermission(string Session, string Department)
        {
            if (Session != Department)
            {
                HttpContext.Current.Response.Redirect(clsSession.url + "/Index");
            }
        }

        private byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }

        private string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        public string md5_Encrypt(string toEncrypt)
        {
            string s = md5("ALO");
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(s));
            TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
            {
                Key = buffer,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] inArray = provider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }
        //Hàm giải mã
        public string md5_Decrypt(string toDecrypt)
        {
            string s = md5("ALO");
            byte[] inputBuffer = Convert.FromBase64String(toDecrypt);
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(s));
            TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
            {
                Key = buffer,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] bytes = provider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
            return Encoding.UTF8.GetString(bytes);
        }

        public void CheckAndUpdateBusOrder(C242_InventoryReceivedDetail model, NN_DatabaseEntities _context)
        {
            try
            {

                var dept = _context.View_242_InventoryReceived.Where(x => x.ID == model.VoucherID).FirstOrDefault();
                if (dept == null)
                {
                    return;
                }

                var data = _context.C242_BusOder.Where(x => x.BOderNo.ToLower() == model.OrderNumber).FirstOrDefault();
                if (data == null)
                {
                    return;
                }

                if (dept.ReceiveDept.ToLower() != "cua" && dept.ImportFrom.ToLower() == "qm")
                {
                    data.Finished = false;
                }

                if (dept.ReceiveDept.ToLower() == "qm" && dept.ImportFrom.ToLower() == "cua")
                {
                    var slq = _context.View_242_BusOder.Where(x => x.ID == data.ID).FirstOrDefault();
                    if (slq == null)
                    {
                        return;
                    }

                    if (slq.CONLAI <= model.Qty)
                    {
                        data.Finished = true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new ArgumentException("checkanupdate bus " + ex.Message);
            }
        }
    }

    public class clsDisplayFileURL
    {
        public string DisplayMember { get; set; }
        public string ValueMember { get; set; }
    }
}