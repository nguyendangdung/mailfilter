﻿using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Filters;

namespace Services.Filters
{
    /// <summary>
    /// -	Nếu nội dung email chứa chuỗi có format KH_xxxxxx, trong đó xxxxxx là chuỗi 6 ký tự bất kỳ trở lên
    ///  (VD: “Ngày hôm nay tôi có gặp khách hàng  KH_00xy01z vào buổi chiều”): Vi phạm lỗi Gửi mã khách hàng
    /// </summary>
    public class SendCustomerIDFilter : IFilter
    {
        public Task<bool> CheckMailAsync(EmailContent email)
        {
            throw new System.NotImplementedException();
        }

        public bool CheckMail(EmailContent email)
        {
            var words = email.Content.Split(new char[0]).Cast<string>();

            var illegalWords =
                words.Where(s => s.Length == 9 && s.ToLower().StartsWith("kh_") && s.Substring(3, 6).All(char.IsDigit));
            if (illegalWords.Any())
            {
                email.Status = EmailStatus.Violated;
                return false;
            }
            email.Status = EmailStatus.NotViolated;
            return true;
            // throw new System.NotImplementedException();
        }
    }
}