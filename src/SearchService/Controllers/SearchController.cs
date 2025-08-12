using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.RequestHelper;

namespace SearchService;


[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item, Item>();
        query.Sort(x => x.Ascending(a => a.make));

        if (!string.IsNullOrEmpty(searchParams.searchTerm))
        {
            query.Match(Search.Full, searchParams.searchTerm).SortByTextScore();
            
        }

        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };

        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
            && x.AuctionEnd > DateTime.UtcNow),

            _ => query.Match(x=> x.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrEmpty(searchParams.seller))
        {
            query.Match(x => x.Seller == searchParams.seller);
        }

        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);


        var res = await query.ExecuteAsync();
        return Ok(new
        {
            res = res.Results,
            pageCount = res.PageCount,
            totalCount = res.TotalCount
        });
    }
}
