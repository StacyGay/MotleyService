MotleyService
=============

Service reference utilities library for dealing with multiple server addresses
By: Stacy Gay



Say you have multiple servers to hit all with the same WSDL definitions;
The MotleyService ConnectService class will maintain a service reference 
instance and allow you to switch end point bindings on the fly.  

All buffer size limits and reader quota settings are set within the class
to fit your size or security needs.

XML Message Inspectors are applied to each binding to allow for proper 
XML logging.


Connect Service Example:

foreach(var client in clients)
{
  var soapClient = new ConnectService<SoapClient, ISoapClient>();
  server.SetEndpoint(client.AlwaysUseSSL == 1, client.PMSWebserviceURL, client.PMSPort, client.PMSSecurePort);

  var results = soapClient.service.GetResults("arg1","arg2");

  LogXML(soapClient.GetXMLRequest(),soapClient.GetXMLReply());
}

Note:  When passing in the consumed service type, its interface is also needed
in order to ensure access to all binding properties and methods.
