using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace SimpleCoreApi.Controllers
{
	[Route("api/[controller]")]
    public class ValuesController : Controller
    {
		private readonly AppSettings _appSettings;

		public ValuesController(IOptions<AppSettings> appSettings)
		{
			// Get the app settings to use locally
			_appSettings = appSettings.Value;
		}

		// GET api/values - returns all values
		[HttpGet]
        public IEnumerable<ItemInfo> Get()
        {
			List<ItemInfo> items = new List<ItemInfo>();

			using (var connection = new SqlConnection(_appSettings.DbConnectionString))
			{
				var command = new SqlCommand("select itemId, itemName from Items", connection);
				connection.Open();
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						ItemInfo test = new ItemInfo
						{
							ItemId = (int)reader["itemId"],
							ItemName = (string)reader["itemName"]
						};
						items.Add(test);
					}
				}
			}
			return items;
		}

        // PUT api/values
        [HttpPut]
        public void Put([FromBody] ItemInfo newItem)
        {
			using (var connection = new SqlConnection(_appSettings.DbConnectionString))
			{
				var command = new SqlCommand("insert Items (itemName) select @itemName", connection);
				command.Parameters.AddWithValue("@itemName", newItem.ItemName);
				connection.Open();
				command.ExecuteNonQuery();
			}
		}

		// DELETE api/values/5 - deletes specified value
		[HttpDelete("{id}")]
        public void Delete(int id)
        {
			using (var connection = new SqlConnection(_appSettings.DbConnectionString))
			{
				var command = new SqlCommand("delete Items where itemId = " + id, connection);
				connection.Open();
				command.ExecuteNonQuery();
			}
		}
	}
}
