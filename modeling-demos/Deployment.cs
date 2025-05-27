using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;


namespace modeling_demos
{
    class Deployment
    {

        private static string gitdatapath = "https://api.github.com/repos/AzureCosmosDB/CosmicWorks/contents/data/";


        public static async Task LoadData(CosmosClient cosmosDBClient, bool force = false, int? schemaVersion = null)
        {

            //await GetFilesFromRepo("database-v1", force);
            //await GetFilesFromRepo("database-v2", force);
            //await GetFilesFromRepo("database-v3", force);
            //await GetFilesFromRepo("database-v4", force);

            cosmosDBClient.ClientOptions.AllowBulkExecution = true;
            cosmosDBClient.ClientOptions.EnableContentResponseOnWrite = false;

            await LoadContainersFromFolder(cosmosDBClient, "database-v1", "database-v1");
            await LoadContainersFromFolder(cosmosDBClient, "database-v2", "database-v2");
            await LoadContainersFromFolder(cosmosDBClient, "database-v3", "database-v3");
            await LoadContainersFromFolder(cosmosDBClient, "database-v4", "database-v4");

            cosmosDBClient.ClientOptions.AllowBulkExecution = false;
            cosmosDBClient.ClientOptions.EnableContentResponseOnWrite = true;

        }

        private static async Task GetFilesFromRepo(string databaseName, bool force = false)
        {
            string folder = "data" + Path.DirectorySeparatorChar + databaseName;
            string url = gitdatapath + databaseName;
            Console.WriteLine("Geting file info from repo");
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "cosmicworks-samples-cosmosClient");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error reading sample data from GitHub");
                Console.WriteLine($" - {url}");
                return;
            }

            String directoryJson = await response.Content.ReadAsStringAsync(); ;

            GitFileInfo[] dirContents = JsonConvert.DeserializeObject<GitFileInfo[]>(directoryJson);
            var downloadTasks = new List<Task>();

            foreach (GitFileInfo file in dirContents)
            {
                if (file.type == "file")
                {
                    Console.WriteLine($"File {file.name} {file.size}");
                    var filePath = folder + Path.DirectorySeparatorChar + file.name;


                    Boolean downloadFile = true;
                    if (File.Exists(filePath))
                    {
                        if (new System.IO.FileInfo(filePath).Length == file.size)
                        {
                            Console.WriteLine("    File exists and matches size");
                            downloadFile = false;
                            if (force == true) downloadFile = true;
                        }
                    }

                    if (downloadFile)
                    {
                        Console.WriteLine($"   Download path {file.download_url}");
                        Console.WriteLine("    Started download...");
                        downloadTasks.Add(HttpGetFile(file.download_url, filePath));
                    }
                }
            }

            Task downloadTask = Task.WhenAll(downloadTasks);
            try
            {
                downloadTask.Wait();
            }
            catch (AggregateException)
            {

            }

            if (downloadTask.Status == TaskStatus.Faulted)
            {
                Console.WriteLine("Files failed to download");
                foreach (var task in downloadTasks)
                {
                    Console.WriteLine("Task {0}: {1}", task.Id, task.Status);
                    Console.WriteLine(task.Exception.ToString());
                }
            }
            if (downloadTask.Status == TaskStatus.RanToCompletion) Console.WriteLine("Files download sucessfully");
        }

        private static async Task LoadContainersFromFolder(CosmosClient client, string folderName, string databaseName)
        {
            Console.WriteLine("Preparing to load containers");

            Database database = client.GetDatabase(databaseName);

            string folder = "data" + Path.DirectorySeparatorChar + folderName;            
            string[] fileEntries = Directory.GetFiles(folder);
            
            List<Task> concurrentLoads = new List<Task>();

            foreach (string fileName in fileEntries)
            {
                var containerName = fileName.Split(Path.DirectorySeparatorChar)[2];
                Console.WriteLine($"    Container {containerName} from {fileName}");
                try
                {
                    Container container = database.GetContainer(containerName);
                    //concurrentLoads.Add(LoadContainerFromFile(container, fileName));
                    await LoadContainerFromFile(container, fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to container {containerName} ");
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static async Task LoadContainerFromFile(Container container, string file)
        {
            using (StreamReader streamReader = new StreamReader(file))
            {
                string itemsJson = streamReader.ReadToEnd();
                dynamic itemsArray = JsonConvert.DeserializeObject(itemsJson);
                List<Task> tasks = new List<Task>();
                foreach (var item in itemsArray)
                {
                    tasks.Add(CreateItemWithRetryAsync(container, item));
                }
                await Task.WhenAll(tasks);
            }
        }

        private static async Task CreateItemWithRetryAsync(Container container, dynamic record)
        {
            bool retry = true;
            while (retry)
            {
                try
                {
                    await container.CreateItemAsync(record);
                    break;
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    int waitTime = ex.RetryAfter.HasValue ? ex.RetryAfter.Value.Milliseconds : 1000;
                    Console.WriteLine($"Rate limited. Waiting for {waitTime}ms before retrying...");
                    await Task.Delay(waitTime);
                }
            }
        }

        private static async Task HttpGetFile(string url, string filename)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
                {
                    using (Stream streamToWriteTo = File.Open(filename, FileMode.Create))
                    {
                        await streamToReadFrom.CopyToAsync(streamToWriteTo);
                    }
                }
            }
        }

        class GitFileInfo
        {
            public String name="";
            public String type="";
            public long size=0;
            public String download_url="";
        }

    }
}
