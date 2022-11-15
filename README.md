# .NET 6 Web API

This is a boilerplate template for building / deploying a .NET Core Web API on Kubernetes.

## Versioning
| GitHub Release | .NET Core Version | Diagnostics HealthChecks Version |
|----------------|------------ |---------------------|
| main | 6.0.100-preview.6.21355.2 | 2.2.0 |

## Project Structure
```
├── appsettings.Development.json
├── appsettings.json
├── bin
├── Controllers
│   └── PassengerController.cs
├── C_Solution.csproj
├── Dockerfile
├── Dtos.cs
├── Extensions.cs
├── kubernetes
│   ├── mongodb.yaml
│   └── titanic.yaml
├── Models
│   └── Passenger.cs
├── obj
├── Program.cs
├── Properties
│   └── launchSettings.json
├── README.md
├── Repositories
│   ├── InMemPassengersRepository.cs
│   ├── IPassengersRepository.cs
│   └── MongoDbPassengersRepository.cs
└── Settings
    └── MongoDbSettings.cs
```

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
$ dotnet restore
```

## Deploying a .NET Core Web API microservice on Kubernetes

### Prerequisite:

- .NET Core Web API Docker Image

To deploy the app on Kubernetes, run following command:

```sh
$ kubectl apply -f kubernetes/titanic.yaml
$ kubectl apply -f kubernetes/mongodb.yaml
```

More instructions on that coming soon.

