using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace JoinReward.Models.JoinReward
{
    public class ReCaptchaValidator
    {
        public static ReCaptchaValidationResult IsValid(string captchaResponse)
        {
            if (string.IsNullOrWhiteSpace(captchaResponse))
            {
                return new ReCaptchaValidationResult()
                { Success = false };
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.google.com");

            var values = new List<KeyValuePair<string, string>>();
            //values.Add(new KeyValuePair<string, string>
            //("secret", "6Le5fG4fAAAAAAm9LVgdRcXYQLWi0aLiLwYmnKlH"));
            values.Add(new KeyValuePair<string, string>
            ("secret", "6LdxRsYfAAAAAEek6VS82usL3RiGtmbuurez3xsB"));
            // values.Add(new KeyValuePair<string, string>
            //("secret", "6LfuEZMfAAAAAHTzx2OlM6lR0ORnbiAFAP0tUEMv"));
            values.Add(new KeyValuePair<string, string>
             ("response", captchaResponse));
            FormUrlEncodedContent content =
        new FormUrlEncodedContent(values);

            HttpResponseMessage response = client.PostAsync
            ("/recaptcha/api/siteverify", content).Result;

            string verificationResponse = response.Content.
            ReadAsStringAsync().Result;

            var verificationResult = JsonConvert.DeserializeObject
            <ReCaptchaValidationResult>(verificationResponse);

            return verificationResult;
        }
    }
}
