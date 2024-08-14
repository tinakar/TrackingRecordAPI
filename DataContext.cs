using Microsoft.EntityFrameworkCore;

namespace TrackingRecordAPI
{
    public class DataContext : DbContext
    {
       
            public DataContext(DbContextOptions<DataContext> options) : base(options)
            {
            }

        
    }
}
