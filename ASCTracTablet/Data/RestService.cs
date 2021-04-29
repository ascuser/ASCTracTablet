    using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;


namespace ASCTracTablet.Data
{
    public class RestService : IRestService
    {
        HttpClient client;
        public ASCTracFunctionStruct.SignonType mySignonData { get; private set; }

        public RestService()
        {
#if DEBUG
            client = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
#else
            client = new HttpClient();
#endif
        }

        public async Task<ASCTracFunctionStruct.SignonType> Signon(ASCTracFunctionStruct.inputType aInputType)
        {
            mySignonData = new ASCTracFunctionStruct.SignonType();

            //var RestUrl = Url. Globals.hostURL + "api/logon";
            //Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:5001/api/Signon" : "https://localhost:44332/api/logon";

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/logon"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(aInputType);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content); // .GetAsync( uri, .GetAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    mySignonData = JsonConvert.DeserializeObject<ASCTracFunctionStruct.SignonType>(myResult);
                }
                else
                {
                    mySignonData.ReturnMessage = "Com Error " + response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return mySignonData;

        }

        public async Task<ASCTracFunctionStruct.ascBasicReturnMessageType> Signoff(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();
            //var RestUrl = Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:5001/api/Signon" : "https://localhost:44332/api/logoff";
            //var RestUrl = Globals.hostURL + "api/logoff";

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/logoff"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;

        }

        public async Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/postatussummary"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;

        }

        public async Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOStatusByPO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/postatusbypo"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        public async Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetConfirmReceiptInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/getconfirmreceiptinfo"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        public async Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doConfirmReceiptInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/getconfirmreceiptinfo"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        public async Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PrintPOReport(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/printporeport"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            // /api/coinfo/?aOrderNumber=CX002242&aUserID=admin&aSiteID=1
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aPONumber=" + aInboundMsg.inputDataList[0] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/poinfo/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPOList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();
            //aCustFilterType, string aCustData, int aFiltertype, string aFilterData
            string msg = "?aVendorFilterType=" + aInboundMsg.inputDataList[0] + "&aVendorData=" + aInboundMsg.inputDataList[1] +
                "&aFiltertype=" + aInboundMsg.inputDataList[2] + "&aFilterData=" + aInboundMsg.inputDataList[3] +
               "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/poinfo/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetMaintInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = aInboundMsg.UserID + "|" + aInboundMsg.SiteID + "|" + aInboundMsg.DataMessage;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/maintinfo/?aID=" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> SetMaintInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/maintinfo"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ImageCapture(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/imagecapture"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = aInboundMsg.UserID + "|" + aInboundMsg.SiteID + "|" + aInboundMsg.DataMessage;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/dockschd/?aID=" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/dockschd"); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doNewDockSchd(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string[] data = aInboundMsg.DataMessage.Split('|');
            string orderType = data[0].ToUpper();
            string ordernum = data[1];
            string dock = data[2];
            DateTime schdDT = Convert.ToDateTime(data[3]);


            string msg = "?aOrderType=" + orderType + "&aordernum=" + ordernum + "&aDock=" + dock + "&aDate=" + schdDT.ToString() + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/dockschd/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aStatusList=" + aInboundMsg.inputDataList[0] + "&aDockRange=" + aInboundMsg.inputDataList[1] + "&aDatefield=" + aInboundMsg.inputDataList[2] + "&aDateFilter=" + aInboundMsg.inputDataList[3] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/cosmsummary/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOStatusByCO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aStatusList=" + aInboundMsg.inputDataList[0] + "&aDockRange=" + aInboundMsg.inputDataList[1] + "&aDate=" + aInboundMsg.inputDataList[2] + "&aDatefield=" + aInboundMsg.inputDataList[3] + "&aDateFilter=" + aInboundMsg.inputDataList[4] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/costatusbyco/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            // /api/coinfo/?aOrderNumber=CX002242&aUserID=admin&aSiteID=1
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aOrderNumber=" + aInboundMsg.inputDataList[0] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coinfo/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();
            //aCustFilterType, string aCustData, int aFiltertype, string aFilterData
            string msg = "?aCustFilterType=" + aInboundMsg.inputDataList[0] + "&aCustData=" + aInboundMsg.inputDataList[1] +
                "&aFiltertype=" + aInboundMsg.inputDataList[2] + "&aFilterData=" + aInboundMsg.inputDataList[3] +
               "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coinfo/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PrintOrderContainer(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/printordercontainer");
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> CalcPCE(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aOrderType=" + aInboundMsg.inputDataList[0] + "&aOrderNum=" + aInboundMsg.inputDataList[1] + "&aLineNum=" + aInboundMsg.inputDataList[2]; // + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coinfo/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> UpdateOrdrDet(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aCO=" + aInboundMsg.inputDataList[0] + "&aLineNum=" + aInboundMsg.inputDataList[1] + "&aPCEType=" + aInboundMsg.inputDataList[2] + "&aNewStatus=" + aInboundMsg.inputDataList[3] + "&aClearPickLoc=" + aInboundMsg.inputDataList[4]; // + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coinfo/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetInvAvail(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aOrderNumber=" + aInboundMsg.inputDataList[0] + "&aLineNum=" + aInboundMsg.inputDataList[1] + "&aIncludeQC=" + aInboundMsg.inputDataList[2] + "&aIncludeExp=" + aInboundMsg.inputDataList[3] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/invavail/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetScheduleInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aOrderNumber=" + aInboundMsg.inputDataList[0] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/cosched/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doScheduleInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {

            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/cosched"); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetContainerList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aOrderNumber=" + aInboundMsg.inputDataList[0] + "&aContainerID=" + aInboundMsg.inputDataList[1] + "&aCntrType=" + aInboundMsg.inputDataList[2] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/cocontainerlookup/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOReportInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aOrderNumber=" + aInboundMsg.inputDataList[0] + "&aReportType=" + aInboundMsg.inputDataList[1] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coreport/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doCOReport(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();
            string msg = "?aReportType=" + aInboundMsg.inputDataList[0] + "&aPrinterID=" + aInboundMsg.inputDataList[1];

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coreport/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetUsersCOEntryInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aCustType=" + aInboundMsg.inputDataList[0] + "&aCustTypeData=" + aInboundMsg.inputDataList[1] +
                "&aItemType=" + aInboundMsg.inputDataList[2] + "&aItemTypeData=" + aInboundMsg.inputDataList[3] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coentry/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCOEntryInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aOrderNumber=" + aInboundMsg.inputDataList[0] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coentry/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> COEntrySaveHeader(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();
            string msg = "?aRecType=H";

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coentry/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> COEntrySaveDetail(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();
            string msg = "?aRecType=D";

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coentry/" + msg); // string.Format(RestUrl, string.Empty));
            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> CompleteCOEntry(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/coentry"); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetInvList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aItemID=" + aInboundMsg.inputDataList[0] + "&aIncludeQC=" + aInboundMsg.inputDataList[1] + "&aIncludeExp=" + aInboundMsg.inputDataList[2] +
                "&aIncludePicked=" + aInboundMsg.inputDataList[3] + "&aFieldType=" + aInboundMsg.inputDataList[4] + "&aFieldValue=" + aInboundMsg.inputDataList[5] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/invlookup/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aSkidID=" + aInboundMsg.inputDataList[0] + "&aItemID=" + aInboundMsg.inputDataList[1] + "&aLocationID=" + aInboundMsg.inputDataList[2] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/invlookup/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidHistory(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aSkidID=" + aInboundMsg.inputDataList[0] + "&aItemID=" + aInboundMsg.inputDataList[1] + "&aLocationID=" + aInboundMsg.inputDataList[2] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/invhistorylookup/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> UpdateSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/invlookup"); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetCounts(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();
            string msg = "?aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/physcount/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPhysLocs(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();
            /*string aCountNum, string aStartLocID, string aEndLocID, string aStartItemID, string aEndItemID,
            bool aIncludeLocCounted, bool aIncludeLocUncounted, bool aIncludeReviewed,
            bool aIncludeInvAll, bool aIncludeQtyVar, bool aIncludeLocChg, bool aIncludeLocEmpty, string aUserID, string aSiteI
             */
            string msg = "?aCountNum=" + aInboundMsg.inputDataList[0] + "&aStartLocID=" + aInboundMsg.inputDataList[1] + "&aEndLocID=" + aInboundMsg.inputDataList[2] +
                "&aStartItemID=" + aInboundMsg.inputDataList[3] + "&aEndItemID=" + aInboundMsg.inputDataList[4] +
                 "&aIncludeLocCounted=" + aInboundMsg.inputDataList[5] + "&aIncludeLocUncounted=" + aInboundMsg.inputDataList[6] + "&aIncludeReviewed=" + aInboundMsg.inputDataList[7] +
                  "&aIncludeInvAll=" + aInboundMsg.inputDataList[8] + "&aIncludeQtyVar=" + aInboundMsg.inputDataList[9] + "&aIncludeLocChg=" + aInboundMsg.inputDataList[10] + "&aIncludeLocEmpty=" + aInboundMsg.inputDataList[11] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/physcount/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetPhysLocitems(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aCountID=" + aInboundMsg.inputDataList[0] + "&aLocationID=" + aInboundMsg.inputDataList[1] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/physcount/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> RecountPhys(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/physcount"); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> PhysCount(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/physcount"); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetBOMAvailList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aItemID=" + aInboundMsg.inputDataList[0] + "&aQty=" + aInboundMsg.inputDataList[1] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/bom/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri); //, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOStatusSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aByWO=False&aStatusList=" + aInboundMsg.inputDataList[0] + "&aProdLineRange=" + aInboundMsg.inputDataList[1] +
                "&aDatefield=" + aInboundMsg.inputDataList[2] + "&aDateFilter=" + aInboundMsg.inputDataList[3] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/wosm/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOStatusByWO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aByWO=True&aStatusList=" + aInboundMsg.inputDataList[0] + "&aProdLineRange=" + aInboundMsg.inputDataList[1] +
                "&aDatefield=" + aInboundMsg.inputDataList[2] + "&aDateFilter=" + aInboundMsg.inputDataList[3] +
                "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/wosm/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }



        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOHdrInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aWorkorder=" + aInboundMsg.inputDataList[0] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/wohdr/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }




        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOHdrList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aProdLine=" + aInboundMsg.inputDataList[0] + "&aDate=" + aInboundMsg.inputDataList[1] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/wohdr/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }



        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ScheduleWO(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/wohdr"); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetProdSkidList(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aWorkorder=" + aInboundMsg.inputDataList[0] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/prodskid/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ProdNewSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/prodskid"); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOComponents(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aWorkorder=" + aInboundMsg.inputDataList[0] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/prodcomponent/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetWOComponentLicenses(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aWorkorder=" + aInboundMsg.inputDataList[0] + "&aSeqNum=" + aInboundMsg.inputDataList[1] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/prodcomponent/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> WOIssueComponent(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            //        public ASCTracFunctionStruct.ascBasicReturnMessageType WOIssueComponent(string aWorkorder_ID, string aSeqNum, string aSkidID, string aFGSkidID, string aItemID, string aLocationID, string aQty, ASCTracFunctionStruct.ascBasicInboundMessageType ainboundMsg)

            string msg = "?aWorkorder_ID=" + aInboundMsg.inputDataList[0] + "&aSeqNum=" + aInboundMsg.inputDataList[1] +  // + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
                "&aSkidID=" + aInboundMsg.inputDataList[2] + " &aFGSkidID=" + aInboundMsg.inputDataList[3] +
                "&aItemID=" + aInboundMsg.inputDataList[4] + " &aLocationID=" + aInboundMsg.inputDataList[5] + "&aQty=" + aInboundMsg.inputDataList[6];

            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/prodcomponent/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;

        }




        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetQCTests(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aWorkorder=" + aInboundMsg.inputDataList[0] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/qctest/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetQCTestInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aRecType=" + aInboundMsg.inputDataList[0] + "&aRecID=" + aInboundMsg.inputDataList[1] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/qctest/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> doQCTest(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aRecType=" + aInboundMsg.inputDataList[0] + "&aRecID=" + aInboundMsg.inputDataList[1]; // + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/qctest/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetSkidQCInfo(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aSkidID=" + aInboundMsg.inputDataList[0] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/qchold/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> ToggleQCSkid(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            //string msg = "?aRecType=" + aInboundMsg.inputDataList[0] + "&aRecID=" + aInboundMsg.inputDataList[1]; // + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/qchold"); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }


        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetReplenInfoForZone(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aZoneID=" + aInboundMsg.inputDataList[0] + "&aReplenFilterType=" + aInboundMsg.inputDataList[1] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/replen/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }

        async public Task<ASCTracFunctionStruct.ascBasicReturnMessageType> GetReplenSummary(ASCTracFunctionStruct.ascBasicInboundMessageType aInboundMsg)
        {
            ASCTracFunctionStruct.ascBasicReturnMessageType retval = new ASCTracFunctionStruct.ascBasicReturnMessageType();

            string msg = "?aZoneID=" + aInboundMsg.inputDataList[0] + "&aReplenFilterType=" + aInboundMsg.inputDataList[1] + "&aUserID=" + aInboundMsg.UserID + "&aSiteID=" + aInboundMsg.SiteID;
            Uri baseuri = new Uri(Globals.hostURL);
            Uri uri = new Uri(baseuri, "/api/ReplenSummary/" + msg); // string.Format(RestUrl, string.Empty));

            string json = JsonConvert.SerializeObject(Globals.curBasicMessage);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string myResult = await response.Content.ReadAsStringAsync();
                    retval = JsonConvert.DeserializeObject<ASCTracFunctionStruct.ascBasicReturnMessageType>(myResult);
                }
                else
                {
                    retval.successful = false;
                    retval.ErrorMessage = response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                mySignonData.ReturnMessage = "Exception " + ex.Message;
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return retval;
        }
    }
}