using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace MvcApplication1.Infrastructure
{
public class GoogleWithFullNameClient : OpenIdClient
{
    public const string FULLNAME_KEY = "Fullname";

    public GoogleWithFullNameClient()
        : base("Google", WellKnownProviders.Google) { }

    protected override Dictionary<string, string> GetExtraData(IAuthenticationResponse response)
    {
        var fetchResponse = response.GetExtension<FetchResponse>();
        if (fetchResponse == null) return null;

        var result = new Dictionary<string, string> ();
        var fullname = fetchResponse.GetAttributeValue(WellKnownAttributes.Name.First) + " " +
            fetchResponse.GetAttributeValue(WellKnownAttributes.Name.Last);
        result.Add(FULLNAME_KEY, fullname);
        return result;
    }

    protected override void OnBeforeSendingAuthenticationRequest(IAuthenticationRequest request)
    {
        var fetchRequest = new FetchRequest();
        fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
        fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
        request.AddExtension(fetchRequest);
    }
}
}