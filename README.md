# .NET 6 Web API

This is a .Net 6 web API app. With a pre-populated MongoDB database with the details of the Titanic passengers.
The Api endpoints are:
To get all: https://localhost:8282/passengers/all 
To get by ID: https://localhost:8282/passengers/{Id}
To search by characters or words: https://localhost:8282/passengers/search
To edit or update: https://localhost:8282/passengers/{Id}
To delete: https://localhost:8282/passengers/{id}
Note! the above endpoints are for those running the application using the docker-compose file.

## Versioning
| GitHub Release | .NET Core Version | Diagnostics HealthChecks Version |
|----------------|------------ |---------------------|
| main | 6.0.100-preview.6.21355.2 | 2.2.0 |

## Project Structure
```
.
├── build.proj
├── docker-compose.debug.yml
├── docker-compose.yml
├── obj
├── README.md
├── Titanic.Api
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── bin
│   ├── Controllers
│   │   └── PassengerController.cs
│   ├── Dockerfile
│   ├── Dtos.cs
│   ├── Extensions.cs
│   ├── kubernetes
│   │   ├── mongodb.yaml
│   │   └── titanic.yaml
│   ├── Models
│   │   └── Passenger.cs
│   ├── obj
│   ├── Program.cs
│   ├── Properties
│   │   └── launchSettings.json
│   ├── Repositories
│   │   ├── InMemPassengersRepository.cs
│   │   ├── IPassengersRepository.cs
│   │   └── MongoDbPassengersRepository.cs
│   ├── Settings
│   │   └── MongoDbSettings.cs
│   └── Titanic.Api.csproj
├── titanic.csv
└── Titanic.UnitTests
    ├── bin
    ├── obj
    ├── PassengerControllerTests.cs
    ├── Titanic.UnitTests.csproj
    └── Usings.cs
```

## Deploying a .NET Core Web API microservice on Kubernetes

### Prerequisite:

- Docker
- .NET Core 6


## to run in production
```sh
$ cd Titanic.Api
$ docker-compose up -d
```

## Checking the Readiness and Liveness of the App.
###To check if the API is live
    ```sh
    {url/endpoint}/healt/live
    ```
###To check is the database is ready to serve requests
    ```sh
    {url/endpoint}/healt/ready
    ```

- The `docker-compose.yml` is for building both db and api.
- `Dockerfile` is .NET Core Web API Multistage Dockerfile (following Docker Best Practices)
- `appsettings.Development.json` is .NET Core Web API development environment config
- `kubenertes` folder contains Kubernetes yaml files (deployment, statefulstes, services) the webapi image cheloghm/titanic:v26 is the latest and best version
               while the database image cheloghm/mongo:v4 is a seeded/pre-populated database and the best version. Both images are already implemented in their                respective yml files is the Kubernets directory. Just run the comand:
               ```sh
               $ kubectl apply -f kubernetes/mongodb.yaml
               $ kubectl apply -f kubernetes/mongodb.yaml
               ```
               In your cluster to spin up the service/app.
- `Program.cs` is .NET Core Web API environment variable mapping config and .NET Core Web API startup & path routing config 

## Setting Up

To setup this project, you need to clone the git repo

```sh
$ git clone https://github.com/cheloghm/C_Solutions.git
```

    ### To run in Dev environment 
        ```sh
        $ dotnet run
        ```
        OR
        ```sh
            docker run -it --rm -p 8282:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=pass1234 --network=titanic cheloghm/titanic:v26
```
        And run the database
        ```sh
            $ docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e                MONGO_INITDB_ROOT_PASSWORD=pass1234 --network=titanic cheloghm/mondo:v4
```

    ### To run UnitTests
        ```sh
        $ dotnet test 
        ```

To run in a K8s cluster
Ensure you create the secret: kubectl create secret generic titanic-secrets --from-literal=mongodb-password='pass1234'

More information on that coming soon.

