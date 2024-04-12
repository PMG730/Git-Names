using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace API_names
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string[] pokName = new string[3];
            int[] pokAge = new int[3];
            string[] names = new string[3];
            int h = 0;

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Enter name {i + 1}:");
                names[i] = Console.ReadLine();
            }

            foreach (string name in names)
            {
                try
                {
                    string url = $"https://api.agify.io?name={name}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            string json = await response.Content.ReadAsStringAsync();
                            using (JsonDocument doc = JsonDocument.Parse(json))
                            {
                                var root = doc.RootElement;
                                if (root.TryGetProperty("age", out JsonElement ageElement) && ageElement.TryGetInt32(out int age))
                                {
                                    Console.WriteLine($"The predicted age for the name {name} is {age}.");
                                    pokAge[h] = age;
                                    pokName[h] = name;
                                    h = h + 1;
                                }
                                else
                                {
                                    Console.WriteLine($"No age prediction available for the name {name}.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to retrieve data for the name {name}. Status code: {response.StatusCode}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Failed to connect to the server for the name {name}. Error message: {e.Message}");
                }
                catch (JsonException e)
                {
                    Console.WriteLine($"Failed to parse JSON for the name {name}. Error message: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred for the name {name}. Error message: {e.Message}");
                }
            }

            if (pokAge[0] > pokAge[1])
            {
                if (pokAge[0] > pokAge[2])
                {
                    Console.WriteLine($"{pokName[0]} is the oldest");
                }
                else
                {
                    Console.WriteLine($"{pokName[2]} is the oldest");
                }
            }
            else
            {
                if (pokAge[1] > pokAge[2])
                {
                    Console.WriteLine($"{pokName[1]} is the oldest");
                }
                else
                {
                    Console.WriteLine($"{pokName[2]} is the oldest");
                }
            }
        }
    }
}
