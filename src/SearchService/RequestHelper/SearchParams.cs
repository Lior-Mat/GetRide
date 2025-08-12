using System;

namespace SearchService.RequestHelper;

public class SearchParams
{

    public string searchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 4;
    public string seller { get; set; }

    public string Winner { get; set; }

    public string OrderBy { get; set; }
    public string FilterBy { get; set; }



}
