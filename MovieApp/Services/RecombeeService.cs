using Recombee.ApiClient;
using Recombee.ApiClient.ApiRequests;
using System;
using System.Collections.Generic;
using System.IO;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace MovieApp.Services
{
    public class RecombeeService
    {
        private readonly RecombeeClient client;

        public RecombeeService(string database, string token)
        {

            client = new RecombeeClient(database, token);
        }

        // Metodă pentru configurarea bazei Recombee
        // public void ConfigureDatabase()
        // {
        //     try
        //     {
        //         // Adaugă proprietăți pentru item
        //         client.Send(new AddItemProperty("Title", "string"));
        //         client.Send(new AddItemProperty("Genre", "string"));
        //         client.Send(new AddItemProperty("Premiere", "string"));
        //         client.Send(new AddItemProperty("Runtime", "int"));
        //         client.Send(new AddItemProperty("IMDB-Score", "double"));
        //         client.Send(new AddItemProperty("Language", "string"));
        //         Console.WriteLine("Baza de date a fost configurată cu succes!");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Eroare la configurarea bazei: {ex.Message}");
        //     }
        // }

        // Metodă pentru încărcarea datelor dintr-un fișier CSV
        // public void LoadDataFromCsv(string filePath, int maxLines = 585)
        // {
        //     try
        //     {
        //         var requests = new List<Request>();
        //         int lineCount = 0;
        //         int itemId = 0;

        //         using (var reader = new StreamReader(filePath))
        //         {
        //             var headerLine = reader.ReadLine();
        //             if (headerLine == null)
        //                 throw new Exception("Fișierul CSV este gol!");

        //             var headers = headerLine.Split(',');

        //             Console.WriteLine("Header extras din fișierul CSV:");
        //             Console.WriteLine(string.Join(", ", headers));

        //             while (!reader.EndOfStream)
        //             {
        //                 var line = reader.ReadLine();
        //                 var values = line?.Split(',');

        //                 if (values == null) continue;

        //                 // Selectăm primele 4 coloane (sau câte sunt disponibile)
        //                 var selectedColumns = new Dictionary<string, object>
        //                 {
        //                     { headers[0], values[0] }, // Title
        //                     { headers[1], values[1] }, // Genre
        //                     { headers[2], values[2] }, // Premiere
        //                     { headers[3], values[3] },  // Runtime
        //                     { headers[4], values[4] },
        //                     { headers[5], values[5] }
        //                 };

        //                 // Adaugă cererea pentru setarea valorilor item-ului
        //                 var request = new SetItemValues(itemId.ToString(), selectedColumns, cascadeCreate: true);
        //                 requests.Add(request);

        //                 itemId++;
        //                 lineCount++;

        //                 if (lineCount >= maxLines)
        //                     break;
        //             }
        //         }

        //         // Trimite cererile în Batch
        //         var response = client.Send(new Batch(requests));
        //         Console.WriteLine($"S-au încărcat cu succes {lineCount} elemente din CSV.");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Eroare la încărcarea datelor din CSV: {ex.Message}");
        //     }
        // }



        // Metodă pentru a testa conexiunea (opțional)

        public void LoadDataFromCsv(string filePath, int maxLines = 585)
        {
            try
            {
                var requests = new List<Request>();
                int lineCount = 0;
                int itemId = 0;

                // Configurația pentru CSV (ignora diferențele majuscule/minuscule în header)
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    MissingFieldFound = null, // Ignorăm câmpurile lipsă
                    BadDataFound = context => Console.WriteLine($"Linie invalidă: {context.RawRecord}")
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    // Citim fișierul CSV
                    var records = csv.GetRecords<dynamic>().Take(maxLines);

                    // Maparea dintre coloanele CSV și proprietățile Recombee
                    var propertyMapping = new Dictionary<string, string>
            {
                { "Title", "Title" },
                { "Genre", "Genre" },
                { "Premiere", "Premiere" },
                { "Runtime", "Runtime" },
                { "IMDB Score", "IMDB-Score" },
                { "Language", "Language" }
            };

                    foreach (var record in records)
                    {
                        var recordDict = (IDictionary<string, object>)record;

                        var selectedColumns = new Dictionary<string, object>();

                        foreach (var column in propertyMapping)
                        {
                            if (recordDict.TryGetValue(column.Key, out var value))
                            {
                                selectedColumns[column.Value] = value;
                            }
                        }

                        // Console.WriteLine("Selected columns pentru item:");
                        // foreach (var kvp in selectedColumns)
                        // {
                        //     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                        // }

                        // Adaugă cererea pentru setarea valorilor item-ului
                        var request = new SetItemValues(itemId.ToString(), selectedColumns, cascadeCreate: true);
                        requests.Add(request);

                        itemId++;
                        lineCount++;
                    }
                }

                // Trimite cererile în Batch
                Console.WriteLine($"Număr total de cereri: {requests.Count}");
                var response = client.Send(new Batch(requests));
                Console.WriteLine($"Răspuns de la Recombee: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la încărcarea datelor din CSV: {ex.Message}");
            }
        }
    }
}
