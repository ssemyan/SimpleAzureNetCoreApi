using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace SimpleCoreApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<ItemInfo> Get()
        {
			List<ItemInfo> stores = new List<ItemInfo>();
			string connStr;
			if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
			{
				// Running local
				connStr = "Server=localhost;Initial Catalog=scottsedisdb;Integrated Security=SSPI;Persist Security Info=True;";
			}
			else
			{
				// Running in Azure
				var azureServiceTokenProvider = new AzureServiceTokenProvider();
				var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
				// TODO: handle errors if the web app cannot connect to the keyvault or get the secret. 
				var secret = keyVaultClient.GetSecretAsync("https://scottsedisvault.vault.azure.net/secrets/ApiUserConnectionString-c19aed5e-4a64-41ce-9f0d-0b44300b223a/ba31031dc3094de0b6fa0907bd5a74c1").Result;
				connStr = secret.Value;
			}

			using (var connection = new SqlConnection(connStr))
			{
				var command = new SqlCommand("select itemId, itemName from Items", connection);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						ItemInfo test = new ItemInfo
						{
							itemId = (int)reader["itemId"],
							itemName = (string)reader["itemName"]
						};
						stores.Add(test);
					}
				}
			}
			return stores;
		}

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
