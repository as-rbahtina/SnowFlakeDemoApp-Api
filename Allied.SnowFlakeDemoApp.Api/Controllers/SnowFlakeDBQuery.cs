using Apache.Arrow;
using Microsoft.AspNetCore.Mvc;
using Mono.Unix.Native;
using Snowflake.Data.Client;
using System.Data.Common;
using System.Data;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Text;
using RestSharp;
using Allied.SnowFlakeDemoApp.Domain.Models;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Allied.SnowFlakeDemoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SnowFlakeDBQuery : ControllerBase
    {

        private readonly ILogger<SnowFlakeDBQuery> _logger;

        public SnowFlakeDBQuery(ILogger<SnowFlakeDBQuery> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "QueryWithSnowflakeSQLAPI")]
        public async Task<string> QueryWithSnowflakeSQLAPI(SnowFlakeRequest query)
        {
            string result ="";
            string account = "ss53963.east-us-2.azure";
            string apiUrl = $"https://{account}.snowflakecomputing.com/";

            RestClient restClient = new(apiUrl);

            try
            {
                //requestId and retry are optional
                var restRequest = new RestRequest($"api/v2/statements/?requestId={Guid.NewGuid()}&retry=true", Method.Post);
                var request = new SnowFlakeSqlApiRequest()
                {
                    statement = query.Query,
                    warehouse = "EDW_DEV_INFA_WH",
                    role = "CTPT_DEMO_USERS",
                    database = query.Database,
                    schema = query.Schema,
                };
                restRequest.AddHeader("Content-Type", "application/json");
                restRequest.AddHeader("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Im9hdXRoc2lnbmluZ2NlcnQiLCJwaS5hdG0iOiIxIn0.eyJzY29wZSI6WyJzZXNzaW9uOnJvbGU6Q1RQVF9ERU1PX1VTRVJTIl0sImNsaWVudF9pZCI6IlNub3dmbGFrZS1Tbm93Q0xJIiwiaXNzIjoiYWxsaWVkX3NvbHV0aW9uc19vYXV0aDJfYXMiLCJhdWQiOiJodHRwczovL3NzNTM5NjMuZWFzdC11cy0yLmF6dXJlLnNub3dmbGFrZWNvbXB1dGluZy5jb20iLCJzdWJqZWN0Ijoic3ZjX2N0cHRkZW1vX2RldjAxIiwiZXhwIjoxNzMyMDU4Njg0fQ.MPjpwcnNsXImOIXnvfpepoaD0ZULooiEXJJa3s9eDCteSwLD-0Fp8YzhzN-_JriE-4PGIFZ81uwRdu409p_LNp-iQs7LdW0sroiRADUBLQTssvu45cg7G-12CAPBytmtmtYe8J_RjKNccuUMOoIh10JWjRxb3S1rOpJCh0zoLdXeKHDghj2DkUmT8uvWGw73eb_dygJCI1dzGvW5vrYnJ027vkrgjlarbT9hu6zOUhoOZg7m4zaaYJ7i9sUL-ZKsh2brI5H3AuS1Z50DfLoBXl7SqRVvahE31xAKJhL1yl5gZx8ZpDzxTv0twZ9szP_1YUL_1o2rLF_i1EXstXv38Q");
                restRequest.AddHeader("X-Snowflake-Authorization-Token-Type", "OAUTH");
                restRequest.AddHeader("Accept", "application/json");

                restRequest.AddJsonBody(request);

                var response = await restClient.ExecuteAsync<RestResponse>(restRequest);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    var json = JObject.Parse(response.Content.ToString());
                    result = json.ToString();
                }
                else
                {
                    result = response.Content;
                }
                

            } catch(Exception ex)
            {
                result = ex.Message;
            }



            return result;
        }

        [HttpPost(Name = "QueryWithSnowflakeNetDriver")]
        public string QueryWithSnowflakeNetDriver(SnowFlakeRequest query)
        {
            string response = "";
            string connectionString = $"account=ss53963.east-us-2.azure;user=SVC_CTPTDEMO_DEV01;password=Q^j5m*caL21ZY3XP;db={query.Database};schema={query.Schema};ROLE=CTPT_DEMO_USERS;TOKEN=eyJhbGciOiJSUzI1NiIsImtpZCI6Im9hdXRoc2lnbmluZ2NlcnQiLCJwaS5hdG0iOiIxIn0.eyJzY29wZSI6WyJzZXNzaW9uOnJvbGU6Q1RQVF9ERU1PX1VTRVJTIl0sImNsaWVudF9pZCI6IlNub3dmbGFrZS1Tbm93Q0xJIiwiaXNzIjoiYWxsaWVkX3NvbHV0aW9uc19vYXV0aDJfYXMiLCJhdWQiOiJodHRwczovL3NzNTM5NjMuZWFzdC11cy0yLmF6dXJlLnNub3dmbGFrZWNvbXB1dGluZy5jb20iLCJzdWJqZWN0Ijoic3ZjX2N0cHRkZW1vX2RldjAxIiwiZXhwIjoxNzMyMDU4Njg0fQ.MPjpwcnNsXImOIXnvfpepoaD0ZULooiEXJJa3s9eDCteSwLD-0Fp8YzhzN-_JriE-4PGIFZ81uwRdu409p_LNp-iQs7LdW0sroiRADUBLQTssvu45cg7G-12CAPBytmtmtYe8J_RjKNccuUMOoIh10JWjRxb3S1rOpJCh0zoLdXeKHDghj2DkUmT8uvWGw73eb_dygJCI1dzGvW5vrYnJ027vkrgjlarbT9hu6zOUhoOZg7m4zaaYJ7i9sUL-ZKsh2brI5H3AuS1Z50DfLoBXl7SqRVvahE31xAKJhL1yl5gZx8ZpDzxTv0twZ9szP_1YUL_1o2rLF_i1EXstXv38Q;AUTHENTICATOR=OAUTH;warehouse=Sandbox";

            using (SnowflakeDbConnection conn = new SnowflakeDbConnection(connectionString))
            {
                conn.Open();

                using (SnowflakeDbCommand cmd = new SnowflakeDbCommand(conn, query.Query))
                {
                    using (SnowflakeDbDataReader reader = (SnowflakeDbDataReader)cmd.ExecuteReader())
                    {
                        response = DataReaderToJson(reader);

                    }
                }
            }

            return response;
        }

        private string DataReaderToJson( SnowflakeDbDataReader rdr)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.WriteStartArray();

                while (rdr.Read())
                {
                    jsonWriter.WriteStartObject();

                    int fields = rdr.FieldCount;

                    for (int i = 0; i < fields; i++)
                    {
                        jsonWriter.WritePropertyName(rdr.GetName(i));
                        jsonWriter.WriteValue(rdr[i]);
                    }

                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndArray();

                return sw.ToString();
            }
        }
    }
}
