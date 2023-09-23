# ManageMySpace

Manage My Space is a simple web service that allows students to easily explore and book rooms in their universities.

Students can see a list of registered rooms in their university with such details as room size, equipment, etc.

Students can book particular rooms - and create appropriate events.

## Architecture

Manage My Space is built with a classic layered architecture.

API Level:
On this level, we have defined controllers and contract models (requests and responses).

BLL Level:
On this level, we define services that encapsulate all business logic.

DAL Level:
It's a place where we interact with the data.

## Tech stack

1. .net core 3.1
2. Entity Framework Core
3. JWT for authorization
4. Docker
5. MS SQL
6. Yaml
7. Elastic Search
