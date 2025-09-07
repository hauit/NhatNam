using NUnit.Framework;
using SendEmailDll;
using System.Collections.Generic;
using System;
using ProcessingWork.Base;
using System.Data;
using System.Data.SqlClient;

namespace Tests
{
    public class Tests
    {
        private AEmail Test;
        [SetUp]
        public void Setup()
        {
            Test = new EmailErrorItem();
        }

        [Test]
        public void Test1()
        {
            Test.CreateEmailContent();
            Test.SendEmail();
        }

        [Test]
        public void CreateContentByObj()
        {
            EmailObject obj = new EmailObject();
            obj.AttachedFiles = new List<string>();
            obj.BCC = new List<string>();
            obj.CC = new List<string>();
            obj.Body = string.Empty;
            obj.Subject = "Email created by test";
            obj.To = new List<string>() { "nguyenvuhau0511@gmail.com"};
            obj.Pass = string.Empty;
            obj.SMTP = string.Empty;
            obj.Sender = string.Empty;

            Test.CreateEmailContent(obj);
            Test.SendEmail();
        }

        [Test]
        [TestCase("","",ExpectedResult="07/08/2072")]
        [TestCase("czxc","Y459242",ExpectedResult="07/08/2022")]
        public DateTime TestGetTHPhoiByTrackingHistory(string partid, string mono)
        {
            clsBase ba = new clsBase();
            return ba.GetTHPhoi(partid, mono);
        }
    }
}