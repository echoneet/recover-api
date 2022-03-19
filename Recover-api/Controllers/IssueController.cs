using Microsoft.AspNetCore.Mvc;
using Recover.Models;
using Recover.Requests;
using Recover.Responses;

namespace Recover.Controllers;

[ApiController]
[Route("issue")]
public class IssueController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public IssueController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public IActionResult List()
    {
        var issueList = _appDbContext.Issues?.ToList();

        var res = issueList.Select(issue => new ListIssueResponse
        {
            Id = issue.Id,
            Code = issue.Code,
            Description = issue.Description,
            Status = issue.IsCancelled ? "Cancelled" : "Created",
            Title = issue.Title
        }).ToList();

        return Ok(res);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateIssueRequest request)
    {
        var issue = new Issue();
        issue.Title = request.Title;
        issue.Description = request.Description;
        issue.Code = "";
        issue.IsCancelled = false;
        _appDbContext.Issues.Add(issue);
        _appDbContext.SaveChanges();

        issue.Code = $"AA-{issue.Id:000000}";
        _appDbContext.SaveChanges();

        var res = new CreateIssueResponse
        {
            Id = issue.Id,
            Code = issue.Code,
            Description = issue.Description,
            Status = issue.IsCancelled ? "Cancelled" : "Created",
            Title = issue.Title
        };

        return CreatedAtAction(nameof(Get), new {id = issue.Id}, res);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var issue = _appDbContext.Issues.Find(id);

        return Ok(issue);
    }

    [HttpPut("{id:int}/cancel")]
    public IActionResult Cancel(int id)
    {
        var issue = _appDbContext.Issues.Find(id);
        if (issue == null) return NotFound();

        issue.IsCancelled = true;
        _appDbContext.SaveChanges();

        var res = new CreateIssueResponse
        {
            Id = issue.Id,
            Code = issue.Code,
            Description = issue.Description,
            Status = issue.IsCancelled ? "Cancelled" : "Created",
            Title = issue.Title
        };
        return Ok(res);
    }
}