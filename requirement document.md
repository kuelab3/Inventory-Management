 PDF To Markdown Converter
Debug View
Result View
💻 Event-Driven Programming Project: Simple C#
💻 Event-Driven Programming Project:
Simple C# .NET Web Systems
Course: Event-Driven Programming (C# / .NET)
Objective: To implement a simple web application using C# and ASP.NET Core that
demonstrates mastery of the Event-Driven Architecture (EDA) pattern, emphasizing
decoupling, asynchronous processing, and best practices.

I. 💡 Project Selection (Choose One)
Your team must select one common management system. The focus is on the backend logic
and its event-driven implementation, not on complex user interface design.
Project Title Core Entities
(Data)
Required Events to Implement
(Minimum 3)



1. Inventory
Management
Products,
Suppliers, Stock
Levels
StockLevelLowEvent,
ProductReceivedEvent,
SupplierOrderPlacedEvent


II. 🛠 Event-Driven Architecture (EDA) Requirements
Your application's core business logic must utilize the Event-Driven pattern to ensure
components remain decoupled.

A. Architecture Overview
● Event Publisher (Producer): A service or controller that initiates a business change
and publishes an event (e.g., RegistrationService).
● Event Consumer (Handler): Decoupled components that subscribe to and process
specific events. You must have at least two different handlers react to the same
event.
● Event Bus/Aggregator: The central mechanism that routes events from Publishers to
all registered Consumers. This should be implemented using either:
○ C# Delegates and the event keyword.
○ A Dependency Injection (DI) based service that discovers implementations of
IEventHandler<TEvent>.
B. C# and .NET Best Practices
● Platform: ASP.NET Core MVC
● Decoupling: Use Interfaces for all services (e.g., IUserService) and handlers.
● Asynchronicity: Implement logic using the async/await pattern for non-blocking
operation.
● Immutability: Use C# records or classes with { get; init; } properties for event payloads
to ensure state cannot change during propagation.
III. ⚙ Step-by-Step Project Implementation
Phase 1: Setup and Planning
Form Groups: Form a group of not more than seven (7) students.
Project Selection: Select one project from the list in Section I.
Define Events: Decide on the three minimum events and identify which components will
handle them.
GitLab/Github Account: Ensure all members have a personal GitLab/Github account.
Submission 1 (Planning): Submit your Project Title, Group Member List, and the three
planned Events to email tadegewk@gmail.com.
Phase 2: Technical Setup and Collaboration
Repository Creation: One group member creates a Private Project on GitLab.
Add Team Members: Add all team members as Maintainers.
Add Instructor: Add the instructor (tadegewk@gmail.com) as a Reporter.
Local Setup: All team members clone the repository locally using Git.
Initialize Project: Create a new ASP.NET Core solution and commit the structure to the
main branch.
Branching: Create a feature branch for every task (e.g., git checkout -b
feature/event-bus-setup). Never commit directly to main.
Phase 3: Core EDA Implementation
Define Event Base: Create a base class or interface (e.g., IEvent) for all events.
Implement Event Bus: Create the EventBus service. This service must use the DI
container to find and trigger handlers.
Define Event Handlers: Create a generic interface: IEventHandler where
TEvent : IEvent.
Implement Services: Create business services (e.g., IPayrollService) that call the Event
Bus to publish events.
Implement Logic: * Publisher: Call _eventBus.Publish(myEvent) inside your service.
○ Consumers: Implement at least two concrete handler classes (e.g.,
EmailHandler, LogHandler) per event using async Task Handle(TEvent e).
Phase 4: Review and Finalization
Peer Review: Use Merge Requests (MR) on GitLab. At least one peer must review and
approve code before it merges into main.
Documentation: Create a README.md including:
○ Project Title and Group Members.
○ Setup/Run instructions.
○ A flow diagram of your events (Publisher → Event → Consumers).
Submission 2 (Final): Ensure the main branch is stable and complete.
IV. 📅 Deliverables and Grading
Deliverable Requirement
Initial Submission Project Title, Description, and three Events defined.
Mid-Project Check GitLab setup verified, active feature branches, and Event Bus
implementation.
Final Project Functional app with 3+ events and multiple consumers per
event.
Documentation A complete, informative README.md file in the repository.
Presentation Demonstration of the app and explanation of the EDA
components.
This is a offline tool, your data stays locally and is not send to any server!
Feedback & Bug Reports