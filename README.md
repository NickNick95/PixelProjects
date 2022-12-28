For this task, I use two services: PixelService and StoreService. 
For communications uses RabbitMQ.

You can install RabbitMQ from the website: https://www.rabbitmq.com/download.html
Also, you can use docker-compose. Opens terminal in a folder and run docker-compose up -d

As the result, you can run projects in VisualStudio.

Moreover, you can run apps using only docker files.
To start with updating appsettings.json in projects.

After that run command in the terminal where located docker file.
PixelService: docker build -t pixelservice .
StoreService: docker build -t storeservice .

Second, you have to create your network because docker-compose creates a private network by default.
Link: https://docs.docker.com/engine/reference/commandline/network_create/

In the end you need run docker containers 
PixelService: docker run -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS=http://+:90 -p 9090:90 pixelservice:latest
StoreService: docker run -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS=http://+:80 -p 8080:80 storeservice:latest

By the way don't forget to add image to your network:
docker network connect [network name] [docker image id]

