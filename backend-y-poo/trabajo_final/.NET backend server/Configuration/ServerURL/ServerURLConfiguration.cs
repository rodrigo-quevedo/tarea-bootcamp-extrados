using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.ServerURL
{
    public class ServerURLConfiguration : IServerURLConfiguration
    {
        private string URL {  get; set; }
        public ServerURLConfiguration(string url) {
            this.URL = url;
        }

        public string GetServerURL()
        {
            return this.URL;
        }
    }
}
