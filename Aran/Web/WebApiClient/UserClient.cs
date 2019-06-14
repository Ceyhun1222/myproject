using HttpRequest.Implementations;
using HttpRequest.Interfaces;
using HttpRequest.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UMCommonModels.Dto;

namespace WebApiClient
{
    public class UserClient
    {
        static readonly HttpClient Client = new HttpClient();

        public static string WebApiAddress { get; set; }

        public UserDto GetUserById(int id)
        {
            if (string.IsNullOrWhiteSpace(WebApiAddress))
                throw new Exception("Web API client address is undefined");

            var task = Client.GetAsync($"{WebApiAddress}/api/users/{id}/dto");
            var user = task.Result;
            return user.As<UserDto>();
        }

        public List<UserDto> GetUsers()
        {
            if (string.IsNullOrWhiteSpace(WebApiAddress))
                throw new Exception("Web API client address is undefined");

            var task = Client.GetAsync($"{WebApiAddress}/api/users/dto");
            var users = task.Result;
            return users.As<List<UserDto>>();
        }
    }
}
