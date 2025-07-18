using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class CreateAuctionDTO
{

    [Required]
    public string make { get; set; }
    [Required]
    public string model { get; set; }
    [Required]
    public string color { get; set; }
    [Required]
    public int Mileage { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]

    public int ReservePrice { get; set; }
    [Required]
    public DateTime AuctionEnd { get; set; }
}
