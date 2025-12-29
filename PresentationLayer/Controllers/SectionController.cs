using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Models;
using TaskManagementSystem.Models.SectionRequests;

namespace TaskManagementSystem.Controllers;

public class SectionController : Controller
{
   private readonly ISectionService _sectionService;
   
   public SectionController(ISectionService sectionService)
   {
        _sectionService = sectionService;
   }

   [HttpPatch]
   public async Task<IActionResult> UpdateBackgroundUrl(ChangeSectionBackgroundUrlRequest request)
   {
       var result  = await _sectionService.ChangeBackgroundUrl(request.SectionId, request.Url);
       return (result == request.Url) ?  Ok() : BadRequest();
   }
}