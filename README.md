
TaskManagerProject
--------------------------------------------------------------------------------------------------
TaskManager is a web-based project management tool that allows users to manage tasks, projects, and team members efficiently. The application includes features for user authentication, project creation, task assignment, and profile management. It is built using ASP.NET Core MVC with Entity Framework Core for database management and is styled with Bootstrap for a responsive design.
--------------------------------------------------------------------------------------------------


PROJECT DESCRIPTION

The project consists of three main components:

API Project - responsible for providing REST API endpoints for user authentication and authorization. It contains controllers for registration, login, and other profile-related functionalities. The API uses Entity Framework Core for database access and JWT tokens for authentication.

Data Library - contains the database context and models used by the API and MVC projects. It defines the structure of the database and provides methods for accessing and manipulating the data.

MVC Project - represents the web application that uses the API for user authentication and authorization. The MVC project provides the user interface and logic for managing profiles, projects, and other related functionalities. It uses HttpClient for communication with the API.

DATABASE CONNECTION
The database connection is configured in the appsettings.json files of the API and MVC projects through the ConnectionStrings section. This information is used by Entity Framework Core to establish a connection to the database.

The application supports user authentication via email and password.
Users can register, log in, and manage their profiles.
The authentication is implemented using cookie-based authentication.

Authenticated users can view and update their profiles.
Users can upload profile pictures and update their personal information.

Users can create new projects and assign unique codes to each project.
Projects have descriptions and can have multiple team members assigned.
Users can view a list of their projects.

Within each project, users can create tasks, assign them to team members, and set start and end dates.
Tasks can be edited or deleted by the project creator.
Tasks are displayed on a calendar to visualize project timelines.



--------------------------------------------------------------------------------------------------
Dependencies
ASP.NET Core MVC: Framework for building web applications using the MVC pattern.
Entity Framework Core: ORM for interacting with the SQL Server database.
Bootstrap: CSS framework for responsive design.
FullCalendar: JavaScript library for creating and managing calendars.
