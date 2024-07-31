using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Areas.Identity.Data;
using Inventory.Models;

namespace Inventory.HostedServices
{
    public class OrderStatusUpdater : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OrderStatusUpdater> _logger;

        public OrderStatusUpdater(IServiceScopeFactory scopeFactory, ILogger<OrderStatusUpdater> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateOrderStatuses, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            _logger.LogInformation("OrderStatusUpdater started.");
            return Task.CompletedTask;
        }

        private void UpdateOrderStatuses(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AuthContext>();

                _logger.LogInformation("Updating order statuses.");

                // Process Pending to InProgress
                var pendingOrders = context.Orders.Where(o => o.Status == OrderStatus.Pending).ToList();
                foreach (var order in pendingOrders)
                {
                    order.Status = OrderStatus.InProgress;
                }

                // Process InProgress to Shipped
                var inProgressOrders = context.Orders.Where(o => o.Status == OrderStatus.InProgress).ToList();
                foreach (var order in inProgressOrders)
                {
                    order.Status = OrderStatus.Shipped;
                }

                // Process Shipped to Completed
                var shippedOrders = context.Orders.Where(o => o.Status == OrderStatus.Shipped).ToList();
                foreach (var order in shippedOrders)
                {
                    order.Status = OrderStatus.Completed;
                }

                context.SaveChanges();
                _logger.LogInformation("Order statuses updated.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _logger.LogInformation("OrderStatusUpdater stopped.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
