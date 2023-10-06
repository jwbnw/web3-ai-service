using Microsoft.AspNetCore.Mvc;
using Web3Ai.Service.Authorization;
using Web3Ai.Service.Models;
using Web3Ai.Service.Services;

namespace Web3Ai.Service.Controllers;

//TODO: Add a controller exception filter or enter a ticket to do so.
public abstract class ServiceBaseController : ControllerBase
{
    // Downstream handles generic Ex and re-throws specific error
    // for this to handle. Execptions caught here should be known 
    // and safe to return to caller.
    protected async Task<ActionResult<TResult>> HandleActionResultErrorsAsync<TResult>(Func<Task<TResult>> expression)
    {
        try
        {
            return Ok(await expression.Invoke());
        }
        catch (Exception ex) // Temp. Exceptions here should be known..
        {
            return BadRequest(ex.Message);
        }
    }

}
