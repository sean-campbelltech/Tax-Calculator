using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Campbelltech.TaxCalculator.UI.Clients;
using Campbelltech.TaxCalculator.UI.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Campbelltech.TaxCalculator.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IPostalCodeTaxClient _postalCodeTaxClient;

        public IndexModel(ILogger<IndexModel> logger, IPostalCodeTaxClient postalCodeTaxClient)
        {
            _logger = logger;
            _postalCodeTaxClient = postalCodeTaxClient;
        }

        [BindProperty]
        public string PostalCode { get; set; }

        [BindProperty]
        public decimal AnnualIncome { get; set; }

        [BindProperty]
        public List<PostalCodeTax.DTO.Postal_Code_Taxes.PostalCodeTax> PostalCodeTaxes { get; set; }

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
    }
}
