using System;
using System.Data;
using System.Xml;
using System.IO;
using System.Net;

namespace MotleyService
{
	public class SoapConnect
	{
		public bool useSSL { get; set; }
		public string serviceAddress { get; set; }
		public string port { get; set; }
		public string securePort { get; set; }

		public SoapConnect(bool boolUseSSL, string strServiceAddress, string strPort = "80", string strSecurePort = "443")
		{
			this.useSSL = boolUseSSL;
			this.serviceAddress = strServiceAddress;
			this.port = strPort;
			this.securePort = strSecurePort;
		}

		private XmlDocument CreateSoapEnvelope(string xml)
		{
			XmlDocument soapEnvelope = new XmlDocument();
			soapEnvelope.LoadXml("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + xml + "</s:Body></s:Envelope>");
			return soapEnvelope;
		}

		public DataSet ConnectService(string methodName, string xmlToSend)
		{
			string addressToUse = "";
			DataSet returnData = new DataSet(methodName);

			if (useSSL)
			{
				string[] addressParts = serviceAddress.Split('/');
				addressParts[0] += ":" + securePort;
				serviceAddress = String.Join("/", addressParts);
				addressToUse = "https://" + serviceAddress;
			}
			else
			{
				string[] addressParts = serviceAddress.Split('/');
				addressParts[0] += ":" + port;
				serviceAddress = String.Join("/", addressParts);
				addressToUse = "http://" + serviceAddress;
			}

			XmlDocument soapRequest = CreateSoapEnvelope(xmlToSend);

			byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(soapRequest.OuterXml);
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(addressToUse);
			request.Method = "POST";
			request.ContentLength = requestBytes.Length;
			request.Accept = "XML";
			request.Headers.Add("SOAPAction: \"" + methodName + "\"");
			request.ContentType = "text/xml;charset=utf-8";

			using (Stream requestStream = request.GetRequestStream())
			{
				requestStream.Write(requestBytes, 0, requestBytes.Length);
			}

			using (var response = (HttpWebResponse)request.GetResponse())
			{
				//StreamReader stReader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
				//string responseString = stReader.ReadToEnd();
				//return responseString;

				returnData.ReadXml(response.GetResponseStream());
			}

			return returnData;
		}
	}
}
