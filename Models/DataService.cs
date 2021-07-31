using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Hangman.Models
{
    public class DataService
    {
        // Offline Data
        public static string GenerateRandomOfflineWord()
        {
            string _file;
            try
            {
                _file = File.ReadAllText(@"..\..\..\Data\OfflineData.json");
            }
            catch (Exception)
            {
                _file = File.ReadAllText(@"Data\OfflineData.json");
            }
            OfflineWords _words = JsonSerializer.Deserialize<OfflineWords>(_file);
            Random _random = new Random();
            return _words.Words[_random.Next(0, _words.Words.Length)];
        }

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

        public record OfflineWords(string[] Words);
    }
}
