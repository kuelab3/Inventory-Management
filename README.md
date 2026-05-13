# Inventory Management System - Event Driven ASP.NET Core MVC

This project implements the **Inventory Management** option from the assignment brief, including core entities **Products, Suppliers, and Stock Levels** and the required events **StockLevelLowEvent**, **ProductReceivedEvent**, and **SupplierOrderPlacedEvent**.

## Team

- Project title: Inventory Management System
- Group members:
**Ermiyas Befkadu DF/UR38353/16**
**Selam**

## Features

- ASP.NET Core MVC web application
- Event-driven architecture using DI-based event bus
- Immutable event payloads with C# `record`
- Async/await business flow
- File-backed persistence for products, transactions, and supplier orders
- Multiple handlers per event type
- Dedicated supplier orders page and event history page
- Audit and notification streams

## Architecture overview

The application uses a publisher/consumer pattern with dependency injection:

- `InventoryController` receives user commands
- `InventoryService` performs business logic and publishes domain events
- `EventBus` routes each event to all registered handlers
- Event handlers update state, create orders, and log notifications

### Event flow

InventoryController -> InventoryService -> EventBus -> event handlers

Implemented event chains:

- `ProductReceivedEvent`
  - `InventoryAuditHandler`
  - `ReceptionNotificationHandler`
- `StockLevelLowEvent`
  - `SupplyChainNotificationHandler`
  - `ReorderPlanningHandler`
- `SupplierOrderPlacedEvent`
  - `ProcurementLogHandler`
  - `SupplierEmailHandler`

## Persistence

The repository uses file persistence via `src/InventoryManagementSystem/Services/FileInventoryRepository.cs`.
Application state is stored in `Data/inventory-data.json` so product stock, transactions, and orders survive restarts.

## Run

```bash
dotnet restore
dotnet run --project src/InventoryManagementSystem
```

Then open the local address shown in the terminal.

## Useful pages

- Dashboard
- Products
- Suppliers
- Receive Stock
- Orders
- Event History

## Notes

This project is built as an event-driven coursework implementation with durable persistence, dedicated event history views, and improved handler robustness. Update the team member list before submission.

Not all parse errors were reported.  Correct the reported errors and try again.
    + CategoryInfo          : ParserError: (:) [], ParentContainsErrorRecordEx 
   ception
    + FullyQualifiedErrorId : MissingExpressionAfterOperator
 
