using HR.Data.Models;
using HR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiLoginData api = new ApiLoginData();
            api.GetUser("tsidimas.o", "everton21##!");
        }

    }
}
