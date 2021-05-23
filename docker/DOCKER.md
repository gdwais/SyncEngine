# Docker Overview

Just some quick notes to make working with Docker easier here

# syncengine-db 
to login to container
> docker exec -it syncengine-db bash

to access 
> /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P '<PASSWORD>' 

volume persistent in docker volume container `sqlvolume`

# rabbit-mq notes

Once running the RabbitMQ management website can be found at http:/localhost:15672

* rabbit mq configuration can be found at /rabbitmq/etc/rabbitmq.conf
* preset definitions can be found at /rabbitmq/data/definitions.json