using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace TrackingRecordAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingTrackController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ShippingTrackController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetLatestShipmentStatus")]
        public IActionResult GetLatestShipmentStatus([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetLatestShipmentStatusByDateRange", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var results = new List<dynamic>();

                        while (reader.Read())
                        {
                            var result = new
                            {
                                TrackingNumber = reader["TrackingNumber"],
                                ShipmentDate = reader["ShipmentDate"],
                                LatestShipmentStatus = reader["LatestShipmentStatus"],
                                StatusDate = reader["StatusDate"]
                            };

                            results.Add(result);
                        }

                        return Ok(results);
                    }

                }
            }
        }

        

    }
}