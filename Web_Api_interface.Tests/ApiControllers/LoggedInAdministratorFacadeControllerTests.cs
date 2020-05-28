using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web_Api_interface.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows;
using Flight_Center_Project_FinalExam_DAL;

namespace Flight_Center_Project_FinalExam_UnitTest
{
    [TestClass()]
    public class LoggedInAdministratorFacadeControllerTests
    {
        [TestMethod()]
        public async void CreateNewAirline_Test()
        {
            string methodUrl = "https://localhost:44361/api/LoggedInAdministratorFacade/CreateNewAirline";

            string base64mockImage = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAAQABADASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDj/DegWmvahLHqUs0VsqqqbDjcxznmmeINFt9D1s2OnSyS27plVdslWzggHvV7wBfXET3dtLJuQqGGBymTyfp79qoeNNQa91VY4WDJaKY8svDEnOPpjArlSlzWO18nsVLqf//Z";

            AirlineCompany company = new AirlineCompany("-= Even More Super Buper Fancy Airline =-", 8563, base64mockImage, Guid.NewGuid().ToString(), "JubJUbJub-1@2", 10934);

            //Get request
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(methodUrl);

            //Add an Accept header for JSON format
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IklTU2RxIiwicm9sZSI6IkFkbWluaXN0cmF0b3IiLCJQYXNzd29yZCI6IldCQTVXIiwibmJmIjoxNTg5MzkyNzkxLCJleHAiOjE1ODk5OTc1OTEsImlhdCI6MTU4OTM5Mjc5MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNjEvIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNjEvIn0.fXzA0I0O9OnndyhukVVGoYl-pCleHaeKDTq7PFGjXV8");

            //List data response
            //HttpResponseMessage response = client.GetAsync("").Result; // Blocking call! Program will wait here until a response is received or a timeout occurs.
            HttpResponseMessage response = await client.PostAsJsonAsync(methodUrl, company);            

            if(response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                MessageBox.Show(String.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
            }

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();






            Assert.Fail();
        }
    }
}