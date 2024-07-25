using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blaved.ViewsRazor.Pages.Shared
{
    public class StartPage : PageModel
    {
        public string SolutionPath { get; private set; }

        public void OnGet()
        {
            SolutionPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\"));
        }
    }
}
