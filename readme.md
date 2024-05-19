# Project goals
This project aims to develop a robust Task Management System with the following key objectives:
- User Authentication and Authorization: Implement secure user authentication and authorization mechanisms to ensure that only authorized users can access and manage tasks.
- Efficient Task Management: Enable users to create, update, and manage tasks effectively. This includes assigning tasks to multiple users, setting priorities, and deadlines.
- API-First Approach: Develop a RESTful API to handle all task management operations. This API will be used by both the web interface and potentially other clients in the future.

# How to run this project

## Prerequisites

- .NET SDK: Ensure you have the .NET SDK installed. 

- SQL Server: Ensure you have SQL Server installed and running on your machine. You can use SQL Server Express, which is free.

- Visual Studio.

## Running the Project from the Console
- Open the MyDbContext.cs file and ensure the _windowsConnectionString points to your SQL Server instance.
- Apply the database migrations to create the necessary database schema:
```
dotnet ef database update
```
- Start the projects by running:
```
dotnet run --project WebAPI
dotnet run --project BlazorApp1
```

## Running the Project from Visual Studio
- Open Visual Studio and navigate to File -> Open -> Project/Solution, then select the solution file (.sln) in the root directory of the cloned repository.
- Open the MyDbContext.cs file and ensure the _windowsConnectionString points to your SQL Server instance.
- Open the Package Manager Console (found under Tools -> NuGet Package Manager) and run:
```
Update-Database
```
- Run the Project:
Set WebAPI and BlazorApp1 as the startup projects by enabling them in the solution properties menu. Then press F5 or click on the Start button to run the projects.


