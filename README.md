# .NET 6 Web API

This is a boilerplate template for building / deploying a .NET Core Web API on Kubernetes.

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

- The `docker-compose.yml` is for building both db and api.
- `Dockerfile` is .NET Core Web API Multistage Dockerfile (following Docker Best Practices)
- `appsettings.Development.json` is .NET Core Web API development environment config
- `kubenertes` folder will contain Kubernetes yaml files (deployment, statefulstes, services)
- `Program.cs` is .NET Core Web API environment variable mapping config and .NET Core Web API startup & path routing config 

## Setting Up

To setup this project, you need to clone the git repo

```sh
$ git clone https://github.com/cheloghm/C_Solutions.git
$ cd C_Solution
```

followed by

```sh
$ docker-compose up -d
```

## Running the database in dev enviromnent.
```sh
$ docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=pass1234 --network=titanic mongo
```
Running the Titanic app in dev environment.
docker run -it --rm -p 8282:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=pass1234 --network=titanic cheloghm/titanic:v9

To run in a K8s cluster
Ensure you create the secret: kubectl create secret generic titanic-secrets --from-literal=mongodb-password='pass1234'

## Checking the Readiness and Liveness of the API.
To check if the API is live
url/healt/live
To check is the database is ready to serve requests
url/healt/ready

## Deploying a .NET Core Web API microservice on Kubernetes

### Prerequisite:

- .NET Core Web API Docker Image

To deploy the app on Kubernetes, run following command:

```sh
$ cd Titanic.Api/kubernetes/
$ kubectl apply -f kubernetes/titanic.yaml
$ kubectl apply -f kubernetes/mongodb.yaml
```

More information on that coming soon.

