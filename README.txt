A demo of a WCF/REST API With Forms Authentication

I had been looking around the web for a solution on how to authenticate a WCF/REST API built with .NET
My goal was to keep it simple, so I stayed away from oAuth and other techniques and come to the conclusion that securing the API with Forms Authentication was a nice idea.

There are others out there that posted some solutions, which I got inspiration from. In particular, I have to thank Sergey Barskiy for his great post here:
http://dotnetspeak.com/index.php/2012/01/securing-wcf-with-forms-authentication/

My main concern was to keep the authentication simple, but secure enough. The major requirement for this code to be secure is to have SSL and do everything trough HTTPS.
Additionally, make sure you'll not be tempted to pass credential or other sensitive information through GET request on the URL. In the demo I included example on how to call POST service methods and that is the best way you can use along with HTTPS.
But of course, if you are planning to only play with this code, you don't need HTTPS. But if you are going to use in a production environment, please only use with HTTPS. I can't say it too many times.

About the project
The project consists of a C# class library wher I keep my WCF services and a Web Application that is the actual Web REST API.

WCFREST.Services library
For this demo there is only one service called CustomService. This is very simple and has methods to return a list of all people (Hard-coded) or a set matched to a keyword.
The first method, GetPeople(), is to demonstrate how you can use a GET request, which is good for getting sets of data without passing sensitive parameters on the URL.
The second method, GetPeopleWithPOST(), is to demonstrate how you can use a POST request using JSON to pass in your parameters through the request body (and again, don't forget HTTPS if you are doing this in a production environment).
The first returns a list of all the people. The second takes a parameter called Keyword that gets automatically deserialized into an instance of PersonLookupInfo. I did it this way because I wanted to show you how you can actaully pass several parameters and have them deserialized automatically as properties of a single data structure.

The GetPeople() method is decorated with a WebGet attribute (System.ServiceModel.Web) and the UriTemplate is "GetPeople".
The GetPeopleWithPOST() method is decorated with a WebInvoke attribute which can be used with other http methods, like POST or even PUT etc. In this case, I'm using POST by sepcifying the Method parameter.
Also, when using WebInvoke, you want to specify the BodyStyle to Bare. I'm not really sure how this work, but I experience some problems with the others and did not have time to look further into it.
You want to also make sure you specify the RequestFormat to Json.

WCFREST.WebAppAPI web application
The most critical parts are in the Web Application. The configuration is critical for WCF, as well for Url Routing.
My goal here was to have a web application that is only consumed by either a mobile device or another application, not directly by a user from a web browser.
One of the problems with ASP.NET Forms Authentication is the redirection to the login page. ASP.NET does this automatically. 
In my case, I did not want ASP.NET to do the redirection so a good solution I found on the web was to use an HttpModule that intercepts 302 redirection and returns instead a 401 (Unauhorized).
This is because when I call this API from another app, I always want to be able to check if I'm authorized or not. 
The HttpModule is called AuthRedirectModule and it's important to wire up this correctly in the Web.Config.
If you are using IIS as your webserver, you just need an entry in the <system.webServer><modules> section. But if you are using the Visual Studio development server (some call this Cassini), than you need the older IIS6 style configuration <httpModules> within the <system.web> section. However, this will break IIS7 so you might want to add:
<validation validateIntegratedModeConfiguration="false"/>
within the <system.webServer> section as well. 
I tried to comment the web.config sections to my best and I hope they are clear enough.

In the Web.config, we have to also make sure we setup Forms Authentication correctly.
In my case I decided I was going to use a HttpHandler to handle incoming Login request. This will return a Cookie if the Login was successful.
So in the web.config, I had to configure the <authentication> node like this:
	<authentication mode="Forms">
		<forms loginUrl="~/Login.ashx"/>
	</authentication>
	
I then had to deny access to all unauthenticated users to all areas of the web applications with this:
	<authorization>
		<deny users="?"/>
	</authorization>
	
But then I had to add an excpetion for the Login.ashx file which I want to be accessible by anonymous users:
	<location path="Login.ashx">
		<system.web>
			<authorization>
				<allow users="*"/>
			</authorization>
		</system.web>
	</location>
	
Finally, the <system.serviceModel> is configured just to handle default end points. I'm NOT going to use the config to setup each WCF service, becuase configuring WCF sucks. I've been working with WCF for many years and I still think this is one of the down sides of WCF.
The most important thing is to keep the <standardEndpoint> name to "" in order to setup our ServiceRoute correctly in the Global.asa instead.
One additional important thing is to have the <serviceHostingEnvironment> section as follows:
	<serviceHostingEnvironment
            aspNetCompatibilityEnabled="true"
            multipleSiteBindingsEnabled="true" />
			
In particular, you want to definitely have multipleSiteBindingsEnabled set to true if you are going to use multiple services.

Global.asa
Here we need to register the routes for the Url Routing in the Application_Start handler. Again, I tried to comment the code to my best. The most important part to remember is that the route prefix is going to be part of the URL.
While I was working at this I had the tendency to forget that and I was going straight to the method call in my URL (and getting errors of course).
So in our case, any call to any method in CustomService will always have the base URL be http://localhost/CustomService/ (if you use VS Web Server you'll have http://localhost:65432/CustomService/

Last but not least
When you run this Web Application in a web browser, you should get a blank page (empty response).
That is the same if you try to go navigate directly to Login.ashx 
In order to test everything out, you need to use Fiddler2 (or another application like a Windows Forms or Console app).
From Fiddler2, if you try to send a GET to http://localhost:65432 or http://localhost:65432/CustomService/GetPeople or any other area of the app, you should get a response with a status of 401 (Unauthorized).

If you send a POST to http://localhost:65432/Login.ashx and set the correct JSON content (like in the file "Fiddler2 Sample Requests.txt"), you should get a Cookie back in the Set-Cookie header.
Then you can copy this value and add to the headers of a new request to call the service methods, i.e. http://localhost:65432/CustomService/GetPeople and you'll be able to call the service methods this time, get the data returned along with a status of 200 (OK).

This step might be confusing for beginners. So again, first you build a request to calle the Login.ashx and get a cookie, which will come in the Set-Cookie header.
Then you copy the Set-Cookie value and add to your new request header as Cookie and you'll be able to call the service methods.

It's important to remember that for this demo I set the Cookie expiration to just 5 minutes, so if you wait that long, the Cookie will be expired and you'll have to request another one.

Any comments/suggeston are appreciated. If you need help, please do not hesitate to contact me.

