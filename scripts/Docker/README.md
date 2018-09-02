# Docker

## Starting Docker Containers
1. Open PowerShell and type `cd D:/projects/Hesehus.ProductSearch/scripts/Docker`
2. Type `docker-compose up`
   - After a few seconds your Docker containers will be up and running, and you are ready to get started.

Type `docker-compose --help` for further commands i.e. to stop containers again.

## Default
`docker-compose up`
  * ES (1 GiB) - [http://localhost:9200](http://localhost:9200)
  * Kibana - [http://localhost:5601](http://localhost:5601)
  * SQL Server 2017 - [http://localhost:1433](http://localhost:1433)
   
## Docker Storage
The Docker containers use persistent storage by mounting the following paths:

1. Elasticsearch
   - `C:/docker/hesehusproductsearch/elasticsearch/data`
2. SQL Server 2017   
   - `C:/docker/hesehusproductsearch/mssql/data`