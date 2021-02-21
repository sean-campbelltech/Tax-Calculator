using System;
using System.Collections.Generic;
using System.Linq;
using Campbelltech.PostalCodeTax.Domain.Data_Models;
using Campbelltech.PostalCodeTax.DTO.Responses;
using Microsoft.Extensions.Logging;

namespace Campbelltech.PostalCodeTax.Domain.Mapping
{
    public class ResponseMapper : IResponseMapper
    {
        private readonly ILogger<ResponseMapper> _logger;

        public ResponseMapper(ILogger<ResponseMapper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Maps a list of PostalCodeTaxModel to the PostalCodeTaxResponse object
        /// </summary>
        /// <param name="postalCodeTaxModels">List of PostalCodeTaxModel</param>
        /// <returns>PostalCodeTaxResponse</returns>
        public PostalCodeTaxResponse Map(List<PostalCodeTaxModel> postalCodeTaxModels)
        {
            try
            {
                if (postalCodeTaxModels == null || !postalCodeTaxModels.Any())
                    return null;

                var postalCodeTaxes = postalCodeTaxModels.Select(
                    s => new DTO.Postal_Code_Taxes.PostalCodeTax
                    {
                        PostalCode = s.PostalCode,
                        TaxCalculationType = s.TaxType?.Description,
                    }).ToList();

                return new PostalCodeTaxResponse
                {
                    PostalCodeTaxes = postalCodeTaxes,
                    Message = $"Successfully retrieved {postalCodeTaxes.Count()} postal codes with their corresponding tax calculation types."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying to map list of PostalCodeTaxModel to the PostalCodeTaxResponse object.");

                // rethrow to preserve stack details
                throw;
            }
        }
    }
}