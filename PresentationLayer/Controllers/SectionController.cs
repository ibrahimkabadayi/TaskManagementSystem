using System.Security.Claims;
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

    public IActionResult TestPage()
    {
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "İbrahim Kabadayı"), 
        
            new Claim(ClaimTypes.NameIdentifier, "1"),
        
            new Claim(ClaimTypes.Role, "Admin") 
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        HttpContext.User = principal;

        TaskGroupDto taskGroupDto1;
        TaskGroupDto taskGroupDto2;
        TaskGroupDto taskGroupDto3;
        
        var mockProject = new ProjectDto
        {
            Id = 1,
            Name = "TaskFlow Web UI",
            StartDate = DateTime.Now,
            ProjectUsers =
            [
                new ProjectUserDto
                {
                    Id = 101, Title = "Frontend Dev", Role = ProjectRole.Leader,
                    User = new UserDto
                    {
                        Id = 1, Name = "İbrahim Kabadayı", Email = "ibrahim@test.com", Password = "123", ProfileLetters = "İK",
                        ProfileColor = "#0079bf"
                    },
                    ProjectId = 1,
                    IsActive = true,
                    JoinedDate = DateTime.Today
                },

                new ProjectUserDto
                {
                    Id = 102, Title = "Backend Lead", Role = ProjectRole.Developer,
                    User = new UserDto
                    {
                        Id = 2, Name = "Can Yılmaz", Email = "can@test.com", Password = "1234", ProfileLetters = "CY",
                        ProfileColor = "#4bbf6b"
                    },
                    ProjectId = 1,
                    IsActive = true,
                    JoinedDate = DateTime.Today
                },

                new ProjectUserDto
                {
                    Id = 103, Title = "Designer", Role = ProjectRole.Viewer,
                    User = new UserDto
                    {
                        Id = 3, Name = "Ayşe Demir", Email = "ayse@test.com", Password = "12345", ProfileLetters = "AD",
                        ProfileColor = "#ff9f1a"
                    },
                    ProjectId = 1,
                    IsActive = true,
                    JoinedDate = DateTime.Today
                }
            ]
        };

        var mockSection = new SectionDto
        {
            Id = 50,
            Name = "Sprint 1",
            ImageUrl = "https://www.freepik.com/free-photo/panoramic-shot-tranquil-lake-reflecting-blue-sky_10583765.htm#fromView=image_search_similar&page=1&position=0&uuid=d3f3501d-6215-4c9e-affe-4a038a30ff2d&query=lake+background",
            ProjectId = 1,
            TasksGroupDtos =
            [
                taskGroupDto3 = new TaskGroupDto
                {
                    Id = 1, Name = "Yapılacaklar (To Do)",
                    CreatedById = 1,
                    TaskDtos = new List<TaskDto>
                    {
                        new TaskDto
                        {
                            Id = 10, Title = "Login Ekranı Tasarımı",
                            Description = "Figma çizimleri incelenecek.",
                            AssignedTo = mockProject.ProjectUsers[0].User,
                            Priority = TaskPriority.High, State = TaskState.Todo,
                            StartDate = DateTime.Now, DueDate = DateTime.Now.AddDays(-2),
                            Position = 1
                        },
                        new TaskDto
                        {
                            Id = 11, Title = "Veritabanı Oluşturma",
                            Description = "MSSQL kurulumu ve migrationlar.",
                            AssignedTo = mockProject.ProjectUsers[1].User,
                            Priority = TaskPriority.Medium, State = TaskState.InProgress,
                            StartDate = DateTime.Now, DueDate = DateTime.Now.AddDays(-5),
                            Position = 2
                        }
                    }
                },

                taskGroupDto2 = new TaskGroupDto
                {
                    Id = 2, Name = "Devam Edenler (In Progress)",
                    CreatedById = 1,
                    TaskDtos = new List<TaskDto>
                    {
                        new TaskDto
                        {
                            Id = 12, Title = "API Gateway Entegrasyonu",
                            Description = "Ocelot konfigürasyonları yapılıyor...",
                            AssignedTo = mockProject.ProjectUsers[1].User, // CY
                            Priority = TaskPriority.High, State = TaskState.Done,
                            StartDate = DateTime.Now.AddDays(-2), DueDate = DateTime.Now.AddDays(1),
                            Position = 1
                        }
                    }
                },

                taskGroupDto1 = new TaskGroupDto
                {
                    Id = 3, Name = "Tamamlananlar (Done)",
                    CreatedById = 1,
                    TaskDtos = new List<TaskDto>
                    {
                        new TaskDto
                        {
                            Id = 13, Title = "Proje Kurulumu",
                            Description = "",
                            AssignedTo = mockProject.ProjectUsers[0].User, // İK
                            Priority = TaskPriority.Low, State = TaskState.Done,
                            StartDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(-5),
                            Position = 2
                        }
                    }
                }

            ]
        };
        taskGroupDto1.Section = mockSection;
        taskGroupDto2.Section = mockSection;
        taskGroupDto3.Section = mockSection;
        
        ViewBag.Project = mockProject;
        return View("SectionTasks", mockSection);
    }

    public async Task<IActionResult> TestWithRealData()
    {
        var section = await _sectionService.GetSectionWithTasksAsync(50);
        var project = await _projectService.GetProjectWithSectionAsync(1);
        
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

