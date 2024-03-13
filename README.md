### 1. Filters Section (Standard) (COMPLETED)

The users page contains 3 buttons below the user listing - **Show All**, **Active Only** and **Non Active**. Show All has already been implemented. Implement the remaining buttons using the following logic:
* Active Only – This should show only users where their `IsActive` property is set to `true` [x]
* Non Active – This should show only users where their `IsActive` property is set to `false` [x]

### 2. User Model Properties (Standard) (COMPLETED)

Add a new property to the `User` class in the system called `DateOfBirth` which is to be used and displayed in relevant sections of the app. [x]

### 3. Actions Section (Standard) (COMPLETED)

Create the code and UI flows for the following actions
* **Add** – A screen that allows you to create a new user and return to the list [x]
* **View** - A screen that displays the information about a user [x]
* **Edit** – A screen that allows you to edit a selected user from the list [x]
* **Delete** – A screen that allows you to delete a selected user from the list [x]

Each of these screens should contain appropriate data validation, which is communicated to the end user.

### 4. Data Logging (Advanced) (COMPLETED)

Extend the system to capture log information regarding primary actions performed on each user in the app. [x]
* In the **View** screen there should be a list of all actions that have been performed against that user. [x]
* There should be a new **Logs** page, containing a list of log entries across the application. [x]
* In the Logs page, the user should be able to click into each entry to see more detail about it. [x]
* In the Logs page, think about how you can provide a good user experience - even when there are many log entries. [x]

### 5. Extend the Application (Expert) (PARTIALLY COMPLETED)

Make a significant architectural change that improves the application.
Structurally, the user management application is very simple, and there are many ways it can be made more maintainable, scalable or testable.
Some ideas are:
* Re-implement the UI using a client side framework connecting to an API. Use of Blazor is preferred, but if you are more familiar with other frameworks, feel free to use them.
* Update the data access layer to support asynchronous operations. [x]
* Implement authentication and login based on the users being stored.
* Implement bundling of static assets.
* Update the data access layer to use a real database, and implement database schema migrations. [x]

### Missing dependencies
Dependencies can be installed in Visual Studio:
* Open Package Manager Console (Click Tools, NuGet Package Manager, Package Manager Console)
* Set Default project: to UserManagement.Data
* Copy the following lines and paste them one by one
    * Install-Package Microsoft.EntityFrameworkCore.SqlServer - Version 7.0.5
    * Install-Package Microsoft.EntityFrameworkCore.Design - Version 7.0.5
    * Install-Package Microsoft.EntityFrameworkCore.InMemory - Version 7.0.5
    * Install-Package Microsoft.EntityFrameworkCore.Tools - Version 7.0.5
    * Install-Package Microsoft.Extensions.Configuration - Version 7.0.0
    * Install-Package Microsoft.Extensions.Configuration.FileExtensions - Version 7.0.0
    * Install-Package Microsoft.Extensions.Configuration.Json - Version 7.0.0
    * Install-Package Microsoft.Extensions.DependencyInjection - Version 7.0.0
    * Install-Package Microsoft.Extensions.DependencyInjection.Abstractions - Version 7.0.0
    * Install-Package Microsoft.NET.Test.Sdk - Version 17.5.0
