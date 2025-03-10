using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PostListApp
{
    public partial class MainPage : ContentPage
    {
        private const string ApiUrl = "https://jsonplaceholder.typicode.com/posts";

        public MainPage()
        {
            InitializeComponent();
            _ = FetchPostsAsync();
        }

        private async Task FetchPostsAsync()
        {
            try
            {
                // Show loading indicator
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                ErrorMessageLabel.IsVisible = false;

                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(ApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var posts = JsonConvert.DeserializeObject<List<Post>>(jsonResponse);

                    // Bind data to ListView
                    PostsListView.ItemsSource = posts;
                }
                else
                {
                    ShowErrorMessage("Failed to retrieve posts.");
                }
            }
            catch (Exception)
            {
                ShowErrorMessage("An error occurred while loading posts.");
            }
            finally
            {
                // Hide loading indicator
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
            }
        }

        private void ShowErrorMessage(string message)
        {
            ErrorMessageLabel.Text = message;
            ErrorMessageLabel.IsVisible = true;
        }
    }

    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}