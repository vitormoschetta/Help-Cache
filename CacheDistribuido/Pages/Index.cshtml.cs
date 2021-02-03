﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CacheDistribuido.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDistributedCache _cache;

        public IndexModel(IDistributedCache cache)
        {
            _cache = cache;
        }

        public string CachedTimeUTC { get; set; }

        public async Task OnGet()
        {
            CachedTimeUTC = "Tempo de cache expirou";
            var encodedCachedTimeUTC = await _cache.GetAsync("cachedTimeUTC");

            if (encodedCachedTimeUTC != null)            
                CachedTimeUTC = Encoding.UTF8.GetString(encodedCachedTimeUTC);            
        }

        public async Task<IActionResult> OnPostResetCachedTime()
        {
            var currentTimeUTC = DateTime.UtcNow.ToString();
            byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);
            var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20));
            await _cache.SetAsync("cachedTimeUTC", encodedCurrentTimeUTC, options);

            return RedirectToPage();
        }
    }
}
