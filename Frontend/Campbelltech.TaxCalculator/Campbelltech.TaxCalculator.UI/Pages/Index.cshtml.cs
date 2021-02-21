using System;
using System.Collections.Generic;
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
        public List<SelectListItem> PostalCodesList { get; set; }

        [BindProperty]
        public List<PostalCodeTax.DTO.Postal_Code_Taxes.PostalCodeTax> PostalCodeTaxes { get; set; }

        [BindProperty]
        public decimal TaxAmount { get; set; }

        public async Task OnGet()
        {
            try
            {
                var response = await _postalCodeTaxClient.GetAsync();
                PostalCodeTaxes = response?.PostalCodeTaxes;
            }
            catch (Exception ex)
            {
                RedirectToPage("Error", new { msg = ex.Message });
            }
        }

        public async Task OnPost()
        {
            try
            {
                var response = await _taxCalculationClient.CalculateAsync(PostalCode, AnnualIncome);

                TaxAmount = response.TaxAmount;
            }
            catch (Exception ex)
            {
                RedirectToPage("Error", new { msg = ex.Message });
            }
        }
    }
}
