using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookClient.Data
{
    public class BookManager
    {

        const string Url = "https://bookserver26341.azurewebsites.net/api/books/";
        private string authorizationKey = "45d6ec08-3501-4975-942e-296d77ed8f90";
    private async Task<HttpClient> GetClient()
    {
        HttpClient client = new HttpClient();
        if (string.IsNullOrEmpty(authorizationKey))
        {
            authorizationKey = await client.GetStringAsync(Url + "login");
            authorizationKey = JsonConvert.DeserializeObject<string>(authorizationKey);
        }

        client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        return client;
    }

    public async Task<IEnumerable<Book>> GetAll()
        {
            HttpClient client = await GetClient();
            var content = await client.GetStringAsync(Url);
            var allBooks = JsonConvert.DeserializeObject<IEnumerable<Book>>(content);

            return allBooks;
        }

        public async Task<Book> Add(string title, string author, string genre)
        {
            Book aBook = new Book
            {
                Title = title,
                Authors = new List<string>(new string[] { author }),
                Genre = genre,
                PublishDate = DateTime.Now
            };

            HttpClient client = await GetClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(aBook), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Url, content);

            var sResult = await response.Content.ReadAsStringAsync();
            Book theBook = JsonConvert.DeserializeObject<Book>(sResult);
            return theBook;
        }

        public async Task Update(Book book)
        {
            HttpClient client = await GetClient();
            await client.PutAsync(Url + "/" + book.ISBN,
                new StringContent(
                    JsonConvert.SerializeObject(book),
                    Encoding.UTF8, "application/json"));
        }

        public async Task Delete(string isbn)
        {
            HttpClient client = await GetClient();
            await client.DeleteAsync(Url + isbn);
        }
    }
}

