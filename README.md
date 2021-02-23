# Tax-Calculator

A Tax Calculator that is based on ASP.NET Core MVC (Frontend) and ASP.NET Core REST APIs (Backend) 

## Context Diagram

![alt text](https://raw.githubusercontent.com/sean-campbelltech/Tax-Calculator/main/Diagrams/Tax-Calculator%20Context%20Diagram.png "Context Diagram")

## Environment Setup

**1. Create Attachable Docker Network**

If you plan to run the Tax-Calculator solutions in Docker, you need to create an attachable Docker network. This will allow containers within the same network to be albe to communicate with each other. You can create such a network by running the following commnads:

*Note: If you are using a Linux OS, always start your Docker commands with sudo.*

```
docker network create --attachable -d bridge taxCalculatorNet
```

Then run the following command to list all your Docker networks to see if the newly created `taxCalculatorNet` network has been created.

```
docker network ls
```

**2. Install Microsoft SQL Server**

If you already have a Microsoft SQL server instance running on a staging server or in Docker, you can skip this step. However, if you plan to connect from your Docker containers to a local Microsoft SQL server instance, it is advised that you rather run a new instance of Microsoft SQL server in Docker, because from the containers perspective, localhost means local to the container and not local to the host.

Execute the following command to run Microsoft SQL Server Express in Docker:

```
docker run --name sql-container \
--network taxCalculatorNet \
--restart always \
-e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Eph2:8-9!' -e 'MSSQL_PID=Express' \
-p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu 
```

**3. Running SQl Scripts**

Run following SQL scripts from your client tools (SSMS or Azure Data Studio or VS Code SQL Server (mssql) plugin) to create the `Taxation` database, the `TaxUser` user, the required tables and insert some needed data:

[SQL Scripts](https://github.com/sean-campbelltech/Tax-Calculator/blob/main/SQL%20Scripts/SQL%20Scripts.sql)


## Running the Applications

**1. Running the Applications in Docker (Preferred):**

You can run all the applications by copying the [docker-compose.yml](https://github.com/sean-campbelltech/Tax-Calculator/blob/main/Docker/docker-compose.yml) into a directory and executing the following command from that directory:

```
docker-compose up -d
```

If you do not have Docker Compose installed, please follow the steps in the following link:

[https://docs.docker.com/compose/install/](https://docs.docker.com/compose/install/)

**2. Running the Applications in VS Code:**

If you choose to run the applications from your local machine in VS Code, you will need to open and run all of the following workspaces/solutions:

**Backend APIs / Microservices:**

- [API Gateway](https://github.com/sean-campbelltech/Tax-Calculator/tree/main/Backend/Campbelltech.ApiGateway)
- [PostalCodeTax API](https://github.com/sean-campbelltech/Tax-Calculator/tree/main/Backend/Campbelltech.PostalCodeTax)
- [TaxCalculator API](https://github.com/sean-campbelltech/Tax-Calculator/tree/main/Backend/Campbelltech.TaxCalculation)

**Frontend:**

- [Tax Calculator](https://github.com/sean-campbelltech/Tax-Calculator/tree/main/Frontend/Campbelltech.TaxCalculator)

## Design Patterns Used

- **Factory**

  - A transient factory was used to abstract the tax calculation logic. More specifically, the factory uses the tax type to determine which implementation of ITaxCalculation it should instantiate: ProgressiveTaxCalculation, FlatValueTaxCalculation or FlatRateTaxCalculation.
  - This allows us to inject a factory resolver Func using dependency injection in the client constructor to resolve the ITaxCalculation implementation.
  - The transient factory can be found [here](https://github.com/sean-campbelltech/Tax-Calculator/blob/main/Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Calculations/TaxCalculationFactory.cs)

- **Builder**

  - A fluent builder was used segregate the construction of the TaxCalculationModel from its representation. The TaxCalculationModelBuilder can be found [here](https://github.com/sean-campbelltech/Tax-Calculator/blob/main/Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Mapping/TaxCalculationModelBuilder.cs)
  - Another fluent builder was used to map the result Tuple (HTTP status code + TaxCalculationResponse) that is returned to controller method after the main tax calculations have been processed. The ResultBuilder can be found [here](https://github.com/sean-campbelltech/Tax-Calculator/blob/main/Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Mapping/ResultBuilder.cs)

- **Repository Pattern**

  - The repository pattern was used in both APIs to abstract the data persistance layer. In this way, the main service logic is agnostic to the type of database, and segregated from the data access layer. 
  - The repositories for the TaxCalculation API can be found [here](https://github.com/sean-campbelltech/Tax-Calculator/tree/main/Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Repositories).
  - The repositories for the PostalCodeTax API can be found [here](https://github.com/sean-campbelltech/Tax-Calculator/tree/main/Backend/Campbelltech.PostalCodeTax/Campbelltech.PostalCodeTax.Domain/Repositories).

- **API Gateway Pattern**

  - The API gatway pattern is used to provide the TaxCalculator frontend with a unfied entry point to the backend APIs. In this way, the frontend need not know the addresses or ports of each of the backend APIs. The API gateway will route each of the HTTP requests to the desired APIs.

## Compliance to SOLID Principles

The aim of SOLID principles is to ensure that your system has low coupling and high cohesion to ensure that systems are highly extensible and easy to maintain. This section decribes how the frontend and backend solutions comply to the SOLID principles.

- **S: Single Responsibility Priniciple (SRP)**

  - The Single Responsibility Priniciple states that a class should have only a single reason to change. Thus, it should only have one responsibility. This principle was rigourously applied by both the backend and frontend solutions. Single responsibility is achieved in the backend APIs in the following way:
    - **Project Structure**: Each API solution consists of 4 projects: 
      - **The API project**: Represents the RESTful HTTP interface and contains nearly no logic other than calling the main service implementation.
      - **The DTO project**: Contains the request and response data transfer objects only.
      - **The Domain project**: This is the project that contains the domain / business logic and data access logic.
      - **The Test project**: Contains the unit tests.

     - **Domain logic implemenation**: Domain logic is segregated into the following segments:
       - **Services**: Contains the main business logic of the API. It is the layer between the controller methods and the repositories.
       - **Repositories**: Abstracts the data layer from the service and controller methods.
       - **Mapping**: Allows us to segregate the mapping logic away from the main service or aggregation logic towards complying to the SRP.
      
- **O: Open-Closed Priniciple (ORP)**

  - The open-closed principle states that a class should be open for extension and closed for modification.
  - Each class of the backend and frontend projects implements an interface. This allows for ease of extension, where:
    - You can create a second implementation of an interface and use a factory method to instantiate the correct implementation based on a desired input / key. 
    - You can inherit from an existing class that already implements an interface and override a method and extend the logic of the existing class using polymorphism.
  
- **L: Liskov Substitution Priniciple (LSP)**

  - The liskov substitution principle was not used in this solution, but it states that you should be able to substitute a class (concretion) with a base class (abstraction) without affecting the correctness of the program. 
  - Often when I implement a v2 of a controller method, I would inherit from the existing request and response DTOs and add the additional fields that a client requires. Then, instead of adding another method to a mapping interface, I would keep the original that returns the old response DTO (in a response mapper for example) which now has become the base class of the new response DTO. I would then create another mapper class that inherits from the same interface, and then in the new interface implementation I would simply instantiate a new instance of the new derrived class, map it, and assign it to the base class. (e.g. `BaseReponse response = new DerrivedResponse() { }`). This allows compliance to the Liskov Substitution Priniciple.
   
- **I: Interface Segregation Priniciple (ISP)**

  - Inteface segregation states that a client should not be forced to implement an interface that it doesn't use, and that we should create fine grained interfaces that are client specific. 
  - This is dillegenty applied accross all solutions. For example, there are 3 different repository interfaces in the TaxCalculation API, each with their own implementations. Now, I could have created a single interface with methods that are defined for all repository logic. However, one of the repositories are used to retrieve the progressive tax rates. Now, this is only applicable for the ProgressiveTaxCalculation and not for the other calculation types, nor is it needed from the main TaxCalculationService. Therefore, interface segregation was applied to ensure that client logic were not forced to implement methods it don't need.

- **D: Dependency Inversion Priniciple (DIP)**

  - Dependency inversion states that our code should depend on abstractions and not concretions. 
  - All client logic is coded to the interface rather than instantiating classes. (With the exception of mapping / builder logic). Each interface implementation is injected via dependency injection into the constructor of the client classes. This is done via the .NET Core service registration where services are registered as either:
    - **Scoped**: A new instance is created of an interface implementation for every unique HTTP request. In other words, one instance per HTTP request.
    - **Transient**: A new instance is created everytime it is used. Therefore, if used multiple times in a single HTTP requets, an instance will be created everytime.
    - **Singleton**: A single instance will be created for the life of the API / container.

