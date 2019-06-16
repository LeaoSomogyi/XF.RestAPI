using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XF.RestAPI.Model
{
    public class Professor
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        private string name;

        [JsonProperty("Nome")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                App.ProfessorVM.OnSave.IsModelValid();
            }
        }

        private string title;

        [JsonProperty("Titulo")]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                App.ProfessorVM.OnSave.IsModelValid();
            }
        }

        public Professor() { }
    }

    public static class ProfessorRepository
    {
        #region "  Properties  "

        private static readonly string baseURL = "http://apiaplicativofiap.azurewebsites.net/";

        private static HttpClient httpClient;

        #endregion

        #region "  Private Methods  "

        private static HttpClient CreateHttpClient(bool addHeaders = false, string baseUrl = "")
        {
            httpClient = new HttpClient();

            if (addHeaders)
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            if (!string.IsNullOrEmpty(baseUrl))
            {
                httpClient.BaseAddress = new Uri(baseUrl);
            }

            return httpClient;
        }

        #endregion

        #region "  API Call Methods  "

        public static async Task<List<Professor>> GetTeachersAsync()
        {
            httpClient = CreateHttpClient();

            Stream stream = await httpClient.GetStreamAsync($"{baseURL}api/professors");

            string textResponse = new StreamReader(stream).ReadToEnd();

            List<Professor> jsonResponse = JsonConvert.DeserializeObject<List<Professor>>(textResponse);

            return jsonResponse;
        }

        public static async Task<bool> PostProfessorAsync(Professor professor)
        {
            if (professor == null)
            {
                return false;
            }

            httpClient = CreateHttpClient(true, baseURL);

            string payload = JsonConvert.SerializeObject(professor);

            HttpResponseMessage response = await httpClient.PostAsync("api/professors",
                new StringContent(payload, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public static async Task<bool> PutProfessorAsync(Professor professor)
        {
            if (professor.Id == 0)
            {
                return false;
            }

            httpClient = CreateHttpClient();

            string payload = JsonConvert.SerializeObject(professor);

            HttpResponseMessage response = await httpClient.PutAsync($"{baseURL}/api/professors/{professor.Id}",
                new StringContent(payload, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public static async Task<bool> DeleteProfessorAsync(int id)
        {
            httpClient = CreateHttpClient(true, baseURL);

            HttpResponseMessage response = await httpClient.DeleteAsync($"api/professors/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        
        #endregion
    }
}
