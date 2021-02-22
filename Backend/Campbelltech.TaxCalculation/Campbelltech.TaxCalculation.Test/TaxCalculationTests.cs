using System.Net;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Campbelltech.TaxCalculation.DTO.Requests;
using Campbelltech.TaxCalculation.DTO.Responses;
using Microsoft.Extensions.DependencyInjection;
using Campbelltech.TaxCalculation.Api.Controllers;
using Campbelltech.TaxCalculation.Domain.Services;
using Campbelltech.TaxCalculation.Domain.Repositories;
using Campbelltech.TaxCalculation.Domain.Calculations;
using Campbelltech.TaxCalculation.Domain.Configuration;

namespace Campbelltech.TaxCalculation.Test
{
    public class TaxCalculationTests
    {
        private CalculateTaxController _calculateTaxController;

        [SetUp]
        public void Setup()
        {
            var configDictionary = new Dictionary<string, string>
            {
                {"SqlConnectionString", "Server=localhost,1433;Database=Taxation;User Id=TaxUser;Password=TaxP@ss18013"},
                {"FlatValueTax:FlatValue", "10000.0"},
                {"FlatValueTax:MinAnnualIncome", "200000.0"},
                {"FlatValueTax:Rate", "5.0"},
                {"FlatRate", "17.5"}
            };

            var services = new ServiceCollection();
            services.AddLogging();

            IConfiguration configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(configDictionary)
                                .Build();

            services.Configure<Config>(configuration);
            services.AddScoped<IProgressiveTaxRateRepository, ProgressiveTaxRateRepository>();
            var taxCalculation = new TaxCalculationFactory().Create(services);
            services.AddTransient<ITaxCalculation>(x => taxCalculation);
            services.AddScoped<IPostalCodeTaxRepository, PostalCodeTaxRepository>();
            services.AddScoped<ITaxCalculationRepository, TaxCalculationRepository>();
            services.AddScoped<ITaxCalculationService, TaxCalculationService>();
            var serviceProvider = services.BuildServiceProvider();

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

            var successfulCalc = (result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.Created;

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
            ActionResult<TaxCalculationResponse> result = _calculateTaxController.PostAsync(new TaxCalculationRequest
            {
                PostalCode = postalCode,
                AnnualIncome = annualIncome
            }).GetAwaiter().GetResult();

            var taxTypeUsed = ((result.Result as ObjectResult).Value as TaxCalculationResponse)?.CalculationTypeUsed;

            Assert.AreEqual(taxTypeUsed, taxType);
        }

        /// <summary>
        /// Deliberately tries to fail validation by passing incorrect postalCode and annualIncome values
        /// </summary>
        /// <param name="postalCode">Postal code</param>
        /// <param name="annualIncome">Annual income</param>
        [TestCase("7441a", 1000000)]
        [TestCase("A10000", 900000)]
        [TestCase("70001111", 0)]
        [TestCase("100057911", 700000)]
        public void TaxCaluclationValidationFailureTest(string postalCode, decimal annualIncome)
        {
            var result = _calculateTaxController.PostAsync(new TaxCalculationRequest
            {
                PostalCode = postalCode,
                AnnualIncome = annualIncome
            }).GetAwaiter().GetResult();

            var expectedValidationFailure = (result.Result as ObjectResult).StatusCode == (int)HttpStatusCode.BadRequest;

            Assert.IsTrue(expectedValidationFailure);
        }
    }
}