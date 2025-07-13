using System.ComponentModel.DataAnnotations.Schema;
using AuctionService.Modules;
namespace AuctionService.Modules;


[Table("items")]
public class Item
{
    internal int year;

    public Guid Id { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public int Mileage { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public int Year { get; set; }

        //navigation property
        public Auction Auction { get; set; }

        public Guid AuctionId { get; set; }
}
