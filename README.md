This demonstrates using JWT authentication between an API server and regular web app client.

To run this with Visual Studio:

1. Right-click on the **ApiServer** project and click Debug >> Start New Instance. This will bring up a Swagger UI in a browser window.

2. Right-click on the **SampleClient** project click Debug >> Start New Instance. This will bring up a browser showing a default Razor Pages home page. It has a login form.

3. Enter the user name **herbie** with password **hancock** and click Login. You will see a "weather forecast":

![img](https://adamosoftware.blob.core.windows.net/images/5Q2BNR77ZT.png)


## Server Project

- In the server project, there's a [UserStore](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Services/UserStore.cs) and related config added to [service container](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Program.cs#L17-L18). There's just a [single user](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Services/UserStore.cs#L53-L54), so use this when testing.

- I accepted the [default Swagger UI](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Program.cs#L21) from .NET project template. I'm not clear how this line relates to the next `AddSwaggerGen` method however.

- In order to make it so you can use Bearer headers in Swagger (and thereby test your authentication within the Server project), you have to add [this stuff](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Program.cs#L21-L43). I don't know why this is so verbose, nor even exactly what this is doing. I got it from an [earlier example](https://github.com/adamfoneil/CloudObjects/blob/master/CloudObjects.App/Extensions/ServiceCollectionExtensions.cs#L57) I had.

- In order to make your secure my controllers with the `[Authorize]` attribute, I added [this stuff](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Program.cs#L45-L59).

- Now my main controller [WeatherForecaseController](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Controllers/WeatherForecastController.cs) can use the `[Authorize]` attribute. Note that the [AccountController](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Controllers/AccountController.cs) provides the login method through a JWT token is obtained via [Login](https://github.com/adamfoneil/JwtDemo/blob/master/ApiServer/Controllers/AccountController.cs#L20).

## Client Project

- The client web app [adds Session capability](https://github.com/adamfoneil/JwtDemo/blob/master/SampleClient/Program.cs#L11-L15) to the service container. There are many ways to configure Session, this is just a barebones setup. This will be needed when we store the user token to represent an active user of our weather service.

- To access the weather service functionality, I used a Refit [interface](https://github.com/adamfoneil/JwtDemo/blob/master/SampleClient/Interfaces/IWeatherClient.cs). [Refit](https://github.com/reactiveui/refit) is a great library for building API clients because lets you write something close to a plain C# object with methods that map to backend API calls. You'd otherwise need to interact with `HttpClient` directly, which can be kind of verbose -- even with improvements made over the years in .NET http code. Refit can be hard to debug when things don't behave right, however, due to its tendency for silent fail. But I got this working without much trouble, and take every opportunity I have to practice.

- The main Page is [here](https://github.com/adamfoneil/JwtDemo/blob/master/SampleClient/Pages/Index.cshtml) along with its [code behind](https://github.com/adamfoneil/JwtDemo/blob/master/SampleClient/Pages/Index.cshtml.cs).

- A successful login [stores the token in Session](https://github.com/adamfoneil/JwtDemo/blob/master/SampleClient/Pages/Index.cshtml.cs#L48-L49)

- When the page loads, we check if there's a logged in user, then [get the Forecast](https://github.com/adamfoneil/JwtDemo/blob/master/SampleClient/Pages/Index.cshtml.cs#L63-L66) if so.

## Shared Project

The only reason for the Shared project is to have a common [WeatherForecast](https://github.com/adamfoneil/JwtDemo/blob/master/Shared/WeatherForecast.cs) object, used by both Client and Server.

I use linked source rather than a typical project reference in order to leverage shared assets like this. When adding linked elements, use the Visual Studio command Add Existing, but select Add Linked in the File dialog instead of the default Add button.

I could have used project references in both the client and server projects, but I've sort of gotten in the habit of using linked source because that works better with NuGet. NuGet packages are not allowed to have project references, but they can have linked source.
