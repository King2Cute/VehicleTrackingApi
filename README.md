Vehicle Tracking API
========
A Vehicle Tracking API able to track GPS navigation positions with devices emboarded into vehicles.

Authentication is retrieved using JWT tokens and password are stored in the database using Sha256 with salt
Authorizaton is done by Role based claims stored in the JWT tokens
Storage is done by local MongoDB server storage running on localhost:27017

How to run solution
------------
1. Install [MongoDB](https://www.mongodb.com/try/download/community) onto your machine make sure port is running on default 27017
2. Running the API should bring you to the [Swagger](https://localhost:5001/swagger/index.html) page.
3. Call https://localhost:5001/api/user/create POST with body of email and password - if email contains admin it will be created with admin privileges.
```json
{
  "email": "admin@hotmail.co.uk",
  "password": "123"
}
```
4. Call https://localhost:5001/api/user/auth POST with body of email and password to get bearer JWT token
```json
{
  "email": "admin@hotmail.co.uk",
  "password": "123"
}
```
5. In the top right of swagger use the Authorize to insert the JWT token taken from Step 4 or pass this token to your api software e.g postman as a bearer token
6. Once authenticated call https://localhost:5001/api/device/{userId} replacing {userId} with the Id taken from Step 3 to create a Device.
7. Device would call https://localhost:5001/api/device/createVehicle with body example below
```json
{
  "vehicle": {
    "deviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "registration": "ABC",
    "vehicleStats": {
      "type": "BMW",
      "fuelCapacity": 100,
      "topSpeed": 100
    }
  },
  "vehicleLocation": {
    "locations": [
      {
        "time": "2022-01-14T16:20:59.608Z",
        "lat": 13.728939921539132,
        "long": 100.49403502858323
      }
    ]
  }
}
 ```
 8. Once vehicle is created device would update vehicle using https://localhost:5001/api/device/updateLocation with body example of
```json
{
  "vehicleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "deviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "location": {
    "time": "2022-01-14T16:20:59.608Z",
    "lat": 13.728939921539132,
    "long": 100.49403502858323
  }
}
```
9. Using an admins bearer token authorized use https://localhost:5001/api/vehicle/getPosition/{vehicleId} replacing {vehicleId} to get a vehicles latest position and time
10. using an admins bearer token authorized use https://localhost:5001/api/vehicle/getTimePosition with body
```json
{
  "vehicleId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "startTime": "2022-01-14T16:27:33.394Z",
  "endTime": "2022-01-14T16:27:33.394Z"
}
```
to get a vehicles position between two set times

If step 9 and 10s call is done using a bearer token without admin in its username/as a role this will return 403

Dependencies
------------

### VehicleTracking ###
[Microsoft.AspNetCore.Authentication](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication/) v2.2.0  
[Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/) v3.1.22  
[Microsoft.Extensions.Configuration.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Abstractions/) v 2.2.0  
[Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/) v6.2.3  
[Swashbucke.AspNetCore.Annotations](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Annotations/) v6.2.3  
[System.IdentityModel.Tokens.Jwt](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt) v6.15.0  

### VehicleTracking.Core ###
[Microsoft.AspNetCore.Authorization](https://www.nuget.org/packages/Microsoft.AspNetCore.Authorization) v3.1.22  
[Microsoft.Extensions.Configuration.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Abstractions/) v6.0.0  
[Microsoft.Extensions.Logging.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions) v6.0.0  
[Microsoft.Extensions.Options](https://www.nuget.org/packages/Microsoft.Extensions.Options/) v6.0.0  
[Microsoft.Extensions.Options.ConfigurationExtensions](https://www.nuget.org/packages/Microsoft.Extensions.Options.ConfigurationExtensions) v6.0.0  
[MongoDB.Bson](https://www.nuget.org/packages/MongoDB.Bson) v2.14.1  

### VehicleTracking.Models ###
[MongoDB.Bson](https://www.nuget.org/packages/MongoDB.Bson) v2.14.1  
