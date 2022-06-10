Contract Managment System Full Stack ASP.NET MVC application.
=======
![GitHub all releases](https://img.shields.io/github/downloads/ahmed7am1d/ContractManagmentSystem/total?logo=GitHub&style=flat-square)<br> 
Introduction:
------
Welcome to Contract Mangment System application, it is a full stack ASP.NET Core MVC (.NET6) application

App and Team Inforamtion:
------
`App Name:` Contract Managment System.<br>
`Created By:` Ahmed Al-Doori.<br>
`App For:` Blogic Company as Interview Task<br>
`Front-End Techs:` HTML5, CSS3, JavaScript, RazorCode, Jquery, CSS Media Quries for Responsive Design, BodymovinLibary for Lottie Animation <br>
`Back-End Techs:` ASP.NET Core, MS SQL Server, ClosedXML Library for Exporting Sheets<br>

What you will be able to do as Website owner:
--------
The system will allow you to do the following:
 - Browsering the Contracts from your database, with full joined details with the advisors and client
 - Admin Role
 - Login page 
 - Signout functionallity
 - Change admin settings [Name, PhoneNumber ...etc]
 - Protected Routing
 - Browsering all the clients from your database 
 - Browsering all the advisors from your database 
 - Search for contracts by insituit name 
 - Search for clients by client name. Filtering clients by [Age, Name, Surname] 
 - Search for advisors by advisor name. Filtering clients by [Age, Name, Surname] 
 - [Future feature] Admin can (add, delete, create) (Contracts, Advisors, Clients)
 - Export (Contracts, Advisors, Clients) as Excel Sheet or CSV
 
 App Internal Features:
 --------
 - ASP.NET Core Identitfication and authentication
 - Cookies managment
 - different roles 
 - seprated bussiness logic 
 - Responsive Design (Mobile Friendly Desing) 
 - Splash Screen 
 - Server Side Validation 
 - [Future feature]- Client Side Validation using JavaScript
 
 Software || Hardware Requirements:
 -----------
 - Internet Connection is Required
 - IDE that is able to run .net and C# projects
 - Your own database Connection string 
 - .net sdk and run time (.NET6)
 - .net tools for using terminal and ef
 - when using .NET CLI or CMD for commands follow (https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
 
 Setup Instructions:
 ------------
 (Prefered IDE: Visual Studio Community)<br>
 <strong>1-</strong> Download the GitRepo as Zip or Pull it to your local repo<br>
 <strong>2-</strong> Open the .NET Solution file (example.sln)<br>
 <strong>3-</strong> in appSettings.json, add your own connection string to your database<br>
 <strong>4-</strong> in case you are using [Visual Studio Community] open Package Manager console and add first Migration: <br> 
 `Add-Migration "0.0.1-FirstAppMigration"`<br>
 <strong>5-</strong> in case your are using VS Code or another IDE you can use Terminal or CMD inside the solution path and add:<br>
`dotnet ef migrations add InitialCreate`<br>
 <strong>6-</strong> in case you are using [Visual Studio Community] open Package Manager console again and update database using following code : <br> 
 `Update-Database`<br>
 <strong>7-</strong> in case your are using VS Code or another IDE again you can use Terminal or CMD inside the solution path and add:<br>
`dotnet ef database update`<br>
 <strong>8-</strong> To Login as Admin use the (Username: admin, Password: admin)<br>
 <strong>9-</strong> Here you go run the application and happy coding :)<br>

Internal App ScreenShots:
-----------
 **- Splash Screen:**
<img src="AppScreenShots/SplashScreen.PNG"></img>
 **- Login Page:**
<img src="AppScreenShots/SignInPage.PNG"></img>
 **- Account Settings Page:**
<img src="AppScreenShots/AccountSettings.PNG"></img>
 **- Contracts Page:**
<img src="AppScreenShots/ContractPage.PNG"></img>
 **- Clients Page:**
<img src="AppScreenShots/ClientPage.PNG"></img>
 **- Advisors Page:**
<img src="AppScreenShots/AdvisorsPage.PNG"></img>
 **- Responsive Account Settings:**<br>
<img src="AppScreenShots/ResponsiveAccountSettings.png"></img><br>
 **- Responsive SignIn Page:**<br>
<img src="AppScreenShots/ResponsiveSignInView.png"></img><br>
**- Responsive Contracts Page:**<br>
<img src="AppScreenShots/ResponsiveContractsView.png"></img><br>
>>>>>>> 122e6cba014d019c1d9aa396efdf5f3e59aee96f
