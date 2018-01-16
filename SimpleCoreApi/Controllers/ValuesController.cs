using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.ServiceBus;
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
				connStr = Environment.GetEnvironmentVariable("LOCAL_DB_CONN_STR");
			}
			else
			{
				// Running in Azure
				var azureServiceTokenProvider = new AzureServiceTokenProvider();
				var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

				// Get the secret name from the env variable
				var dbKeyvaultId = Environment.GetEnvironmentVariable("DB_KEYVAULT_ID");

				// TODO: handle errors if the web app cannot connect to the keyvault or get the secret. 
				var secret = keyVaultClient.GetSecretAsync(dbKeyvaultId).Result;
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
        [HttpGet("{emailAddress}")]
        public string Get(string emailAddress)
        {
			string connStr;
			if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
			{
				// Running local
				connStr = Environment.GetEnvironmentVariable("LOCAL_SB_CONN_STR");
			}
			else
			{
				// Running in Azure
				var azureServiceTokenProvider = new AzureServiceTokenProvider();
				var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

				// Get the secret name from the env variable
				var sbKeyvaultId = Environment.GetEnvironmentVariable("SB_KEYVAULT_ID");

				// TODO: handle errors if the web app cannot connect to the keyvault or get the secret. 
				var secret = keyVaultClient.GetSecretAsync(sbKeyvaultId).Result;
				connStr = secret.Value;
			}

			string queueName = Environment.GetEnvironmentVariable("QUEUE_NAME");

			// Add a message to the queue to state what changed
			var topicClient = new QueueClient(connStr, queueName);

			// Create a new message to send to the topic.
			var message = new Message();
			message.UserProperties.Add("ToEmail", emailAddress);
			message.UserProperties.Add("Message", "Here is a queued message sent: " + DateTime.Now);
			message.Body = Encoding.ASCII.GetBytes(emailAddress);

			// Send the message to the topic.
			topicClient.SendAsync(message);

			return "Message Sent";
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
