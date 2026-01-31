using System.Security.Claims;
using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.SectionRequests;

namespace TaskManagementSystem.Controllers;

public class SectionController : Controller
{
    private readonly ISectionService _sectionService;
    private readonly IProjectUserService _projectUserService;
    private readonly IProjectService _projectService;

    public SectionController(ISectionService sectionService, IProjectUserService projectUserService,  IProjectService projectService)
    {
        _sectionService = sectionService;
        _projectUserService = projectUserService;
        _projectService = projectService;
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateBackgroundUrl([FromBody] ChangeSectionBackgroundUrlRequest request)
    {
        var result = await _sectionService.ChangeBackgroundUrl(request.SectionId, request.Url);
        return (result == request.Url) ? Ok() : BadRequest();
    }
    
    [HttpGet("Section/GetProjectUsers/{projectId:int}")]
    public async Task<IActionResult> GetProjectUsers(int projectId)
    {
        var result = await _projectUserService.GetAllProjectUserDetailsOfOneProjectAsync(projectId);
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> TaskFlow(int userId)
    {
        var allProjects = await _projectService.GetAllProjectsOfUserAsync(userId);

        var lastVisitedSections = new List<RecentSectionCookieDto>();
    
        var cookieName = $"TaskFlow_RecentSections_{userId}";
    
        var cookie = Request.Cookies[cookieName];

        if (!string.IsNullOrEmpty(cookie))
        {
            try
            {
                lastVisitedSections = JsonSerializer.Deserialize<List<RecentSectionCookieDto>>(cookie);
            }
            catch
            {
                lastVisitedSections = [];
            }
        }

        ViewBag.LastVisitedSections = lastVisitedSections;

        return View(allProjects);
    }
    
    private void AddToRecentlyViewed(int userId, int sectionId, string name, string imageUrl)
    {
        var cookieName = $"TaskFlow_RecentSections_{userId}";

        var existingCookie = Request.Cookies[cookieName];

        var recentList = (!string.IsNullOrEmpty(existingCookie) 
            ? JsonSerializer.Deserialize<List<RecentSectionCookieDto>>(existingCookie) 
            : []) ?? [];

        var existingItem = recentList.FirstOrDefault(x => x.Id == sectionId);
        if (existingItem != null)
        {
            recentList.Remove(existingItem);
        }

        recentList.Insert(0, new RecentSectionCookieDto 
        { 
            Id = sectionId, 
            Name = name, 
            ImageUrl = imageUrl 
        });

        if (recentList.Count > 3)
        {
            recentList = recentList.Take(3).ToList();
        }

        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(30),
            HttpOnly = true,
            Secure = true,
            IsEssential = true
        };

        var jsonString = JsonSerializer.Serialize(recentList);
    
        Response.Cookies.Append(cookieName, jsonString, cookieOptions);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSection([FromBody] CreateSectionRequest request)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("SignIn", "Home");
        
        var createdSection = await _sectionService.CreateSectionAsync(request.ProjectId, request.Name, request.ImageUrl);

        return (createdSection.Name == request.Name)
            ? Ok(createdSection)
            : BadRequest(new { message = "Section could not be created." });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSection(int sectionId)
    {
        var result = await _sectionService.DeleteSectionAsync(sectionId);
        return (result) ? Ok() : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("SignIn", "Home");
        
        var result = await _projectService.CreateProjectAsync(request.Name, request.Description, int.Parse(userId));
        return (result.Name == request.Name) ? Ok(result) : BadRequest(new { message = "Project could not be created." });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectRequest request)
    {
        var result = await _projectService.UpdateProjectAsync(request.ProjectId, request.Name, request.Description, request.StartDate, request.EndDate);
        return (result == request.ProjectId) ? Ok() : BadRequest(new { message = "Project could not be updated." });
    }
    
    [HttpDelete("~/Section/DeleteProject/{wsId:int}")]
    public async Task<IActionResult> DeleteProject(int wsId) 
    {
        var result = await _projectService.DeleteProjectAsync(wsId);
        return (result) ? Ok() : BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> SectionTasks(int sectionId)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("SignIn", "Home");
        
        var section = await _sectionService.GetSectionWithTasksAsync(sectionId);
        
        if (section == null) return RedirectToAction("Home", "Home");
        
        var project = await _projectService.GetProjectWithSectionAsync(section.ProjectId);
        
        AddToRecentlyViewed(int.Parse(userId), sectionId, section.Name, section.ImageUrl);
        
        ViewBag.Project = project;
        return View(section);
    }
    
    public async Task<IActionResult> TestWithRealData()
    {
        var section = await _sectionService.GetSectionWithTasksAsync(50);
        
        if (section == null) return RedirectToAction("Home", "Home");
        
        var project = await _projectService.GetProjectWithSectionAsync(section.ProjectId);
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, "İbrahim Kabadayı"), 
        
            new (ClaimTypes.NameIdentifier, "1"),
        
            new (ClaimTypes.Role, "Admin") 
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        HttpContext.User = principal;
        
        ViewBag.Project = project;
        return View("SectionTasks", section);
    }
    
    public async Task<IActionResult> Index(int projectId = 1) 
    {
        var sections = await _sectionService.GetSectionsByProjectAsync(projectId);

        return View("SectionTasks",sections.FirstOrDefault());
    }

    [HttpGet]
    public async Task<IActionResult> SectionUsers(int projectId)
    {
        var project = await _projectService.GetProjectWithSectionAsync(projectId);
        
        ViewBag.ProjectName = project?.Name;
        ViewBag.ProjectId = projectId;
        
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (project == null) return RedirectToAction("TaskFlow", "Section", new { userId });

        var projectUsers = project.ProjectUsers;
        
        return View(projectUsers);
    }
}

