using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Campbelltech.PostalCodeTax.DTO.Responses;
using Campbelltech.PostalCodeTax.Domain.Services;

namespace Campbelltech.PostalCodeTax.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ListPostalCodeTaxesController : ControllerBase
    {
        private readonly IPostalCodeTaxService _postalCodeTaxesService;

        public ListPostalCodeTaxesController(IPostalCodeTaxService postalCodeTaxesService)
        {
            _postalCodeTaxesService = postalCodeTaxesService;
        }

        /// <summary>
        /// Retrieves a list of postal codes and their corresponding tax calculation types
        /// </summary>  
        /// <response code="200">If a list of postal codes and their tax calculation types were successfully retrieved</response>
        /// <response code="204">The service found no postal codes to return</response>         
        /// <response code="401">Unauthorised - No token, invalid token or token expired</response>
        /// <response code="403">Forbidden - The user has been authenticated but is not allowed to make request</response> 
        /// <response code="500">Internal error in postal code retrieval process</response>    
        /// <returns>ActionResult + PostalCodeTaxResponse</returns>
        [HttpGet]
        [ProducesResponseType(201)]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PostalCodeTaxResponse>> ListAsync()
        {
            try
            {
                var postalCodeTaxes = await _postalCodeTaxesService.ListAsync();
                var haveResults = postalCodeTaxes != null && postalCodeTaxes.PostalCodeTaxes.Any();

                return haveResults ? Ok(postalCodeTaxes) : NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new PostalCodeTaxResponse
                {
                    Message = $"Error while attempting to retrieve a list of postal codes with their corresponding tax calculation types."
                });
            }
        }
    }
}