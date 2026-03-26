using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Models
{
    public class EmailSettings
    {
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public string RecipientEmail { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
