using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Campbelltech.TaxCalculator.UI.Clients;
using Campbelltech.TaxCalculator.UI.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Campbelltech.TaxCalculator.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IPostalCodeTaxClient _postalCodeTaxClient;
        private readonly ITaxCalculationClient _taxCalculationClient;

        public IndexModel(
            ILogger<IndexModel> logger,
            IPostalCodeTaxClient postalCodeTaxClient,
            ITaxCalculationClient taxCalculationClien)
        {
            _logger = logger;
            _postalCodeTaxClient = postalCodeTaxClient;
            _taxCalculationClient = taxCalculationClien;
        }

        [BindProperty]
        public string PostalCode { get; set; }

        [BindProperty]
        public decimal AnnualIncome { get; set; }

        [BindProperty]
        public List<string> PostalCodesList { get; set; }

        [BindProperty]
        public decimal TaxAmount { get; set; }

        [BindProperty]
        public string TaxCalculationTypeUsed { get; set; }

        public async Task<IActionResult> OnGet(decimal annualIncome = 0m, decimal taxAmount = 0m, string taxCalculationTypeUsed = "")
        {
            try
            {
                this.TaxAmount = taxAmount;
                this.AnnualIncome = annualIncome;
                this.TaxCalculationTypeUsed = taxCalculationTypeUsed;

                var response = await _postalCodeTaxClient.GetAsync();
                PostalCodesList = response?.PostalCodeTaxes?.Select(s => s.PostalCode)?.ToList();

                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { msg = ex.Message });
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var response = await _taxCalculationClient.CalculateAsync(PostalCode, AnnualIncome);

                return RedirectToAction("OnGet", "Index", new
                {
                    annualIncome = AnnualIncome,
                    taxAmount = response.TaxAmount,
                    TaxCalculationTypeUsed = response.CalculationTypeUsed
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { msg = ex.Message });
            }
        }
    }
}
