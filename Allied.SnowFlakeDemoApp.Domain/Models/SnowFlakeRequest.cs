using System.ComponentModel;

namespace Allied.SnowFlakeDemoApp.Api
{
    public class SnowFlakeRequest
    {
        [DefaultValue("DEMO_DB")]
        public string? Database { get; set; }

        [DefaultValue("RLS")]
        public string? Schema { get; set; }

        [DefaultValue("select * from lender")]
        public string? Query { get; set; }

    }
}
