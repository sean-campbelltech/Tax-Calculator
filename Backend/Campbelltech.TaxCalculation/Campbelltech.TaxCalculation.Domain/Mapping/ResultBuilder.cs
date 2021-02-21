using System;
using System.Net;
using System.Text.RegularExpressions;
using Campbelltech.TaxCalculation.Domain.Types;
using Campbelltech.TaxCalculation.DTO.Responses;

namespace Campbelltech.TaxCalculation.Domain.Mapping
{
    internal class ResultBuilder
    {
        private readonly TaxCalculationResponse _response = new TaxCalculationResponse();
        private readonly HttpStatusCode _httpStatusCode;

        /// <summary>
        /// Constructor that takes in the tax calculation persisted result
        /// </summary>
        /// <param name="resultPersisted">Was tax calculation persisted to the database?</param>
        internal ResultBuilder(bool resultPersisted)
        {
            _httpStatusCode = resultPersisted ? HttpStatusCode.Created : HttpStatusCode.Accepted;
            _response.Message = resultPersisted
                    ? "Tax successfully calculated and persisted to the database"
                    : "Tax successfully calculated. However, the service was unable to persist the results to the database";
        }

        /// <summary>
        /// Adds the tax amount that was calculated to the TaxCalculationResponse object
        /// </summary>
        /// <param name="taxAmount">The tax amount was calculated</param>
        /// <returns>TaxCalculationModelBuilder</returns>
        internal ResultBuilder AddTaxAmount(decimal taxAmount)
        {
            _response.TaxAmount = Math.Round(taxAmount, 2);

            return this;
        }

        /// <summary>
        /// Adds the tax type that was used for the calculation to the TaxCalculationResponse object
        /// </summary>
        /// <param name="taxyType">The tax type that was used to calculate the tax amount</param>
        /// <returns>TaxCalculationModelBuilder</returns>
        internal ResultBuilder AddTaxType(TaxType taxType)
        {
            var taxTypeText = Regex.Replace(taxType.ToString(), "(\\B[A-Z])", " $1");
            _response.CalculationTypeUsed = taxTypeText;

            return this;
        }

        /// <summary>
        /// Build and return the service result Tuple
        /// </summary>
        /// <returns>HttpStatusCode + TaxCalculationResponse</returns>
        internal Tuple<HttpStatusCode, TaxCalculationResponse> Build()
        {
            return new Tuple<HttpStatusCode, TaxCalculationResponse>(_httpStatusCode, _response);
        }
    }
}