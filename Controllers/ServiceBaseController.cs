using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web3Ai.Service.Authorization;
using Web3Ai.Service.Models;
using Web3Ai.Service.Services;

namespace Web3Ai.Service.Controllers;

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
        // Suppression is temp. Should be logged .
        catch (Exception ex)
        {
            Debug.WriteLine("Ex: handled in ServiceBaseController", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
