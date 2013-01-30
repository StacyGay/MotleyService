using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;

namespace MotleyService
{
	public class MInspector : IClientMessageInspector
    {
        public string xmlRequest = "";
        public string xmlReply = "";

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            xmlReply = reply.ToString();
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            xmlRequest = request.ToString();
            return null;
        }
    }
}
