using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Test.Domain
{
    public class ImapSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
