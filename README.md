# Telemetry data - School project

## Introduction
This project is a school assignment, where we wish to display telemetry data, such as `Temperature` and `Humidity` as well as a `TimeStamp` as to when the readings have been read - in the visualization of a chart, so that any user can get an overview of the readings.

The readings come from a previous project, where we have made a `Web API` which stores and also gets the data from a Database.

An extension to the project, is that we now also wish to display the same data, just on a web application - which will be made in a `Blazor Server` application. 

A further extension will be to have the `API`, `Broker` and `InfluxDB` to be located locally on a RaspberryPI.

## Architecture diagram
This is a diagram to give an overview of how the whole solution speaks together and where we're collecting our data from and what device is giving us the wanted data for the graphs on the mobile application.

<details>
  <summary>Click to see a description of the diagram flow</summary>
    
  So, we have our `2 applications` which will be the user interfaces of our entire solution. They will talk through a `API gateway` to retrieve and push data down through the solution.
    
  The API gateway also have a `POST` so when either of our **2 applications** will trigger a `switch component`, it will send either a `HIGH` or `LOW` as the body sample and pass it through the **MQTTNet Client** who publishes messages down to the **HiveMQ Broker** through the topic `telemetry/home/led`. The **Arduino board** is subscribed to that specific topic and is waiting for messages. Once it receives a message, the `builtin LED` will either light up or turn off, depending on the message's body sample.

  The **API gateway** can send several `GET` requests down to our **Influx database** to retrieve `Measurement` data. This data is coming through a **MQTTNet Client** which is `subscribed` to the topic `telemetry/home/#` to establish a connection with a **HiveMQ Broker** to recieve messages getting send to that specific topic. The **HiveMQ Broker** is getting messages from an **Arduino board** which has a `DHT11 sensor` reading and publishing every 30 seconds on new `Temperature` and `Humidity` readings through the topic `telemetry/home/{location}` so the **Influx database** is constantly fed with data.

</details>
    
![](./Images/architecture-diagram.png)

## Projects
| Project   | Platform                                                                                                    | Language |
|-----------|-------------------------------------------------------------------------------------------------------------|----------|
| `App`     | [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui)                                     | C#       |
| `API`     | [.NET RESTApi](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0) | C#       |
| `Arduino` | [Arduino MKR WiFi 1010](https://store.arduino.cc/products/arduino-mkr-wifi-1010)                            | C++      |
| `Web`     | [Blazor server-side](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-7.0)             | C#       |

## API Overview and Endpoints
| API                        | Description                                      | Request body | Response body                |
|----------------------------|--------------------------------------------------|--------------|------------------------------|
| `GET/telemetries`          | Get **all** Measurement items                    | None         | Array of Measurement items   |
| `POST/servo`               | Write to a servo                                 | string       | None                         |
| `GET/latestTelemetry`      | Get the latest Measurement reading               | None         | Single object of Measurement |
| `GET/telemetry/lastHour`   | Get **all** Measurement items the last hour      | None         | Array of Measurement items   |
| `GET/telemetry/lastDay`    | Get **all** Measurement items the last day       | None         | Array of Measurement items   |
| `GET/telemetry/lastWeek`   | Get **all** Measurement items the last week      | None         | Array of Measurement items   |
| `GET/telemetry/livingRoom` | Get **all** Measurement items in the Living room | None         | Array of Measurement items   |
| `GET/telemetry/kitchen`    | Get **all** Measurement items in the Kitchen     | None         | Array of Measurement items   |

## MQTT Topics
<table>
    <thead>
        <tr>
            <th>Topic</th>      
            <th>Pub/Sub</th>
            <th>Body sample</th>            
            <th>Description</th>
            <th>Client</th>
        </tr>
    </thead>
<tbody>
<tr>
<td>
    <i>telemetry</i>/home</i>/<i>led</i>
</td>
<td>
    Publish
</td>
<td>
    Sending "HIGH" or "LOW"
<td>
    Tell a device to turn a LED <b>ON</b> or <b>OFF</b> by using a switch on our .NET MAUI application. 
</td>
<td>
    <b>TelemetryAppClient</b>    
</tr>
<tr>
<td>
    <i>telemetry</i>/home</i>/<i>#</i>
</td>
<td>
    Subscribe 
</td>
<td>

```json
{
    "location": "living-room",
    "temperature": 22.5,
    "humidity": 10.2,
    "time": "2023-05-22T20:10:43.511Z"
}
```
<td>
    Receive a jSon object reading sensor values from the <b>MKRWiFi1010_Client</b>.    
</td>
<td>   
   <b>TelemetryAppClient</b>
</tr>
<tr>
<td>
    <i>telemetry</i>/home</i>/<i>{location}</i>
</td>
<td>
    Subscribe 
</td>
<td>

```json
{
    "location": "living-room",
    "temperature": 22.5,
    "humidity": 10.2,
    "time": "2023-05-22T20:10:43.511Z"
}
```
<td>    
    Publishing sensor values, being serialized into a jSon object so that we can receive it in our <b>TelemetryAppClient</b> and use that data to display in our .NET MAUI application.
</td>
<td>
    <b>MKRWiFi1010_Client</b>   
</tr>
</tbody>
</table>

## Requirements
 - [x] Show latest reading of `Temperature` and `Humidity` and the meassured times in local time.
 - [x] Show a graph of the meassurements, where you can choose between latest hour, day and week.
 - [x] It needs to have a button, which can activate a servo and simulate opening a window or turning on for the ventilation.
 - [x] The App needs to be built upon MVVM design pattern and contain Dependency injection.
 - [ ] Is robust toward an unstable internet connection.
 - [x] The project is turned in through a Github repository, with a good README.md file, which is presented for the class and teacher.
 - [x] Make an equivalent Blazor server side application to display the same things.


## Optional requirements
 - [x] The opportunity to choose different meassurement readings, eg. the different rooms in the house.
 - [ ] An alarm which will advertise that the `Temperature` is out of its limit - high or low.
 - [ ] Can show latest data, if the network disconnects.

## Docker requirements
 - [x] Have a local MQTT broker running on a RaspberryPI.
 - [x] Have InfluxDB locally located on the RaspberryPi.
     - [x] Make sure that `Telemetry` data is being sent up from the Arduino to the InfluxDB.
 - [x] Have your API located on the RaspberryPI.
     - [x] Make sure that you can retrieve data from the Web application, using the local API now.

## Installation guide - RaspberryPi w. Docker
If you ever wish to have your own `MQTT broker` and run it locally on a RaspberryPi, as well as the `API` and the Database, then there's a way to set all of this up by the use of Docker on Linux. It is actually made quite simple and I will below here try to guide you through how to set it up in an easy step-to-step guide along with the following commands.

### RaspberryPi OS
  
If you don't already have a RaspberryPi OS, then you can install one on an SD-card by using the  `Raspberry Pi Imager` on your PC by accessing this link: [RaspberryPi Imager](https://www.raspberrypi.com/software/) or you can use the following command in a terminal:

```
winget install -e --id RaspberryPiFoundation.RaspberryPiImager
```
    
When accessing the software, then you need to set up the following things in the settings menu:
- Give it a `hostname` that you will be able to remember. 
- `Enable SSH`.
- `Use password authentication` where you will type in a unique password.
- `Configure wifi` Writing in the `SSID` and the password for it. You can also configure the `Wifi country` which is **dk** in this case.
- `Set locale settings` where you will set the `Keyboard layout` to **dk** if you're in Denmark.
- Check the `Skip first-run wizard` and then Uncheck the `Enable Telemetry`.
  
When choosing an `OS`, then choose the one for your liking. In our case, we'll be choosing the newest `Raspberry PI OS Lite (64-bit)`.

Once the above configurations have been set, then you will `CHOOSE STORAGE` and then choose the SD-reader, choosing `WRITE` to overwrite the connected SD-Card. This SD-Card will then be put into the RaspberryPI and connect power for it to start up.

After this, open up a powershell terminal with administrator rights and make sure that your PC is on the same WiFi as the RasberryPI. then write the following command:

```
ssh pi@<hostname>.local
```

After this then write the command:
```
sudo apt update
``` 

Once that finishes, then write this command to install updates and remember to accept all of the prompted questions: 
```
sudo apt upgrade
``` 

### Docker on Rapberry Pi

On the terminal where you're on your ssh connection, then write following command to install docker:

```
sudo apt install docker docker.io docker-compose
```

Once docker has been successfully installed, the start docker with following command:

```
sudo systemctl start docker
```

Then you will be giving your user access to docker with this command:

```
sudo usermod -a -G docker <username>
```

Then it is important to restart your pi and you do that with the command `sudo reboot`.

Once the RaspberryPi has restarted, then you will be prompted to login again and then test that Docker has been installed by writing `docker ps` and retrieving the following in the terminal:

```
CONTAINER ID     IMAGE   COMMAND   CREATED    STATUS   PORTS   NAMES
```

### RabbitMQ as the broker
We will be using RabbitMQ as the `MQTT broker` to run locally on the RaspberryPi and to start using this then you be running the following command:

```
docker run -it -d  --name rabbitmq -p 5672:5672 -p 15672:15672 -p 1883:1883 rabbitmq:management
```

We are using the management tag, to access the UI of the broker - getting a good overview of Clients connected and the flows of the messages being sent and received.

However, this image is built for `AMQP` and therefore we will be needing to install a plugin but, to do so, we will have to attach ourselves to RabbitMqs terminal in the container, which can be done with the following command:

```
docker exec -it rabbitmq bash
```

This should change the prompt to stand on `root@xxxxxxxx:/#` and if that is correct, then we need to enable `MQTT` in RabbitMQ with the following command:

```
rabbitmq-plugins enable rabbitmq_mqtt 
```

You will switch back to the Raspberry terminal by wirting `exit` 

If you wish to not use the hostname to access the website, the you can write `ip -c address` and then use the `wlan` address to write in the url with the port `15672`. In my case it would be `10.135.16.x:15672`. This will make you prompt to login with user credentials.

#### Connecting to RabbitMQ from Arduino project

You will have to change the broker that the [Arduino project](https://github.com/jeongoks/HiveMQ_Broker) is currently using as well as the port, as RabbitMQ uses the `1883` port instead of `8883`.

I have also been giving it specific RabbitMQ Username and password, and this is because I added a new user inside of RabbitMQ's admin page; so that it will be a seperate client access the broker.

Testing this is working, you can see under the tab `Connections` in RabbitMQ that the Arduino now should be connected.

### Configurating InfluxDB on RaspberryPI

It is fairly simple to install InfluxDB in docker, as docker hub already has an image for us to use. So by running the following command, you can easily install it:

```
docker run -d -p 8086:8086 -v influxdb:/var/lib/influxdb -v influxdb2:/var/lib/influxdb2 influxdb:2.0
```

Then you can access InfluxDB by writing your `hostname.local:8086` or your `ip.address:8086`. The website will ask your to set up an account, so you will do that and follow the guide in there until you have it set up.

Once this is done, then you will need to create a new Bucket because you cannot use the initial bucket that it prompted you to create. While also creating this bucket, then make a new `scoped api token` which will have `Read/Write` access to this new bucket.

### Deploying API to RaspberryPI

To deploy, then you will have to copy your solution of your project to lay onto the RaspberryPI.
- Do this by using [WinSCP](https://winscp.net/eng/download.php).
- Connect with your RaspberryPI host on there in a new tab.
- Make a new directory on the RaspberryPI called `projects`.
- Choose the project, drag-and-drop over to the `projects` folder.
- Wait until the copying is done.

When this is done, then you will need to build an image of the API.

Step into the directory that the Solution is placed in. In this case it would be - replace the filepath with whereever yours is:

```
cd ./projects/Telemetry_MAUI
```

When standing there, you can now build the image with the following command:

```
docker build -t api:latest -f ./Telemetry.API/Dockerfile .
```

- `-tag` can be called whatever you want.
- `-f` is where you can find the Dockerfile in the project
- remember to add a **.** at the end!

When it finishes building, without any errors, then you need to run the image into a container.

```
docker run -p 32678:80 -e _BROKER=10.135.16.160 -d api:latest 
```

- `-p` set whatever port number followed with `:80` to let it know that it's a web.
- `-e` this can be used to set variables in the project, where I have specified the `Broker`.
- `-d` detacher, so it just connects when it is being run.
- the last thing is the image which is needed to be run.

## Use of third parties
| Package name                                                                             | Version        |
|------------------------------------------------------------------------------------------|----------------|
| [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) | 8.2.0          |
| [LiveChartsCore.SkiaSharpView](https://lvcharts.com/docs/maui/2.0.0-beta.710/gallery)    | 2.0.0-beta.710 |
| [Polly](https://github.com/App-vNext/Polly)                                              | 7.2.3          |
| [Radzen.Blazor](https://blazor.radzen.com/docs/)                                         | 4.11.2         |