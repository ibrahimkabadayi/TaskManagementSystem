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
    
    public async Task<IActionResult> TaskFlow(int userId = 1)
    {
        var allProjects = await _projectService.GetAllProjectsOfUserAsync(userId);
        
        var lastVisitedSections = new List<RecentSectionCookieDto>();
        var cookie = Request.Cookies["TaskFlow_RecentSections"];

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
    
    private void AddToRecentlyViewed(int sectionId, string name, string imageUrl)
    {
        const string cookieName = "TaskFlow_RecentSections";

        var existingCookie = Request.Cookies[cookieName];
    
        var recentList = !string.IsNullOrEmpty(existingCookie) ? JsonSerializer.Deserialize<List<RecentSectionCookieDto>>(existingCookie) :
        [
        ];
        
        if (recentList == null) return;

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
            Secure = true
        };

        var jsonString = JsonSerializer.Serialize(recentList);
        Response.Cookies.Append(cookieName, jsonString, cookieOptions);
    }


    [HttpGet]
    public async Task<IActionResult> SectionTasks(int sectionId)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("SignIn", "Home");
        
        var section = await _sectionService.GetSectionWithTasksAsync(sectionId);
        
        if (section == null) return RedirectToAction("Home", "Home");
        
        var project = await _projectService.GetProjectWithSectionAsync(section.ProjectId);
        
        AddToRecentlyViewed(sectionId, section.Name, section.ImageUrl);
        
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
    public IActionResult Members(int projectId = 1)
    {
        
        ViewBag.ProjectName = "TaskFlow Web UI | Sprint 1";
        
        var dummyMembers = new List<ProjectUserDto>
        {
               
            new ProjectUserDto
            {
                Id = 101,
                ProjectId = projectId,
                Role = ProjectRole.Leader, 
                Title = "Team Lead",
                IsActive = true,
                JoinedDate = DateTime.Now.AddMonths(-6),
                User = new UserDto
                {
                     
                    Id = 1,
                    Name = "İbrahim Kabadayı",
                    Email = "ibrahim@test.com",
                    ProfileLetters = "İK",
                    ProfileColor = "#0079bf",
                    Password = "123456"
                }
            },
            new ProjectUserDto
            {
                Id = 102,
                ProjectId = projectId,
                Role = ProjectRole.Developer,
                Title = "Backend Developer",
                IsActive = true,
                JoinedDate = DateTime.Now.AddMonths(-2),
                User = new UserDto
                {
                    Id = 2,
                    Name = "Can Yılmaz",
                    Email = "can.yilmaz@test.com",
                    ProfileLetters = "CY",
                    ProfileColor = "#4bbf6b",
                    Password = "123456"
                }
            },
            new ProjectUserDto
            {
                Id = 103,
                ProjectId = projectId,
                Role = ProjectRole.Viewer,
                Title = "UI/UX Designer",
                IsActive = false,
                JoinedDate = DateTime.Now.AddDays(-15),
                User = new UserDto
                { 
                    Id = 3,
                    Name = "Ayşe Demir",
                    Email = "ayse.d@test.com",
                    ProfileLetters = "AD",
                    ProfileColor = "#ff9f1a",
                    Password = "123456"
                }
            },
            new ProjectUserDto
            {
                Id = 104,
                ProjectId = projectId,
                Role = ProjectRole.Developer,
                Title = "QA Tester",
                IsActive = false,
                JoinedDate = DateTime.Now.AddDays(-5),
                User = new UserDto
                { 
                    Id = 4, 
                    Name = "Mehmet Öz",
                    Email = "mehmet.oz@test.com",
                    ProfileLetters = "MÖ",
                    ProfileColor = "#eb5a46",
                    Password = "123456"
                    
                }
            }
        };
        return View("SectionUsers", dummyMembers);
    }
}

