using System;

namespace AuctionService.DTOs;

public class AuctionDto
{
    public Guid Id { get; set; }
    public int ReservePrice { get; set; }
    public string Seller { get; set; } = string.Empty; // Username from claim
    public string Winner { get; set; } = string.Empty; // Username of winner
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime AuctionEnd { get; set; }
    public String Status { get; set; }


    public string make { get; set; }
    public string model { get; set; }
    public string color { get; set; }
    public int Mileage { get; set; }
    public int Year { get; set; }
    public string ImageUrl { get; set; }
}
