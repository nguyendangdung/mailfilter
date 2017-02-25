using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Filters;

namespace Services.Filters
{
    public class SensitiveInfoFilter : IFilter
    {
        /// <summary>
        /// Nếu nội dung email chứa chuỗi số 8 ký tự trở lên: Vi phạm lỗi Thông tin nhạy cảm
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<FilterResult> CheckMailAsync(EmailContent email)
        {
            throw new NotImplementedException();
        }

        public FilterResult CheckMail(EmailContent email)
        {
            var words = email.Content.Split(new char[0]).Cast<string>();
            var illegalWords = words.Where(s => s.Length >= 8 && s.All(Char.IsDigit));
            if (illegalWords.Any())
            {
                // email.Status = EmailStatus.Violated;
                return new FilterResult()
                {
                    Status = EmailStatus.Violated,
                    Message = "Thông tin nhạy cảm"
                };
            }
            // email.Status = EmailStatus.NotViolated;
            return new FilterResult() {Status = EmailStatus.NotViolated};
        }
    }
}
