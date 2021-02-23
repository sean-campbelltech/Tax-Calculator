# Tax-Calculator

A Tax Calculator that is based on ASP.NET Core MVC (Front-End) and ASP.NET Core REST APIs (Back-End) 

## Context Diagram

![alt text](https://raw.githubusercontent.com/sean-campbelltech/Tax-Calculator/main/Diagrams/Tax-Calculator%20Context%20Diagram.png "Context Diagram")

## Environment Setup

**1. Create Attachable Docker Network**

If you plan to run the Tax-Calculator solutions in Docker, you need to create an attachable Docker network by running the following commnads:

*Note: If you are using a Linux OS, always start your Docker commands with sudo.*

```
docker network create --attachable -d bridge taxCalculatorNet
```

Then run the following command to list all you Docker networks to see if the newly created `taxCalculatorNet` network has been created.

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

- Factory Method

  - A transient factory method was used to abstract the tax calculation logic. More specifically, the factory uses the tax type to determine which implementation of ITaxCalculation it should instantiate. 
  - This allows us to inject a factory resolver Func using dependency injection in the client logic to resolve the ITaxCalculation implementation.
  - The transient factory can be found under [Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Calculations/](https://github.com/sean-campbelltech/Tax-Calculator/blob/main/Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Calculations/TaxCalculationFactory.cs)

- Builder

  - A fluent builder was used segregate the construction of the TaxCalculationModel from its representation. This is found under [Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Mapping/TaxCalculationModelBuilder.cs](https://github.com/sean-campbelltech/Tax-Calculator/blob/main/Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Mapping/TaxCalculationModelBuilder.cs)
  - Another fluent builder was used to map the result Tuple (HTTP status code + TaxCalculationResponse) that is returned to controller method after the main tax calculations have been processed. This is found under [Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Mapping/ResultBuilder.cs ](https://github.com/sean-campbelltech/Tax-Calculator/blob/main/Backend/Campbelltech.TaxCalculation/Campbelltech.TaxCalculation.Domain/Mapping/ResultBuilder.cs)

- Repository Pattern

  - Describe where and why repository pattern was used.

- API Gateway Pattern

  - Describe where and why API gateway pattern was used.

## Compliance to SOLID Principles

- S: Single Responsibility Priniciple (SRP)

  - Describe how the code complies to SRP.

- O: Open-Closed Priniciple (ORP)

  - Describe how the code complies to ORP.

- L: Liskov Substitution Priniciple (LSP)

  - Describe how the code complies to LSP.
  
- I: Interface Segregation Priniciple (ISP)

  - Describe how the code complies to ISP.

- D: Dependency Inversion Priniciple (DIP)

  - Describe how the code complies to DIP.

## Design Descriptions

-- Add design descriptions and diagrams