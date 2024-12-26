using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configuration
{
    //kendi iç yapımız için oldupundan ayrı tanımladık 
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public List<string> Audience { get; set; }
    }
}
