using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using Amazon;

namespace MadcakeHousePastry.Controllers
{
    public class BroadcastMessageController : Controller
    {
        private const string topicARN = "arn:aws:sns:us-east-1:774253713100:SNSExampletp059496";

        //function 1: connection string to the AWS Account
        private List<string> getKeys()
        {
            List<string> keys = new List<string>();

            //1. link to appsettings.json and get back the values
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build(); //build the json file

            //2. read the info from json using configure instance
            keys.Add(configure["awscredential:accesskey"]);
            keys.Add(configure["awscredential:secretkey"]);
            keys.Add(configure["awscredential:tokenkey"]);

            return keys;
        }

        //function 2: connect
        private AmazonSimpleNotificationServiceClient getConnect()
        {
            List<string> keys = getKeys();
            AmazonSimpleNotificationServiceClient clientconnect = new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);
            return clientconnect;
        }

        public IActionResult Index(string? msg)
        {
            ViewBag.msg = msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> processBroadcastMessage(string subjectTitle, string broadcastMessage)
        {
            string message = "Message has been delivered to subscribers.";

            AmazonSimpleNotificationServiceClient clientconnect = getConnect();

            try
            {
                PublishRequest request = new PublishRequest
                {
                    TopicArn = topicARN,
                    Subject = subjectTitle,
                    Message = broadcastMessage
                };
                PublishResponse response = await clientconnect.PublishAsync(request);
            }
            catch (AmazonSimpleNotificationServiceException ex)
            {
                message = "Error message: " + ex.Message;
            }
            catch (Exception ex)
            {
                message = "Error message: " + ex.Message;
            }

            return RedirectToAction("Index", "BroadcastMessage", new { msg = message });
        }
    }
}
