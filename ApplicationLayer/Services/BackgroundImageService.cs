using Application.Interfaces;

namespace Application.Services;

public class BackgroundImageService : IBackGroundImageService
{
    private readonly List<string> _backgroundImages = new()
    {
        "https://www.freepik.com/free-vector/diagonal-motion-lines-white-background_17564647.htm#fromView=keyword&page=1&position=0&uuid=7d8c5154-c7a4-4f6f-bfde-b8cb5f5cb0cc&query=Background",
        "https://www.freepik.com/free-photo/field-with-dramatic-scene-background_20111032.htm#fromView=search&page=1&position=3&uuid=5672b3d5-3a9c-4111-9913-f8dff10c3796&query=+nature+background",
        "https://www.freepik.com/free-photo/narrow-road-green-grassy-field-surrounded-by-green-trees-with-bright-sun-background_9852846.htm#fromView=search&page=1&position=2&uuid=5672b3d5-3a9c-4111-9913-f8dff10c3796&query=+nature+background",
        "https://www.freepik.com/free-ai-image/photorealistic-view-tree-nature-with-branches-trunk_186030467.htm#fromView=search&page=1&position=45&uuid=5672b3d5-3a9c-4111-9913-f8dff10c3796&query=+nature+background",
        "https://www.freepik.com/premium-photo/sunrise-lake-park-high-tatras-shtrbske-pleso_173476832.htm#from_element=cross_selling__photo",
        "https://www.freepik.com/free-ai-image/beautiful-mountains-landscape_65644858.htm#fromView=search&page=1&position=0&uuid=481ab34e-5494-4759-bd78-aca793caa7f8&query=mountain+background",
        "https://www.freepik.com/free-photo/panoramic-shot-plain-with-mountains-touching-sky_9971646.htm#fromView=search&page=1&position=17&uuid=481ab34e-5494-4759-bd78-aca793caa7f8&query=mountain+background",
        "https://www.freepik.com/free-photo/beautiful-shot-high-white-hilltops-mountains-covered-fog_7629796.htm#fromView=search&page=2&position=22&uuid=481ab34e-5494-4759-bd78-aca793caa7f8&query=mountain+background",
        "https://www.freepik.com/free-photo/water-ripple-texture-background-blue-design_19097364.htm#fromView=search&page=1&position=5&uuid=57239018-cd7d-4396-8a95-3afd7c049126&query=water+background",
        "https://www.freepik.com/free-photo/sunshine-clouds-sky-morning-background-blue-white-pastel-heaven-soft-focus-lens-flare-sunlight-abstract-blurred-cyan-gradient-peaceful-nature-open-view-out-windows-beautiful-summer-spring_1284996.htm#fromView=search&page=1&position=1&uuid=72bcfc46-8d6c-49ad-a1cd-dd5620bfdabe&query=sky+background",
        "https://www.freepik.com/free-vector/sky-realistic-landscape-with-clouds-grass-vector-illustration_53400224.htm#fromView=search&page=1&position=30&uuid=72bcfc46-8d6c-49ad-a1cd-dd5620bfdabe&query=sky+background",
        "https://www.freepik.com/free-photo/clear-sky-with-clouds_12108678.htm#fromView=search&page=1&position=46&uuid=72bcfc46-8d6c-49ad-a1cd-dd5620bfdabe&query=sky+background",
        "https://www.freepik.com/free-photo/panoramic-shot-tranquil-lake-reflecting-blue-sky_10583765.htm#fromView=image_search_similar&page=1&position=0&uuid=d3f3501d-6215-4c9e-affe-4a038a30ff2d&query=lake+background",
        "https://www.freepik.com/free-photo/lake-summer-sunny-day_10302961.htm#fromView=image_search_similar&page=1&position=10&uuid=d3f3501d-6215-4c9e-affe-4a038a30ff2d&query=lake+background",
        "https://www.freepik.com/premium-photo/scenic-view-lake-against-blue-sky_102223829.htm#fromView=image_search_similar&page=1&position=43&uuid=d3f3501d-6215-4c9e-affe-4a038a30ff2d&query=lake+background"
    };

    private readonly List<string> _defaultColorBackgroundImages = new()
    {
        "https://www.freepik.com/free-photo/water-drops-background_4221961.htm#fromView=search&page=1&position=1&uuid=3115b806-2aea-45ee-aa81-cc956e4b13e1&query=plain+color",
        "https://www.freepik.com/free-photo/red-concrete-textured-wall_4640638.htm#fromView=search&page=1&position=24&uuid=3115b806-2aea-45ee-aa81-cc956e4b13e1&query=plain+color",
        "https://www.freepik.com/free-photo/abstract-luxury-gold-yellow-gradient-studio-wall-well-use-as-background-layout-banner-product-presentation_16790969.htm#fromView=search&page=1&position=9&uuid=3115b806-2aea-45ee-aa81-cc956e4b13e1&query=plain+color",
        "https://www.freepik.com/free-photo/gray-smooth-textured-paper-background_15440848.htm#fromView=search&page=2&position=34&uuid=3115b806-2aea-45ee-aa81-cc956e4b13e1&query=plain+color",
        "https://www.freepik.com/premium-photo/minimalistic-soft-gradient-background-design_204434539.htm#from_element=cross_selling__photo",
        "https://www.freepik.com/free-photo/beautiful-abstract-cloud-clear-blue-sky-landscape-nature-white-background-wallpaper-blue-tex_30776379.htm#fromView=search&page=3&position=8&uuid=3115b806-2aea-45ee-aa81-cc956e4b13e1&query=plain+color",
        "https://www.freepik.com/free-photo/luxury-gold-shiny-background-with-variating-hues_26773947.htm#fromView=search&page=5&position=10&uuid=3115b806-2aea-45ee-aa81-cc956e4b13e1&query=plain+color",
        "https://www.freepik.com/premium-photo/colorful-holographic-gradient-background-design_211683235.htm#fromView=search&page=6&position=37&uuid=3115b806-2aea-45ee-aa81-cc956e4b13e1&query=plain+color"
    };

    public List<string> GetDefaultColorBackgroundImages()
    {
        return _defaultColorBackgroundImages;
    }

    public List<string> GetBackgroundImages()
    {
        return _backgroundImages;
    }
}