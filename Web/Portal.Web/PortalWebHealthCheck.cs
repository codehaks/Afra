using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Web
{
    public class PortalWebHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var client = new HttpClient();


            var healthCheckResultHealthy = true;

            var vegaHealth = await client.GetStringAsync("https://localhost:5005/health");
            if (vegaHealth != "healthy")
            {
                healthCheckResultHealthy = false;
            }

            //if (await client.GetStringAsync("https://localhost:5007") != "healthy")
            //{
            //    healthCheckResultHealthy = false;
            //}

            if (healthCheckResultHealthy)
            {
                return
                    HealthCheckResult.Healthy("healthy1");
            }

            return
                HealthCheckResult.Unhealthy("unhealthy1");
        }
    }
}
