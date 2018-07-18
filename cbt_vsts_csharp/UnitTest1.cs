using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Remote;

namespace BasicTest
{
    [TestClass]
    public class BasicTest
    {
        private TestContext testContextInstance;
        // put your username and authkey here:
        public static string username = "Your User Name";
        public static string authkey = "Your AuthKey";

        //The remote driver for connecting to our hub
        public RemoteWebDriver driver;

        //Used to communicate with the cross browser testing's api
        public CBTApi cbtApi;

        public BasicTest()
        {
            cbtApi = new CBTApi();


        }
        [TestMethod]
        public void TestMethod1()
        {
            //wrap the driver navigation and Assert in a try-catch for error logging via the API
            try
            {
                //Test a website title and set score using cross browser testing's api
                driver.Navigate().GoToUrl("http://crossbrowsertesting.github.io/selenium_example_page.html");

                //Check the title
                Assert.AreEqual(driver.Title, "Selenium Test Example Page");

                //Set the score via the api
                cbtApi.setScore(driver.SessionId.ToString(), "pass");
            }
            catch (Exception ex)
            {

                //take a screenshot, set description and score using cross browser testing's api
                var snapshotHash = cbtApi.takeSnapshot(driver.SessionId.ToString());
                cbtApi.setDescription(driver.SessionId.ToString(), snapshotHash, ex.ToString());
                cbtApi.setScore(driver.SessionId.ToString(), "fail");
                Console.WriteLine("caught the exception : " + ex);

                throw new Exception(ex.ToString());

            }
        }

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        [TestInitialize]
        public void SetupTest()
        {
            //Start by setting the capabilities
            var caps = new DesiredCapabilities();

            caps.SetCapability("name", "Basic Test Publix");
            caps.SetCapability("build", "1.0");


            caps.SetCapability("browserName", "Chrome");
            caps.SetCapability("version", "66");
            caps.SetCapability("platform", "Windows 10");
            caps.SetCapability("screenResolution", "1366x768");


            caps.SetCapability("username", username);
            caps.SetCapability("password", authkey);


            //Start the remote web driver
            driver = new RemoteWebDriver(new Uri("http://hub.crossbrowsertesting.com:80/wd/hub"), caps, TimeSpan.FromSeconds(180));


        }
        [TestCleanup()]
        public void MyTestCleanup()
        {
            driver.Quit();
        }

    }
    public class CBTApi
    {

        public string BaseURL = "https://crossbrowsertesting.com/api/v3/selenium";

        public string username = BasicTest.username;
        public string authkey = BasicTest.authkey;

        public string takeSnapshot(string sessionId)
        {
            // returns the screenshot hash to be used in the setDescription method. 
            // create the POST request object pointed at the snapshot endpoint
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + "/" + sessionId + "/snapshots");
            Console.WriteLine(BaseURL + "/" + sessionId);
            request.Method = "POST";
            request.Credentials = new NetworkCredential(username, authkey);
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "HttpWebRequest";
            // Execute the request
            WebResponse response = request.GetResponse();
            // store the response
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            // parse out the snapshot Hash value 
            var myregex = new Regex("(?<=\"hash\": \")((\\w|\\d)*)");
            var snapshotHash = myregex.Match(responseString).Value;
            Console.WriteLine(snapshotHash);
            response.Close();
            return snapshotHash;
        }

        public void setDescription(string sessionId, string snapshotHash, string description)
        {
            // encode the data to be written
            ASCIIEncoding encoding = new ASCIIEncoding();
            var putData = encoding.GetBytes("description=" + description);
            // create the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseURL + "/" + sessionId + "/snapshots/" + snapshotHash);
            request.Method = "PUT";
            request.Credentials = new NetworkCredential(username, authkey);
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "HttpWebRequest";
            // write data to stream
            Stream newStream = request.GetRequestStream();
            newStream.Write(putData, 0, putData.Length);
            newStream.Close();
            WebResponse response = request.GetResponse();
            response.Close();
        }

        public void setScore(string sessionId, string score)
        {
            string url = BaseURL + "/" + sessionId;
            // encode the data to be written
            ASCIIEncoding encoding = new ASCIIEncoding();
            string data = "action=set_score&score=" + score;
            byte[] putdata = encoding.GetBytes(data);
            // Create the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Credentials = new NetworkCredential(username, authkey);
            request.ContentLength = putdata.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "HttpWebRequest";
            // Write data to stream
            Stream newStream = request.GetRequestStream();
            newStream.Write(putdata, 0, putdata.Length);
            WebResponse response = request.GetResponse();
            response.Close();
        }
    }
}
