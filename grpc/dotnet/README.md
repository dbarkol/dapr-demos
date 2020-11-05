# gRPC demo - .NET

Run the gRPC service

``` bash
dapr run --app-id grpc-loyalty-service --app-port 5000 --app-protocol grpc --dapr-grpc-port 3500 dotnet run
```

Run the client application

``` bash
dapr run --app-id loyalty-client dotnet run
```
