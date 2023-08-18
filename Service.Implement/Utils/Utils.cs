using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Service.Implement.Utilities
{
    public static class Utils
    {
        public static async Task<string> SaveImageToExternalApi(IFormFile image)
        {
            string url = "https://upload-image-viettel.vercel.app/upload";

            HttpClient client = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            client.DefaultRequestHeaders.Clear();

            var fileStreamContent = new StreamContent(image.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            form.Add(fileStreamContent, "image", Guid.NewGuid().ToString() + ".jpg");

            using (var response = await client.PostAsync(url, form))
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error from upload image service");

                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject jsonResult = JObject.Parse(jsonResponse);
                string result = (string)jsonResult["url"]!;
                return result;

            }
        }
    }
}
