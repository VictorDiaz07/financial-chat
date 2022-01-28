# financial-chat
## Table of contents
* [General info](#general-info)
* [Features](#features)
* [Technologies](#technologies)
* [Prerequisites](#prerequisites)
* [Setup](#setup)
* [Usage](#usage)

---

## General nfo
A simple browser-based chat application using .NET. This application allows several users to talk in a chatroom and also to get stock quotes from an API using a specific command.

---

## Features
* Allow registered users to log in and talk with other users in a chatroom.
* Allow users to post messages as commands into the chatroom with the following format **/stock=stock_code**.
* Decoupled bot that calls an API using the stock_code as a parameter (https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here `aapl.us` is the stock_code).
* The bot parses the received CSV file and then send a message back into the chatroom using RabbitMQ. The message is a stock quote
with the following format: “APPL.US quote is $93.42 per share”. The post owner of the message is the bot.
*  Chat messages ordered by their timestamps. When a user gets connected to the chatroom the last 50 messages are displayed.
* Unit tests for the `BotService`.
* Use of .NET Identity for authentication.
* Messages that are not understood or any exceptions raised within the bot are handled.

---

## Technologies
The project is created with or uses:

* Angular 8.2.12
* .NET Core 3.1
* SQL Server
* RabbitMQ
* SignalR
* Bootstrap
* SweetAlert
* ToastR

---

## Prerequisites
* Visual Studio 2019
* SQL Server
* Docker Desktop
* Instance of RabbitMQ

**Note:** If you don't have an instance of RabbitMQ the easiest way to get it, is to run it in a Docker container (that's why Docker Desktop is a prerequisite), once you have installed Docker Desktop, run the following command in Powershell or Bash:

```sh
docker run -d --hostname my-rabbit --name some-rabbit -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password rabbitmq:3-management
```

---

## Setup
Follow the next steps to run this project locally:

1. Make sure you can run C# and Angular apps in your computer. For this, you'll need to have installed NodeJS, Angular version 8.2.12 or higher and .NET SDK version 3.1 or higher.

2. Open the project solution in Visual Studio, look inside the **ChatAPI** for the `appsettings.json` and set the connection string in the `appsettings.Development.json`.

3. Open the **Package Manager Console**, if you can't find it, click Tools in the menu and click `NuGet Package Manager > Package Manager Console`, then select **ChatAPI** in the Default Project dropdown from the Package Manager Console and run:
```
Update-Database
```

4. Open Powershell or Bash and run the next command to start the RabbitMQ Docker image as a container. It's important that you keep this Powershell or Bash window open while running the application.
```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

5. Now you can run the application.

---

## Usage
Once the application is running, you just need to register as an user and login into the app to access the chatroom.

### Considerations

To register, your password must have at least one:

* Uppercase character.
* Lowercase character.
* Digit.
* Non-alphanumeric character.

And at least 6 characters long.