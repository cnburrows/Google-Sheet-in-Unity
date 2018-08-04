using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System.Threading;
using System;

public class GoogleAuthorization : MonoBehaviour {
    public TextAsset secretsFile;

    public class Installed
    {
        public String client_id { get; set; }
        public String project_id { get; set; }
        public String auth_uri { get; set; }
        public String token_uri { get; set; }
        public String auth_provider_x509_cert_url { get; set; }
        public String client_secret { get; set; }
        public List<String> redirect_uris { get; set; }
    }
    public class JsonSecret
    {
        public Installed installed { get; set; }
    }

    public UserCredential GoogleAuthorizationBegin (String[] scopes) {
        var jsonSecret = JsonConvert.DeserializeObject<JsonSecret>(secretsFile.text);
        UserCredential credential;
        String credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = jsonSecret.installed.client_id,
                ClientSecret = jsonSecret.installed.client_secret
            },
            scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        return credential;
    }
}
