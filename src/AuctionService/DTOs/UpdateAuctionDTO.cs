using System;

namespace AuctionService.DTOs;

public class UpdateAuctionDTO
{
    public string make { get; set; }
    public string model { get; set; }
    public int Year { get; set; }
    public string color { get; set; }
    public int Mileage { get; set; }
}
