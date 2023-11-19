using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UserManagement_Model;
using User_Management_Web.Services;
using System.Diagnostics;
using User_Management_Web.Models;
using Microsoft.Extensions.Configuration;

namespace User_Management_Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public UserController(UserService userService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _userService = userService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _baseUrl = configuration["AppSettings:BaseUrl"] ?? "http://localhost:61066";
        }
        public async Task<IActionResult> Index()
        {
            // Make an API request to fetch user data
            var users = await FetchUserDataFromApi();

            _userService.AddUsers(users);

            return View(users);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDto user)
        {
            // Make an API request to fetch user data
            await CreateUserDataInApi(user);

            _userService.AddUser(user);
            return RedirectToAction("Index");
        }

        public IActionResult EditUser(int id)
        {
            var user = _userService.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserDto user)
        {
            _userService.UpdateUser(user);

            await UpdateUserDataInApi(user);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            _userService.DeleteUser(id);

            await DeleteUserDataInApi(id);
            
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private async Task<List<UserDto>> FetchUserDataFromApi()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {

                // Adjust the API endpoint URL accordingly
                var apiUrl = $"{_baseUrl}/api/user/getAll";

                var response = await httpClient.GetStringAsync(apiUrl);

                // Deserialize the JSON response into a list of User objects
                var users = JsonConvert.DeserializeObject<List<UserDto>>(response);

                return users;
            }
        }

        private async Task<bool> CreateUserDataInApi(UserDto user)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = $"{_baseUrl}/api/user/create";

                // Convert the User object to JSON
                var jsonContent = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Make the POST request
                var response = await httpClient.PostAsync(apiUrl, content);

                // Check if the request was successful
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> UpdateUserDataInApi(UserDto user)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                // Adjust the API endpoint URL accordingly
                var apiUrl = $"{_baseUrl}/api/user/update";

                // Convert the User object to JSON
                var jsonContent = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Make the HTTP PUT request to update the user
                var response = await httpClient.PutAsync($"{apiUrl}/{user.UserId}", content);

                response.EnsureSuccessStatusCode(); // Throw an exception if the request is not successful

                // Check if the request was successful
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteUserDataInApi(int userId)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                // Adjust the API endpoint URL accordingly
                var apiUrl = $"{_baseUrl}/api/user/remove";

                // Make the HTTP PUT request to update the user
                var response = await httpClient.DeleteAsync($"{apiUrl}/{userId}");

                response.EnsureSuccessStatusCode(); // Throw an exception if the request is not successful

                // Check if the request was successful
                return response.IsSuccessStatusCode;
            }
        }

    }
}
