# Tech Stack
- Backend Api - .NET Core Minimal API
- Frontend - .NET Core 8 MVC + Htmx
- Message Broker - RabbitMQ
- Messaging Library - MassTransit

# How to run?
1. Install [RabbitMQ (Windows installation)](https://www.rabbitmq.com)
2. Install [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
3. Run the following commands in PowerShell


```shell
rabbitmqctl stop_app
rabbitmqctl reset
rabbitmqctl start_app

rabbitmqctl add_user test test
rabbitmqctl set_user_tags test administrator
rabbitmqctl set_permissions -p / test ".*" ".*" ".*"`
```
4. Run Project **Api** & **Web**