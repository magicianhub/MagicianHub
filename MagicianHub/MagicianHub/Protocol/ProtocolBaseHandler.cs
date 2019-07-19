using MagicianHub.Authorization;
using MagicianHub.Extensions;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;

namespace MagicianHub.Protocol
{
    public static class ProtocolBaseHandler
    {
        public static void ProcessQuery(ProtocolActivatedEventArgs eventArgs)
        {
            var (protocolTypes, parameters) = BaseProtocol(eventArgs.Uri);
            if (protocolTypes == ProtocolTypes.Auth)
            {
                AuthorizationProtocol.ProcessProtocolAsync(parameters);
            }
        }
        
        public static (ProtocolTypes, Dictionary<string, string>) BaseProtocol(Uri uri)
        {
            var uriParts = uri.GetPartAsArray();
            var uriParameters = uri.GetPartParameters();

            if (uriParts.Length == 1)
            {
                if (uriParts[0] == "auth")
                {
                    if (uri.GetPartParameters().Count == 0)
                    {
                        return (ProtocolTypes.Auth, new Dictionary<string, string>());
                    }
                    if (uri.GetPartParameters().Count == 1 ||
                        uri.GetPartParameters().Count == 2)
                    {
                        return (ProtocolTypes.Auth, uriParameters);
                    }
                }
            }

            return (ProtocolTypes.None, new Dictionary<string, string>());
        }
    }
}
