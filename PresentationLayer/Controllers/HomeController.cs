using Application.DTOs;
using Application.Services;
using AutoMapper;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult Home()
    {
        return View();
    }
    public IActionResult TaskFlow(UserDto user)
    {
        ViewData["User"] = user;
        return View();
    }
    public IActionResult CreateAccount()
    {
        return View();
    }
}