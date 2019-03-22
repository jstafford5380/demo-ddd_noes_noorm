Domain Driven Architecture (with Onions):

## Demo
#### The running process/host. In an API, this is the API project itself

### Responsisbilities:
- Configuration
- DI setup
- Translating application results to HTTP results (when implemented as an API)
- Getting commands and queries to the correct services or read models, respectively.

## Demo.Application
#### The application services layer

### Responsibilities:
- Application of use cases
- Is a technical layer
- Works with the domain but DOES NOT apply or enforce business rules
- Translating domain responses to application responses
- Replaces the "Interfaces" project
- Interacts with infrastructure through abstractions it defines (e.g. repositories, services bus, web services, etc.)
- This is the meat of the application. The controllers would delegate to this.

## Demo.Domain
#### The business domain

### Responsibilities:
- *Enforces* business rules -- it does this by allowing actions to be taken against entities and rejecting them if they violate a rule.
- When an action is "approved" then the state is updated, otherwise the state is left unchanged
- Is a non-technical layer
- Objects are unique to the context they serve, so you might have multiple "Person" objects. 
- Each aggregate serves a single context
- In this example, there is only one aggregate which is the "Person-Pet aggregate" serving the "Manage Pet" context.
- Objects are strong and strict. Weak types like int used as an id are replaced with more specific wrappers like `PetId` to remove abiguity and make semantics clear.

## Demo.Infrastructure.Data
#### The data implementation

### Responsibilities:
- Implements interfaces defined in the application layer. In this case, the person repository.


## Automapper
Normally, this pattern has the domain entities exposing and loading their state from `GetState()` and `LoadState(state)` methods respectively, but this is hidden through the use of Automapper. 

## Persisting State
The state objects should contain everything needed to restore the entity and allow state updates but no more. For example, "CreatedDate" might be a field in the database, but if I can't update that through some sort of business action and if that date isn't needed to enforce business rules, then it should be left out of the domain model.
