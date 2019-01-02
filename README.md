## Extract the zip to http-sample folder

## Web Service

- content service: http://localhost:9595/content/{name}


### To run the services
Open the MyWebApplication.sln in Visual Studio 
docker-compose will be set as the startup project
Press F5

### Swagger
Open http://localhost:9595/swagger/index.html

### Description
Methods supported:
 
POST /add

GET /hello/{name}
where response is customised based on content service response

available names: id1, id2