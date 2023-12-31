﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Communication.Emails
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);

        Task<List<EmailMessage>> ReceiveEmailAsync(int maxCount = 10);
    }
}
