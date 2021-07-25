using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hangman.Models
{
    public class DataService
    {
        // Offline Data

        // Online Data
        public static string GenerateRandomOnlineWord()
        {
            var _client = new HttpClient();
            var _request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(@"https://random-word-api.herokuapp.com/word?number=1")
            };

            using (var _response = _client.SendAsync(_request).Result)
            {
                _response.EnsureSuccessStatusCode();
                string _body = _response.Content.ReadAsStringAsync().Result;
                string[] _splitBody = _body.Split('"');

                if (_body != null)
                {
                    return _splitBody[1];
                }
                else
                {
                    return "Did not work";
                }
            }            
        }
    }
}
