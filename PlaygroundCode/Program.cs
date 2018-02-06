using System;
using System.Collections.Generic;
using System.Threading;

namespace FCM.Net.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            //var registrationId = "ID gerado quando o device é registrado no FCM";
            //var serverKey = "acesse https://console.firebase.google.com/project/MY_PROJECT/settings/cloudmessaging";

            var registrationId = "eY7er1US4Yw:APA91bGAQeTFre-A96dIzSG_1kORWWG7fBjh6sLXsuSIUvLxmQWTjK9IcgkfhqJhIkpUjLCglouVibOrpDNELIPfKLQPVMvey9ZFUTc8EbHQNiAERHUzeRQZgBWc_PcCh6pmGjjRh_N7";
            var serverKey = "AAAAxAFSVp8:APA91bEygq1VB5ca_CVhuKAStKEtHRhsEsxQxi7UMXD74hvSnZWUlbFZXEZFOLAJ1N2eNqwj7Km2WnIWrAjJrScDWhEQm7_01OqQ2E46vDNDA8YlebzUg2K71-us0-i5o_AMLzyx0Nw5";
            var projectId = "androidfcmsample-83288";

            var useLegacyFormat = false;

            if (useLegacyFormat)
                TestLegacyApiFormat(serverKey, registrationId);
            else
                TestV1ApiFormat(serverKey, registrationId, projectId);

            Console.Read();
        }

        //Test using V1 API format
        private static void TestV1ApiFormat(string serverKey, string registrationId, string projectId)
        {
            string title = "Teste .Net Core - V1";

            using (var sender = new V1.Sender(serverKey, projectId: projectId))
            {
                for (int i = 0; i < 3; i++)
                {
                    var message = new V1.Message
                    {
                        Token = registrationId ,
                        Notification = new V1.Notification
                        {
                            Title = title,
                            Body = $"Hello World@!{DateTime.Now.ToString()}"
                        }
                    };
                    var result = sender.SendAsync(message).Result;
                    WriteResult(result);
                    Console.WriteLine($"Success: {result.MessageResponse.Success}");

                    var json = "{\"notification\":{\"title\":\"mensagem em json\",\"body\":\"funciona!\"},\"token\":\"" +
                               registrationId + "\"}";
                    result = sender.SendAsync(json).Result;
                    WriteResult(result);

                    Thread.Sleep(1000);
                }
            }
        }

        //Test using Legacy API format
        private static void TestLegacyApiFormat(string serverKey, string registrationId)
        {
            string title = "Teste .Net Core - Lagacy";

            using (var sender = new Sender(serverKey))
            {
                for (int i = 0; i < 3; i++)
                {
                    var message = new Message
                    {
                        RegistrationIds = new List<string> { registrationId },
                        Notification = new Notification
                        {
                            Title = title,
                            Body = $"Hello World@!{DateTime.Now.ToString()}"
                        }
                    };
                    var result = sender.SendAsync(message).Result;
                    WriteResult(result);
                    Console.WriteLine($"Success: {result.MessageResponse.Success}");

                    var json = "{\"notification\":{\"title\":\"mensagem em json\",\"body\":\"funciona!\"},\"to\":\"" +
                               registrationId + "\"}";
                    result = sender.SendAsync(json).Result;
                    WriteResult(result);

                    Thread.Sleep(1000);
                }
            }
        }

        private static void WriteResult(ResponseContent result)
        {
            Console.WriteLine($"StatusCode: {result.StatusCode}");
            Console.WriteLine($"ReasonPhrase: {result.ReasonPhrase}");
            if (result.MessageResponse == null) return;

            Console.WriteLine($"MessageResponse.Success: {result.MessageResponse.Success}");
            Console.WriteLine($"MessageResponse.Failure: {result.MessageResponse.Failure}");
            Console.WriteLine($"MessageResponse.MulticastId: {result.MessageResponse.MulticastId}");
            Console.WriteLine($"MessageResponse.CanonicalIds: {result.MessageResponse.CanonicalIds}");
            Console.WriteLine($"MessageResponse.InternalError: {result.MessageResponse.InternalError}");
            Console.WriteLine($"MessageResponse.ResponseContent: {result.MessageResponse.ResponseContent}");
            if (result.MessageResponse.Results == null) return;

            foreach (var item in result.MessageResponse.Results)
            {
                Console.WriteLine($"MessageResponse.Results.MessageId: {item.MessageId}");
                Console.WriteLine($"MessageResponse.Results.RegistrationId: {item.RegistrationId}");
                Console.WriteLine($"MessageResponse.Results.Error: {item.Error}");
            }
            Console.WriteLine(new string('-', 20));
        }
    }
}
