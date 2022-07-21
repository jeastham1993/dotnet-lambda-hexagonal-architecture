# .NET Lambda Hexagonal Architecture

Each Lambda function will have a single purpose. For example, when building an API each API endpoint will map to a separate function. This documentation talks through the problems with a [monolithic Lambda function.](https://docs.aws.amazon.com/lambda/latest/operatorguide/monolith.html)

Where possible, a hexagonal (ports and adapters) architecture pattern will be followed. The [hexagonal architecture pattern](https://alistair.cockburn.us/hexagonal-architecture/) proposes the division of a system into loosely coupled and interchangeable components. The application and business logic sit at the core of the application.

The next layer up is a set of interfaces that handle bidirectional communication from the core business logic layer. Implementation details are moved to the outside. The inputs (API controllers, UI, consoles, test scripts) and outputs (database implementations, message bus interactions) are at the perimeter.

The chosen compute layer becomes an implementation detail, not a core part of the system. It allows a cleaner process for migrating any integrations, from the frontend, to the compute layer and underlying database engine.

In practice, this means having a single class library containing all core business logic along with a separate library to implement any interfaces and integrations. Each individual Lambda function will then be implemented as a separate project. An example directory structure containing two Lambda functions, a Core library and an Integrations library can be seen below.

All application code will be implemented within the core library. This library will have no dependencies on other libraries. This allows business logic to be easily tested and ported to other compute options if requirements change in the future.

Any interface implementations will reside within an integrations library along with any start-up logic and dependency injection configuration. These implementations can then be shared across multiple functions within the microservice.

The Lambda function code will configure the dependency injection, parse the input event and hand any implementation logic to code in the core library. The function will not contain any business logic, instead acting as a layer to transform input event data to an object that the core library can understand and back again.
