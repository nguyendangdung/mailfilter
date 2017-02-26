using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Services.Filters.Tests
{
    [TestClass()]
    public class SendCustomerIDFilterTests
    {
        [TestMethod()]
        public void CheckMailTest()
        {
            var email = new EmailContent()
            {
                Content = "kh_11111111"
            };
            var filter = new SendCustomerIDFilter();

            var result = filter.CheckMail(email);

            Assert.AreEqual(result.Status, EmailStatus.NotViolated);


            email = new EmailContent() { Content = "aksjfh akjfhkasjdfh                aksjdfh kfajsf            kajshdfaf 11111     1111" };
            result = filter.CheckMail(email);
            Assert.IsTrue(result.Status == EmailStatus.NotViolated);


            email = new EmailContent() { Content = "aksjfh akjfhkasjdfh       kh_123456         aksjdfh kfajsf            kajshdfaf 11111     1111" };
            result = filter.CheckMail(email);
            Assert.IsTrue(result.Status == EmailStatus.Violated);
        }
    }
}