using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Campbelltech.TaxCalculation.Domain.Services;
using Campbelltech.TaxCalculation.DTO.Requests;
using Campbelltech.TaxCalculation.DTO.Responses;

namespace Campbelltech.TaxCalculation.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalculateTaxController : ControllerBase
    {
        private readonly ITaxCalculationService _taxCalculationService;

        public CalculateTaxController(ITaxCalculationService taxCalculationService)
        {
            _taxCalculationService = taxCalculationService;
        }

        /// <summary>
        /// Calculates the total tax payable for a given postal code and annual salary
        /// </summary>  
        /// <param name="request">JSON representation of the TaxCalculationRequest object</param>
        /// <response code="201">If the tax calculation request was processed successfully and the results were persisted to the database</response>
        /// <response code="202">If the tax calculation request was processed successfully but the results were not persisted to the database</response>
        /// <response code="400">If request validation failed</response>         
        /// <response code="401">Unauthorised - No token, invalid token or token expired</response>
        /// <response code="403">Forbidden - The user has been authenticated but is not allowed to make a tax calculation request</response> 
        /// <response code="500">Internal error in tax calculation process</response>    
        /// <returns>ActionResult + TaxCalculationResponse</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TaxCalculationResponse>> PostAsync([FromBody] TaxCalculationRequest request)
        {
            try
            {
                // retrieve the username from the JWT claims
                var requestedByUser = HttpContext.User?.Identity?.Name;

                // execute main tax calculation logic
                var (statusCode, response) = await _taxCalculationService.CalculateAsync(request, requestedByUser);

                return StatusCode((int)statusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, new TaxCalculationResponse
                {
                    Message = $"Error while processing tax calculation request for {nameof(request.PostalCode)} - {request.PostalCode ?? ("Unknown")} and {nameof(request.AnnualIncome)} - {request.AnnualIncome}."
                });
            }
        }
    }
}