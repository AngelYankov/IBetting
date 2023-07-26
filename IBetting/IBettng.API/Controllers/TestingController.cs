using IBetting.Services.DataSavingService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IBettng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly IDataSavingService testingService;

        public TestingController(IDataSavingService testingService)
        {
            this.testingService = testingService;
        }

        /// <summary>
        /// Test endpoint for saving data from XML document
        /// </summary>
        /// <returns>Time elapsed to retrieve and save data from XML document</returns>
        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> UpdateData()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            await this.testingService.Save();
            s.Stop();
            return Ok(s.ElapsedMilliseconds + " milliseconds elapsed.");
        }
    }
}
