# SyncEngine

SyncEngine is a microservice oriented platform for consuming and synchronising data from multiple datasources to multiple distributed systems.

## Startup

SyncEngine.Api requires rabbitmq to be running before it can be started.  Docker compose is used to startup these ancillary services.

* to start docker containers navigate to root directory of the project
> docker-compose up (or docker-compose up -d to run silently)

* then start the api
> cd SyncEngine.Api && dotnet run

* then start the worker(s)
> cd SyncEngine.Worker && dotnet run

## RabbitMQ 

The system uses RabbitMQ to pass jobs between the services.  To view the rabbitmq service manager (running in a docker container) connect to http://localhost:15672/



## Migrations

This project leverages the dbrunner migration tool.

* create new migration
> dbrunner new-script -d <databasename> -n <nameofscript>

* run migrations
> dbrunner update -d <databasename>