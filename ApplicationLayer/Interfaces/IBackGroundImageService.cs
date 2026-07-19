using System.Collections.Generic;

namespace Application.Interfaces;

public interface IBackGroundImageService
{
    List<string> GetDefaultColorBackgroundImages();
    List<string> GetBackgroundImages();
}