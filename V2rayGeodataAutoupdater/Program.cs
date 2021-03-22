using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using V2rayGeodataAutoupdater.Data;

using static System.Console;

namespace V2rayGeodataAutoupdater
{
    class Program
    {
        private static Config config = new();
        private static HttpClient httpClient = new();
        private static readonly string defaultConfigFile = AppDomain.CurrentDomain.BaseDirectory + Constants.CONFIG_FILE;

        static void Main(string[] args)
        {
            Initialize();
            ReadConfig(args.Length > 0 ? args[0] : defaultConfigFile);
            UpdateData().Wait();
            LanchClient();

            WriteLine("Application End");
        }

        private static void Initialize()
        {
            httpClient.DefaultRequestVersion = HttpVersion.Version20;
        }

        private static void ReadConfig(string configFile)
        {
            if (File.Exists(configFile))
            {
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configFile));
            }
            else
            {
                if (configFile == defaultConfigFile)
                {
                    WriteLine("No config File");
                }
                else
                {
                    // if given config file not exist, then try read default config file
                    WriteLine("Given config File not exist, try read default");
                    ReadConfig(defaultConfigFile);
                }
            }
        }

        /// <summary>
        /// An asynchronous enter
        /// </summary>
        /// <returns></returns>
        private static async Task UpdateData()
        {
            try
            {
                await WarmupHandler();
                await UpdateDataHandle();
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Using head request to warm-up HTTP client
        /// </summary>
        /// <returns></returns>
        private async static Task WarmupHandler()
        {
            Dictionary<string, bool> queryList = new();

            if (config.IsUpdateGeoSite)
            {
                if (!string.IsNullOrEmpty(config.GeoSiteSource))
                {
                    queryList.Add(config.GeoSiteSource, false);
                }

                if (!string.IsNullOrEmpty(config.GeoSiteSourceReserve))
                {
                    queryList.Add(config.GeoSiteSourceReserve, false);
                }
            }

            if (config.IsUpdateGeoIp)
            {
                if (!string.IsNullOrEmpty(config.GeoIpSource))
                {
                    queryList.Add(config.GeoIpSource, false);
                }

                if (!string.IsNullOrEmpty(config.GeoIpSourceReserve))
                {
                    queryList.Add(config.GeoIpSourceReserve, false);
                }
            }

            if (queryList.Count == 0)
            {
                return;
            }

            var warmups = from url in queryList.Keys
                          select SendHead(url);

            await Task.WhenAny(warmups).ContinueWith(result =>
            {
                queryList[result.Result.Result.Key] = result.Result.Result.Value;
                WriteLine($"{result.Result.Result.Key} | {result.Result.Result.Value}");
            });

            WriteLine("Warm-up end");
        }

        /// <summary>
        /// Update Data
        /// </summary>
        /// <returns></returns>
        private async static Task UpdateDataHandle()
        {
            List<string> GeoUpdateUrls = new();
            List<string> IPUpdateUrls = new();

            if (config.IsUpdateGeoSite)
            {
                if (!string.IsNullOrEmpty(config.GeoSiteSource))
                {
                    GeoUpdateUrls.Add(config.GeoSiteSource);
                }

                if (!string.IsNullOrEmpty(config.GeoSiteSourceReserve))
                {
                    GeoUpdateUrls.Add(config.GeoSiteSourceReserve);
                }
            }

            if (config.IsUpdateGeoIp)
            {
                if (!string.IsNullOrEmpty(config.GeoIpSource))
                {
                    IPUpdateUrls.Add(config.GeoIpSource);
                }

                if (!string.IsNullOrEmpty(config.GeoIpSourceReserve))
                {
                    IPUpdateUrls.Add(config.GeoIpSourceReserve);
                }
            }

            var savePath = (config.IsDataSaveElsewhere) ? config.DataSavePath : Path.GetDirectoryName(config.ClientPath);

            if (GeoUpdateUrls.Count > 0)
            {
                var tasks = from url in GeoUpdateUrls
                            select httpClient.GetAsync(url);

                using var fs = new FileStream($"{savePath}\\geosite.dat", FileMode.Create);

                await Task.WhenAny(tasks).Result.Result.Content.CopyToAsync(fs);
            }

            if (IPUpdateUrls.Count > 0)
            {
                var tasks = from url in IPUpdateUrls
                            select httpClient.GetAsync(url);

                using var fs = new FileStream($"{savePath}\\geoip.dat", FileMode.Create);

                await Task.WhenAny(tasks).Result.Result.Content.CopyToAsync(fs);
            }

            WriteLine("Update Data end");
        }

        /// <summary>
        /// Launch Client
        /// </summary>
        private static void LanchClient()
        {
            WriteLine("Launch Client...");

            if (config.IsLanchClient && File.Exists(config.ClientPath))
            {
                var client = Process.Start(config.ClientPath, config.ClientParameter);
                WriteLine("Client launched");
                // If show console then show the result to user
                if (config.IsShowConsole)
                {
                    client.WaitForExit();
                }
            }
        }

        private static async Task<KeyValuePair<string, bool>> SendHead(string url)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Head,
                RequestUri = new Uri(url),
            };

            var response = await httpClient.SendAsync(request);

            return new KeyValuePair<string, bool>(url, response.IsSuccessStatusCode);
        }

    }
}
