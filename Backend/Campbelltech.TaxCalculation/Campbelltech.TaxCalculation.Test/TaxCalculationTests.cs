using System.Net;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Campbelltech.TaxCalculation.DTO.Requests;
using Microsoft.Extensions.DependencyInjection;
using Campbelltech.TaxCalculation.Api.Controllers;
using Campbelltech.TaxCalculation.Domain.Services;

namespace Campbelltech.TaxCalculation.Test
{
    public class TaxCalculationTests
    {
        private CalculateTaxController _calculateTaxController;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var taxCalculationService = serviceProvider.GetService<ITaxCalculationService>();
            _calculateTaxController = new CalculateTaxController(taxCalculationService);
        }

        /// <summary>
        /// Tests for positive results from the TaxCalculation API
        /// </summary>
        /// <param name="postalCode">Postal code</param>
        /// <param name="annualIncome">Annual income</param>
        [TestCase("7441", 1000000)]
        [TestCase("A100", 900000)]
        [TestCase("7000", 800000)]
        [TestCase("1000", 700000)]
        public void TaxCaluclationPositiveTest(string postalCode, decimal annualIncome)
        {
            var result = _calculateTaxController.PostAsync(new TaxCalculationRequest
            {
                PostalCode = postalCode,
                AnnualIncome = annualIncome
            }).GetAwaiter().GetResult();

            var successfulCalc = (result.Result as StatusCodeResult).StatusCode == (int)HttpStatusCode.Created;

            Assert.IsTrue(successfulCalc);
        }

        /// <summary>
        /// Tests for positive results from the TaxCalculation API
        /// </summary>
        /// <param name="postalCode">Postal code</param>
        /// <param name="annualIncome">Annual income</param>
        [TestCase("7441", 1000000, "Progressive")]
        [TestCase("A100", 900000, "Flat Value")]
        [TestCase("7000", 800000, "Flat Rate")]
        [TestCase("1000", 700000, "Progressive")]
        public void TaxCaluclationTypeTest(string postalCode, decimal annualIncome, string taxType)
        {
            var result = _calculateTaxController.PostAsync(new TaxCalculationRequest
            {
                PostalCode = postalCode,
                AnnualIncome = annualIncome
            }).GetAwaiter().GetResult();

            var taxTypeUsed = result?.Value?.CalculationTypeUsed;

            Assert.AreSame(taxTypeUsed, taxType);
        }

        /// <summary>
        /// Deliberately tries to fail validation by passing incorrect postalCode and annualIncome values
        /// </summary>
        /// <param name="postalCode">Postal code</param>
        /// <param name="annualIncome">Annual income</param>
        [TestCase("7441a", 1000000)]
        [TestCase("A100", -900000)]
        [TestCase("7000", 0)]
        [TestCase("100057911", 700000)]
        public void TaxCaluclationValidationFailureTest(string postalCode, decimal annualIncome)
        {
            var result = _calculateTaxController.PostAsync(new TaxCalculationRequest
            {
                PostalCode = postalCode,
                AnnualIncome = annualIncome
            }).GetAwaiter().GetResult();

            var expectedValidationFailure = (result.Result as StatusCodeResult).StatusCode == (int)HttpStatusCode.BadGateway;

            Assert.IsTrue(expectedValidationFailure);
        }
    }
}