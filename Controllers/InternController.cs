
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Feature.InternFeature.Commands;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.AddTrack.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace LMS___Mini_Version.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InternController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InternController(IMediator mediator)
        {
            _mediator= mediator;
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<int>>> RegisterNewIntern(
         [FromBody] RegisterNewInternCommand command)
        {
            var result = await _mediator.Send(command);

            var response = result.Success
           ? EndpointResponse<int>.OK(result.Data, result.StatusCode, "Intern registered successfully")
           : EndpointResponse<int>.Fail(result.StatusCode, "Failed to register intern", result.Errors);

            return StatusCode(response.StatusCode, response);

            
        }


    }

    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<>>> GetAll()
    //{

    //}

    //[HttpGet("{id}")]
    //public async Task<ActionResult<>> GetById(int id)
    //{

    //}

    //[HttpPost]
    //public async Task<ActionResult<>> Create( )
    //{



    //}

    //[HttpPut("{id}")]
    //public async Task<ActionResult> Update()
    //{

    //}

    //[HttpDelete("{id}")]
    //public async Task<ActionResult> Delete()
    //{

    //}

}
