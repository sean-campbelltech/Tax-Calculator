using System;
using Campbelltech.TaxCalculation.Domain.Data_Models;

namespace Campbelltech.TaxCalculation.Domain.Mapping
{
    internal class TaxCalculationModelBuilder
    {
        private readonly TaxCalculationModel _taxCalculationModel = new TaxCalculationModel();

        internal TaxCalculationModelBuilder()
        {
            _taxCalculationModel.CalculatedAt = DateTime.Now;
        }

        /// <summary>
        /// Adds the postal code to the TaxCalculationModel
        /// </summary>
        /// <param name="postalCode">Postal code of individual whose tax was calculated</param>
        /// <returns>TaxCalculationModelBuilder</returns>
        internal TaxCalculationModelBuilder AddPostalCode(string postalCode)
        {
            _taxCalculationModel.PostalCode = postalCode;

            return this;
        }

        /// <summary>
        /// Adds the annual income to the TaxCalculationModel
        /// </summary>
        /// <param name="annualIncome">Annual income of individual whose tax was calculated</param>
        /// <returns>TaxCalculationModelBuilder</returns>
        internal TaxCalculationModelBuilder AddAnnualIncome(decimal annualIncome)
        {
            _taxCalculationModel.AnnualIncome = annualIncome;

            return this;
        }

        /// <summary>
        /// Adds the tax amount that was calculated to the TaxCalculationModel
        /// </summary>
        /// <param name="taxAmount">The tax amount was calculated</param>
        /// <returns>TaxCalculationModelBuilder</returns>
        internal TaxCalculationModelBuilder AddTaxAmount(decimal taxAmount)
        {
            _taxCalculationModel.TaxAmount = taxAmount;

            return this;
        }

        /// <summary>
        /// Adds the username to the TaxCalculationModel
        /// </summary>
        /// <param name="requestedBy">Username of person who requested tax calculation</param>
        /// <returns>TaxCalculationModelBuilder</returns>
        internal TaxCalculationModelBuilder AddRequestedBy(string requestedBy)
        {
            _taxCalculationModel.RequestedBy = requestedBy;

            return this;
        }

        /// <summary>
        /// Returns the constructed TaxCalculationModel
        /// </summary>
        /// <returns></returns>
        internal TaxCalculationModel Build()
        {
            return _taxCalculationModel;
        }
    }
}