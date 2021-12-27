using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MovieAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<Movie> Movies { get; set; }
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44368/api/Movies").Result.Content.ReadAsStringAsync();
                Movies = JsonConvert.DeserializeObject<List<Movie>>(response);
            }

        }

        public async Task OnPostFindMovieByTitle(string title)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44368/api/Movies").Result.Content.ReadAsStringAsync();
                Movies = JsonConvert.DeserializeObject<List<Movie>>(response).Where(x => x.Title == title).ToList();
            }

            

        }

        /* public int Id { get; set; }
           public string Title { get; set; }
           public string Director { get; set; }
           public int YearReleased { get; set; } */

        public async void OnPostFindMovie(string Title, string Director, int YearReleased)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44368/api/Movies");

                var movie = new Movie();
                movie.Director = Director;
                movie.Title = Title;
                movie.YearReleased = YearReleased;

                var json = JsonConvert.SerializeObject(movie);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await client.PostAsync("Movies", content);
                string resContent = await result.Content.ReadAsStringAsync();
                

            }
            await OnGet();
        }

        
    }
}
