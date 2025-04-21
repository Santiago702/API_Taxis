using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APITaxiV2.Services;
using APITaxiV2.Models;
using Microsoft.AspNetCore.Authorization;
namespace APITaxiV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmail _emailService;

        public EmailController(IEmail emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("Enviar")]
        [Authorize]
        public IActionResult Email([FromBody] EmailDTO request)
        {
            try
            {
                _emailService.SendEmail(request);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Enviado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

    }
}
