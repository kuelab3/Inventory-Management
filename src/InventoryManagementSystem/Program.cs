using InventoryManagementSystem.EventBus;
using InventoryManagementSystem.Handlers.ProductReceived;
using InventoryManagementSystem.Handlers.StockLevelLow;
using InventoryManagementSystem.Handlers.SupplierOrderPlaced;
using InventoryManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEventBus, EventBus>();
builder.Services.AddSingleton<IInventoryRepository, InMemoryInventoryRepository>();
builder.Services.AddSingleton<IAuditLogService, AuditLogService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddScoped<IEventHandler<InventoryManagementSystem.Events.ProductReceivedEvent>, InventoryAuditHandler>();
builder.Services.AddScoped<IEventHandler<InventoryManagementSystem.Events.ProductReceivedEvent>, ReceptionNotificationHandler>();

builder.Services.AddScoped<IEventHandler<InventoryManagementSystem.Events.StockLevelLowEvent>, SupplyChainNotificationHandler>();
builder.Services.AddScoped<IEventHandler<InventoryManagementSystem.Events.StockLevelLowEvent>, ReorderPlanningHandler>();

builder.Services.AddScoped<IEventHandler<InventoryManagementSystem.Events.SupplierOrderPlacedEvent>, ProcurementLogHandler>();
builder.Services.AddScoped<IEventHandler<InventoryManagementSystem.Events.SupplierOrderPlacedEvent>, SupplierEmailHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inventory}/{action=Index}/{id?}");

app.Run();
