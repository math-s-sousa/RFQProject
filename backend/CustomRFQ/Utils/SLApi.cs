using System.Text.Json;
using System.Text;
using static CustomRFQ.Models.ServiceLayer;

namespace CustomRFQ.Utils;

public class SLApi
{
    private string _companyDB;
    private string _userName;
    private string _password;
    private string _baseUrl;
    private DateTime _sessionTimeOut;
    private string _token;

    public SLApi(string company, string username, string password, string url)
    {
        _companyDB = company;
        _userName = username;
        _password = password;
        _baseUrl = url;
    }

    private async Task<(Login success, Erro failed)> Login()
    {
        try
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(httpClientHandler);

            string body = JsonSerializer.Serialize(new { CompanyDB = _companyDB, UserName = _userName, Password = _password });

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var retorno = await client.PostAsync($"{_baseUrl}Login", content);

            if (retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                var slReturn = JsonSerializer.Deserialize<Login>(responseBody);

                _sessionTimeOut = DateTime.Now.AddMinutes(slReturn.SessionTimeout - 5);
                _token = slReturn.SessionId;

                return (slReturn, null);
            }
            else if (!retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                var slReturn = JsonSerializer.Deserialize<Erro>(responseBody);
                return (null, slReturn);
            }
            else
                return (null, null);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }

    public async Task<(bool success, Erro failed)> Logout()
    {
        try
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={_token}");

            var retorno = await client.PostAsync(_baseUrl + "Logout", null);

            if (retorno.IsSuccessStatusCode)
            {
                _sessionTimeOut = DateTime.Now.AddMinutes(-1);
                _token = string.Empty;
                return (true, null);
            }
            else if (!retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                var slReturn = JsonSerializer.Deserialize<Erro>(responseBody);
                return (false, slReturn);
            }
            else
                return (false, null);
        }
        catch (Exception ex)
        {
            throw new Exception("Error SL: " + ex.Message);
        }
    }

    public async Task<(string success, Erro failed)> Get(string endpoint)
    {
        try
        {
            if (DateTime.Now >= _sessionTimeOut)
                await Login();

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={_token}");

            var retorno = await client.GetAsync(_baseUrl + endpoint);

            if (retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                return (responseBody, null);
            }
            else if (!retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                var slReturn = JsonSerializer.Deserialize<Erro>(responseBody);
                return (null, slReturn);
            }
            else
                return (null, null);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }

    public async Task<(bool success, Erro failed)> Delete(string endpoint)
    {
        try
        {
            if (DateTime.Now >= _sessionTimeOut)
                await Login();

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={_token}");

            var retorno = await client.DeleteAsync(_baseUrl + endpoint);

            if (retorno.IsSuccessStatusCode)
                return (true, null);

            else if (!retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                var slReturn = JsonSerializer.Deserialize<Erro>(responseBody);
                return (false, slReturn);
            }
            else
                return (false, null);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }

    public async Task<(string success, Erro failed)> Post(string endpoint, string corpo)
    {
        try
        {
            if (DateTime.Now >= _sessionTimeOut)
                await Login();

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(httpClientHandler);
            client.Timeout = TimeSpan.FromMinutes(10);
            client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={_token}");

            var content = new StringContent(corpo, Encoding.UTF8, "application/json");

            var retorno = await client.PostAsync(_baseUrl + endpoint, content);

            if (retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                return (responseBody, null);
            }
            else if (!retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                var slReturn = JsonSerializer.Deserialize<Erro>(responseBody);
                return (null, slReturn);
            }
            else
                return (null, null);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }

    public async Task<(bool success, Erro failed)> Patch(string endpoint, string corpo, bool forcePatch = false)
    {
        try
        {
            if (DateTime.Now >= _sessionTimeOut)
                await Login();

            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={_token}");

            if (forcePatch) client.DefaultRequestHeaders.Add("B1S-ReplaceCollectionsOnPatch", "true");

            var content = new StringContent(corpo, Encoding.UTF8, "application/json");

            var retorno = await client.PatchAsync(_baseUrl + endpoint, content);

            if (retorno.IsSuccessStatusCode)
                return (true, null);

            else if (!retorno.IsSuccessStatusCode)
            {
                var responseBody = await retorno.Content.ReadAsStringAsync();
                var slReturn = JsonSerializer.Deserialize<Erro>(responseBody);
                return (false, slReturn);
            }
            else
                return (false, null);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }
}
