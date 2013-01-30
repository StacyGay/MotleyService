using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Data;
using System.ServiceModel;

namespace MotleyService
{
	public class ConnectService<TClass, TInterface>
		where TClass : ClientBase<TInterface>, TInterface, new()
		where TInterface : class
	{
		public TClass service = new TClass();
		public BasicHttpBinding bindingSetup;
		public InspectorBehavior inspector = new InspectorBehavior();
		public string addressToUse = "";

		public ConnectService()
		{
			Init();
		}

		public void Init()
		{
		}

		/// <summary>
		/// Get XML Reply generated from service reference method call
		/// </summary>
		/// <returns></returns>
		public string GetXMLReply()
		{
			return inspector.mInspector.xmlReply;
		}

		/// <summary>
		/// Get XML Request generated from service reference method call
		/// </summary>
		/// <returns></returns>
		public string GetXMLRequest()
		{
			return inspector.mInspector.xmlRequest;
		}

		/// <summary>
		/// Change Endpoint address in service reference instance
		/// </summary>
		/// <param name="boolUseSSL"></param>
		/// <param name="strServiceAddress"></param>
		/// <param name="port"></param>
		/// <param name="securePort"></param>
		/// <returns></returns>
		public bool SetEndpoint(bool boolUseSSL, string strServiceAddress, string port = "80", string securePort = "443")
		{
			bool boolResult = true;

			try
			{
				service = new TClass();
				inspector = new InspectorBehavior();

				if (boolUseSSL)
				{
					string[] addressParts = strServiceAddress.Split('/');
					addressParts[0] += ":" + securePort;
					strServiceAddress = String.Join("/", addressParts);
					addressToUse = "https://" + strServiceAddress;
					bindingSetup = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
				}
				else
				{
					string[] addressParts = strServiceAddress.Split('/');
					addressParts[0] += ":" + port;
					strServiceAddress = String.Join("/", addressParts);
					addressToUse = "http://" + strServiceAddress;
					bindingSetup = new BasicHttpBinding(BasicHttpSecurityMode.None);
				}

				// setup new binding settings, still tinkering with these values (security vs large return sizes)
				bindingSetup.ReceiveTimeout = new TimeSpan(0, 10, 0);
				bindingSetup.SendTimeout = new TimeSpan(0, 10, 0);
				bindingSetup.MaxReceivedMessageSize = int.MaxValue / 2;
				bindingSetup.MaxBufferSize = int.MaxValue / 2;
				bindingSetup.MaxBufferPoolSize = int.MaxValue / 2;
				bindingSetup.ReaderQuotas.MaxArrayLength = int.MaxValue / 2;
				bindingSetup.ReaderQuotas.MaxBytesPerRead = int.MaxValue / 2;
				bindingSetup.ReaderQuotas.MaxNameTableCharCount = int.MaxValue / 2;
				bindingSetup.ReaderQuotas.MaxDepth = 64;

				// setup new endpoint address
				service.Endpoint.Address = new EndpointAddress(addressToUse);
				service.Endpoint.Behaviors.Add(inspector);
				service.Endpoint.Binding = bindingSetup;
			}
			catch (Exception e)
			{
				throw new ConnectServiceException("Error setting new endpoint address: " + e.Message, e);
			}
			return boolResult;
		}

		public void logXML(string ecrmClientID, DateTime WhenSent, string receiveStatus, bool isError)
		{
			// implement error logging here
		}
	}

	public class ConnectServiceException : _ErrorException
	{
		public ConnectServiceException(string errorMessage) : base(errorMessage) { }

		public ConnectServiceException(string errorMessage, Exception innerEx) : base(errorMessage, innerEx) { }
	}
}
