using Keycloak_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json;

namespace Keycloak_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeycloakController : Controller
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _keycloakUrl;
        public KeycloakController(string clientId = "appcliente-api", 
            string clientSecret = "uoaMVcRhTtZmpYdYzs8oUIYENUPQYI0L", 
            string keycloakUrl = "http://localhost:8080/realms/AppRealm/protocol/openid-connect/token")
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _keycloakUrl = keycloakUrl;
        }

        [HttpPost(Name = "GetToken/User")]
        public async Task<IActionResult> GetToken([FromBody] User user)
        {
            using(HttpClient httpClient = new HttpClient())
            {
                string apiUrl = _keycloakUrl;

                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "username", user.Username },
                    { "password", user.Password }
                };

                HttpContent content = new FormUrlEncodedContent(parameters);

                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                if(response.IsSuccessStatusCode)
                {
                    string resposeBody = await response.Content.ReadAsStringAsync();
                    string result = JsonConvert.DeserializeObject<ApiResponse>(resposeBody).access_token;
                    return Ok(result);
                }
                else
                {
                    return StatusCode(404, "Falha ao identificar usuário");
                }
            }
        }
    }
}
