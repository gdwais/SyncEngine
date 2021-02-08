# SyncEngine

SyncEngine is a microservice oriented platform for consuming and synchronising data from multiple datasources to multiple distributed systems.



## Migrations

This project leverages the dbrunner migration tool.

* create new migration
> dbrunner new-script -d <databasename> -n <nameofscript>

* run migrations
> dbrunner update -d <databasename>