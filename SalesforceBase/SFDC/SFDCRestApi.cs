using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SalesforceBase.SFDC
{
    class SFDCRestApi
    {

        private HttpWebRequest createRequest(String url, String method)
        {
            HttpWebRequest request = null;

            Uri uri = new Uri(url);
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.CookieContainer = new CookieContainer();
            request.Method = method;
            if (method == "POST")
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
            }
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";

            //request.Headers["Accept-Language"] = "en-au";
            request.Headers["Cache-Control"] = "no-cache";

            //request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-ms-application, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";

            SFDCSession session = SFDCSession.Instance;
            request.Headers["Authorization"] =  "OAuth " + session.AccessToken;

            return request;
        }


        /**
        * Realiza una get o una post al API REST de Salesforce
        **/
        public async Task<JObject> Request(String method, String path)
        {
            SFDCSession session = SFDCSession.Instance;

            //url final
            String request = session.RequestUrl + path;

            return await Request(method, request, "");
        }

        public async Task<JObject> Request(String method, String url, String parameters)
        {
            JObject resultado = null;
            HttpWebRequest request = createRequest(url, method);
            String responseString = "";

            try
            {

            if (method == "POST")
                responseString = await makePostAsync(url, parameters);
            else
                responseString = await makeGetAsync(url);
            
                resultado = JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
                //TODO write log
                App.Offline = true;
                resultado = new JObject();
            }

            return resultado;
           
        }


        private Task<String> makeGetAsync(String url)
        {
            return Task.Factory.StartNew(() =>
            {
                HttpWebRequest request = null;
                String responseString = "";

                request = createRequest(url, "GET");

                request.BeginGetResponse((result) =>
                {
                    try
                    {
                        using (var response = (HttpWebResponse)request.EndGetResponse(result))
                        using (var streamResponse = response.GetResponseStream())
                        using (var streamRead = new StreamReader(streamResponse))
                              responseString = streamRead.ReadToEnd();                                    

                    }
                    catch (Exception ex)
                    {
                        responseString = ex.ToString();
                    }
                }, request);

                //TODO mejorar con un timeout!!
                while (responseString == String.Empty) ;
                return responseString;

            });


        }

     
        private Task<String> makePostAsync(String url, String data)
        {
            return Task.Factory.StartNew(() =>
            {
                HttpWebRequest request = createRequest(url, "POST");

                String responseString = "";

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(data);

                request.ContentLength = bytes.Length;

                request.BeginGetRequestStream((result) =>
                {
                    HttpWebRequest request2 = (HttpWebRequest)result.AsyncState;

                    // End the operation
                    Stream postStream = request2.EndGetRequestStream(result);

                    // Write to the request stream.
                    postStream.Write(bytes, 0, data.Length);
                    postStream.Close();

                    // Start the asynchronous operation to get the response
                    request2.BeginGetResponse((result2) =>
                    {
                        try
                        {
                            using (var response = (HttpWebResponse)request2.EndGetResponse(result2))
                            using (var streamResponse = response.GetResponseStream())
                            using (var streamRead = new StreamReader(streamResponse))
                                responseString = streamRead.ReadToEnd();

                        }
                        catch (Exception ex)
                        {
                            responseString = ex.ToString();
                        }

                    }, request2);

                }, request);


                //TODO realizar time out!!
                while (responseString == String.Empty) ;
                return responseString;


            });

        }
                       


    }
}
