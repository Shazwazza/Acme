# Acme Corporation


## Development 
If you experience any problems running this solution, please check the prerequisites.

### Prerequisites
#### .NET Core 2.1 SDK (v2.1.300)

Install [.NET Core 2.1 SDK](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.300)

#### Docker

Docker is used to run a local instance of Elasticsearch, Kibana and SQL Server 2017.

1. Install [Docker](https://download.docker.com/win/stable/Docker%20for%20Windows%20Installer.exe)
1. Ensure you're running Docker with Linux containers (This is the default behaviour when installing Docker).
1. Share your `C:/` drive with Docker by ticking the box in your Docker settings, which are accessible when you right click the Docker icon in your system tray.

More details on docker containers can be found in the readme-file in  [./scripts/Docker/](./scripts/Docker/)

## Execution / Run
1. Execute the  `docker-compose up` from powershell in the following folder: [./scripts/Docker/](./scripts/Docker/)
    - The docker container contains the following services
        - MsSQL (For serial numbers, submissions and users)
        - ElasicSearch (For logging)
        - Kibana (For log monitoring)
1. Build solution (using [Microsoft Visual Studio 2017](https://visualstudio.microsoft.com/downloads/) or [JetBrains Rider 2018.2](https://www.jetbrains.com/rider/download/#section=windows))
1. Run solution
1. Open browser on the following urls:
    - Acme Corporation [https://localhost:5001](https://localhost:5001) (Self signed Certificate)
    - Kibana log monitoring [http://localhost:5601](http://localhost:5601)
        - Setup yourself or import the [kibana settings](./scripts/Kibana/kibana-saved-objects.json)

## Architecture notes
- The solution is build using onion architecture
    - The domain is shared between layers, but should be as technology free as posible
    - The applicaion implements the business logic, but should be as technology free as posible
    - The infrastructure should implement the repositories. The infrasturcure is where all the technologies combines the solution
        - ORM - EntityFramework Core
        - Validation - FluentValidation
        - Dependency Injection - ASP.Net Core buildin IOC.