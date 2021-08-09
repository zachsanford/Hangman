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

        // Get stats
        public static Stats GetStats()
        {
            string _file;
            try
            {
                _file = File.ReadAllText(@"..\..\..\Data\Stats.json");
            }
            catch (Exception)
            {
                _file = File.ReadAllText(@"Data\Stats.json");
            }

            Stats _stats = JsonSerializer.Deserialize<Stats>(_file);
            return _stats;
        }

        // Set stats
        public static void SetStats(int _wins, int _losses)
        {
            Stats _stats = new Stats(_wins, _losses);
            string _path1 = @"Data\Stats.json";
            string _path2 = @"..\..\..\Data\Stats.json";
            string _json = JsonSerializer.Serialize(_stats);

            if (File.Exists(_path1))
            {
                File.WriteAllText(_path1, _json);
            } 
            else
            {
                File.WriteAllText(_path2, _json);
            }            
        }

        public record OfflineWords(string[] Words);
        public record Stats(int wins, int losses);
    }
}
