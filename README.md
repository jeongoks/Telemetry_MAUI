# MAUI mobile app - School project

## Introduction
This project is a school assignment, where we wish to display telemetry data, such as `Temperature` and `Humidity` as well as a `TimeStamp` as to when the readings have been read - in the visualization of a chart, so that any user can get an overview of the readings.

The readings come from a previous project, where we have made a `Web API` which stores and also gets the data from a Database.

## Architecture diagram
This is a diagram to give an overview of how the whole solution speaks together and where we're collecting our data from and what device is giving us the wanted data for the graphs on the mobile application.

![](./Images/architecture-diagram.png)

## Projects
| Project   | Platform                                                                                                    | Language |
|-----------|-------------------------------------------------------------------------------------------------------------|----------|
| `App`     | [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui)                                     | C#       |
| `API`     | [.NET RESTApi](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0) | C#       |
| `Arduino` | [Arduino MKR WiFi 1010](https://store.arduino.cc/products/arduino-mkr-wifi-1010)                            | C++      |

## Requirements
 - [ ] Show latest reading of `Temperature` and `Humidity` and the meassured times in local time.
 - [ ] Show a graph of the meassurements, where you can choose between latest hour, day and week.
 - [ ] It needs to have a button, which can activate a servo and simulate opening a window or turning on for the ventilation.
 - [ ] The App needs to be built upon MVVM design pattern and contain Dependency injection.
 - [ ] Can show latest data, if the network disconnects.
 - [ ] Is robust toward an unstable internet connection.
 - [ ] The project is turned in through a Github repository, with a good README.md file, which is presented for the class and teacher.


## Optional requirements
 - [ ] The opportunity to choose different meassurement readings, eg. the different rooms in the house.
 - [ ] An alarm which will advertise that the `Temperature` is out of its limit - high or low.

## API Overview and Endpoints
| API               | Description               | Request body | Response body            |
|-------------------|---------------------------|--------------|--------------------------|
| `GET/telemetries` | Get all Telemetry items   | None         | Array of Telemetry items |
| `POST/servo`      | Write to a servo			| string	   | None					  |

## Use of third parties
| Package name                                                                             | Version |
|------------------------------------------------------------------------------------------|---------|
| [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) | 8.2.0   |


