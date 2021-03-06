﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Filters
{
    public interface IFilter
    {
        Task<FilterResult> CheckMailAsync(EmailContent email);
        FilterResult CheckMail(EmailContent email);
    }
}
