//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.AspNetCore.Mvc.ViewEngines;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Razor;

////public class CustomViewEngine : IViewEngine
////{
////    public ViewEngineResult GetView(string? executingFilePath, string viewPath, bool isMainPage)
////    {
////        return ViewEngineResult.NotFound(viewPath, new string[] { });
////    }
////    public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
////    {
////        string solutionPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\"));
////        string viewPath = $"{solutionPath}/Blaved.ViewsRazor/{viewName}.cshtml";

////        if (File.Exists(viewPath))
////        {
////            return ViewEngineResult.Found(viewPath, new CustomView(viewPath));
////        }
////        else
////        {
////            return ViewEngineResult.NotFound(viewName, new string[] { viewPath });
////        }
////    }
////}
////public class CustomView : IView
////{
////    public CustomView(string viewPath)
////    {
////        Path = viewPath;
////    }
////    public string Path { get; set; }
////    public async Task RenderAsync(ViewContext context)
////    {
////        string content = "";
////        using (StreamReader viewReader = new StreamReader(Path))
////        {
////            content = await viewReader.ReadToEndAsync();
////        }
////        await context.Writer.WriteAsync(content);
////    }
////}
//public class CustomViewEngine : IViewEngine
//{
//    private readonly IRazorViewEngine _razorViewEngine;

//    public CustomViewEngine(IRazorViewEngine razorViewEngine)
//    {
//        _razorViewEngine = razorViewEngine;
//    }

//    public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
//    {
//        // Путь к вашему файлу Razor
//        string filePath = "полный_путь_к_вашему_файлу.cshtml";

//        var view = new RazorView(_razorViewEngine, new RazorViewEngineResult(filePath, null, null));

//        return ViewEngineResult.Found(viewName, view);
//    }

//    public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
//    {
//        return ViewEngineResult.NotFound(viewPath, new string[] { });
//    }
//}

//public class RazorView : IView
//{
//    private readonly IRazorViewEngine _razorViewEngine;
//    private readonly RazorViewEngineResult _viewEngineResult;

//    public RazorView(IRazorViewEngine razorViewEngine, RazorViewEngineResult viewEngineResult)
//    {
//        _razorViewEngine = razorViewEngine;
//        _viewEngineResult = viewEngineResult;
//    }

//    public async Task RenderAsync(ViewContext context)
//    {
//        var view = await _razorViewEngine.FindView(context, _viewEngineResult.ViewName, _viewEngineResult.IsMainPage);

//        if (!view.Success)
//        {
//            throw new InvalidOperationException($"Couldn't find view '{_viewEngineResult.ViewName}'");
//        }

//        await view.View.RenderAsync(context);
//    }
//}
