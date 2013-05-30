using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceBase.SFDC
{
    public class SFDCSession
    {

        public const String REFRESH_TOKEN = "sfdc_refreshToken";
        public const String URL_INSTANCE = "sfdc_urlInstance";

        public bool LogOut { get; set; }

        private static volatile SFDCSession instance;
        private static object syncRoot = new Object();
        private String _refreshToken = "";
        private String _instanceURL = "";//Se obtiene en el login

        public String AccessToken = "";//Se obtiene en el login
        public String OrgId = "";
        public String UserId = "";
        public String ApiVersion = "v26.0";
        public String BasePath = "services/data";
        public String ConsumerKey = "3MVG9PhR6g6B7ps5gmmXq6B6u_vRanARDfGBZekj1.SBoxK8tnEpyyV5fEY2Z7sHcfexDlsHpV4StkjWM2zCm";//Se obtiene de la configuracion de la app en SFDC
        public String ClientSecret = "5934228847934694207";//Se obtiene de la configuracion de la app en SFDC
        public String RedirectUri = "sfdcsample:success"; //La misma que tenga nuestra app en SFDC

        public String urlLoginBase = "https://login.salesforce.com/services/oauth2/authorize?response_type=token&display=touch&client_id=";

        public String InstanceUrl
        {
            get
            {
                if (_instanceURL != null && _instanceURL != String.Empty)
                {
                    return _instanceURL;
                }
                else
                {
                    if (IsolatedStorageSettings.ApplicationSettings.Contains(URL_INSTANCE))
                    {
                        _instanceURL = IsolatedStorageSettings.ApplicationSettings[URL_INSTANCE].ToString();
                        return _instanceURL;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            set
            {
                _instanceURL = value;
                IsolatedStorageSettings.ApplicationSettings[URL_INSTANCE] = _instanceURL;
            }
        }
        public String RefreshToken
        {
            get
            {
                if (_refreshToken != null && _refreshToken != String.Empty)
                {
                    return _refreshToken;
                }
                else
                {
                    if (IsolatedStorageSettings.ApplicationSettings.Contains(REFRESH_TOKEN))
                    {
                        var bytes = IsolatedStorageSettings.ApplicationSettings[REFRESH_TOKEN] as byte[];
                        var bytesDescifrados = ProtectedData.Unprotect(bytes, null);

                        _refreshToken = Encoding.UTF8.GetString(bytesDescifrados, 0, bytesDescifrados.Length);
                        return _refreshToken;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            set
            {
                _refreshToken = value;

                var refreshTokenCifrado = ProtectedData.Protect(Encoding.UTF8.GetBytes(value), null);
                IsolatedStorageSettings.ApplicationSettings[REFRESH_TOKEN] = refreshTokenCifrado;
            }
        }

        /**
         * Instancia Singleton
         **/
        public static SFDCSession Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SFDCSession();
                        }
                    }
                }
                return instance;
            }
        }


        /**
        * Devolvemos la url para las peticiones al API REST
        * generalmente suele tener este formato -> https://na1.salesforce.com/services/data/v26.0/
        **/
        public String RequestUrl
        {
            get
            {
                return InstanceUrl + "/" + BasePath + "/" + ApiVersion + "/";
            }
        }


        /**
         * Devuelve true si no tenemos access token. En ese caso se debera llamar a la pantalla de Login.
         **/
        public async Task<bool> oAuthUserAgentFlow()
        {
            if (this.AccessToken != null && this.AccessToken != String.Empty)
            {
                return false; //Tenemos access token podemos continuar en la aplicacion
            }
            
            if (RefreshToken != null && RefreshToken != String.Empty)
            {
                //No tenemos access token pero si tenemos refresh token, asi que lo utilizamos para obtener un nuevo token.
                //Si lo obtenemos correctamente devolvemos false para no iniciar el login
                var resultado = await RefreshTokenFlow();
                if (resultado)
                    return false;
            }

            //En cualquier otro caso, iniciamos el proceso de login para obtener un nuevo access token
            return true;
             
        }

        /**
         * Refrescamos el access token usando el refresh token.
         * 
         * Devolvemos true si hemos obtenido correctamente el access token. false e.c.c.
        **/
        public async Task<bool> RefreshTokenFlow()
        {
            //si no tenemos refres token no podemos hacer nada
            if (RefreshToken == null || RefreshToken == "")
            {
                return false;
            }
            else
            {
                SFDCRestApi sfdcRestApi = new SFDCRestApi();
                JObject responseObject = await sfdcRestApi.Request("POST",
                                                                   "https://login.salesforce.com/services/oauth2/token",
                                                                   string.Format("grant_type=refresh_token&client_id={0}&refresh_token={1}",
                                                                   ConsumerKey,
                                                                   RefreshToken));
                
                AccessToken = (string)responseObject["access_token"];
                InstanceUrl = (string)responseObject["instance_url"];
                return true;
            }
        }

        /**
        * Revokamos el Token
        *
        **/
        public async Task<bool> callLogOut()
        {
            SFDCRestApi sfdcRestApi = new SFDCRestApi();
            JObject responseObject = await sfdcRestApi.Request("GET",
                                                               "https://login.salesforce.com/services/oauth2/revoke?token=" + this.AccessToken + "&refresh_token=" + this.RefreshToken,
                                                               ""
                                                              );

            return true;
        }


    }
}
