using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.PageTexts.Queries;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageTextsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PageTextsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPageTextsQuery());
            return Ok(result);
        }
    }
}
