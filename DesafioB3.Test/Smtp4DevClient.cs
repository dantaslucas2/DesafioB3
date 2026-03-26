using System;
using System.Collections.Generic;
using System.Text;

namespace DesafioB3.Test
{
    internal class Smtp4DevClient
    {
        private HttpClient _httpClient;

        public Smtp4DevClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
