using System;
using AuctionService.data;
using AuctionService.DTOs;
using AuctionService.Modules;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controlleres;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{

    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionController(AuctionDbContext contex, IMapper mapper)
    {

        _context = contex;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
    {
        var query = _context.Auctions.OrderBy(x => x.Item.make).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null) return NotFound();

        return _mapper.Map<AuctionDto>(auction);
    }
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDTO auctionDTO)
    {
        var auction = _mapper.Map<Auction>(auctionDTO);
        //todo add a current user as  seller
        auction.Seller = "test";
        _context.Auctions.Add(auction);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database.");

        return CreatedAtAction(nameof(GetAuctionById),
         new { id = auction.Id }, _mapper.Map<AuctionDto>(auction));
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<AuctionDto>> UpdateAuction(Guid id, UpdateAuctionDTO auctionDTO)
    {
        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null) return NotFound();
        auction.Item.make = auctionDTO.make ?? auction.Item.make;
        auction.Item.model = auctionDTO.model ?? auction.Item.model;
        auction.Item.Year = auctionDTO.Year != 0 ? auctionDTO.Year : auction.Item.Year;
        auction.Item.color = auctionDTO.color ?? auction.Item.color;
        auction.Item.Mileage = auctionDTO.Mileage != 0 ? auctionDTO.Mileage : auction.Item.Mileage;
        auction.UpdatedAt = DateTime.UtcNow;
        _context.Auctions.Update(auction);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)  BadRequest("Could not update the auction.") ;
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if (auction == null) return NotFound();

        //TODO seller == username

        _context.Auctions.Remove(auction);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");
        return Ok();
    }

 }
