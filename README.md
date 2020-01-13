# ZipPayUserService
----
## Description

This micro service is in charge of the creation and management of ZipPay users and their credit accounts.

----
## Getting Started

A docker-compose file is avaiable to build. Open you command line tool and type to launch the app:

    $ docker-compose up

You can launch it in detached mode if you want to keep control of your command line:

    $ docker-compose up -d


You can turn it down with the following:

    $ docker-compose down


The postgres database is hosted in AWS RDS and the app will connect automatically to it

----
## Available endpoints
### Users 
    GET api/users/
    GET api/users/list

    GET api/users/{id}

    POST api/users/create

### Accounts
    GET api/accounts/
    GET api/accounts/list

    GET api/accounts/{id}

    POST api/accounts/create

----
## Manual Tests with Postman
For ease of testing, a postman collection is available for import in folder "ManualTests". It contains all the endpoints in a collection with valid examples. 

To use it, open Postman and import the collection. Then update the localhost port based on how you launched the app (different ports from debug / docker).

If a problem with certificate occurs, you need to go to Postman *"File > Settings"* and untick *"SSL certificate verification"*. That should solve the problem.