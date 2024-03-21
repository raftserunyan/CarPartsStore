using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace CarPartsStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class LogsController : Controller
    {
        private readonly ILogger<LogsController> _logger;
        private const string _filePath = @"Logs\myLogs.log";

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                string ownLogs;

                using (StreamReader reader = new StreamReader(_filePath))
                {
                    ownLogs = reader.ReadToEnd();
                }

                string[] str = Regex.Split(ownLogs, @"//--raf--rr");

                string[][] model = new string[str.Length / 7][];

                for (int i = 0; i < str.Length / 7; i++)
                {
                    string[] temp = new string[7];
                    for (int j = 0; j < 7; j++)
                    {
                        temp[j] = str[(i * 7) + j];
                    }
                    model[i] = temp;
                }

                return View("Index", model);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public IActionResult ClearLogs()
        {
            try
            {
                System.IO.File.WriteAllText(_filePath, string.Empty);

                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
