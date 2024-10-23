using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class ChatController : Controller
{
    private readonly OpenAIService _openAIService;

    public ChatController(OpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Ask(string prompt)
    {
        if (string.IsNullOrEmpty(prompt))
        {
            ModelState.AddModelError("", "Please enter a prompt.");
            return View("Index");
        }

        var response = await _openAIService.GetResponseFromOpenAI(prompt);
        ViewBag.Response = response;
        return View("Index");
    }
}