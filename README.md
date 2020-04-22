# .NET ServiceNow API Library
Library that uses the ServiceNow Table API to get or create new records.

## Functionality
- Create ServiceNow records of a predefined or your own object type by inheriting the [Record](./Sverto.ServiceNow/API/Record.cs) class

## Usage
```csharp
using Sverto.ServiceNow.API;
using Sverto.ServiceNow.Models;
using Sverto.ServiceNow.ValueTypes;

namespace TestApp
{
    public static class TestClass
    {
        public static async Task TestMethod()
        {
            // Your ServiceNow instance name
            var instance = "instancename";
            // ServiceNow Authentication by username
            // User requires API permissions
            var credential = new NetworkCredential("user", "password"); 
            // Init client
            using var client = new ServiceNowClient(instance, credential);


            // Get record by number example
            var number = new RecordNumber("SR1234567"); // Currently supports CHG/PRB/CTASK/SR

            RestResponseSingle response = await GetByNumber<SupportRequest>(number);

            if (!response.IsError)
            {
                SupportRequest sr = response.Result;
                // ...
            }


            // Get records by query example
            RestResponseQuery response = await client.GetByQuery<SupportRequest>("name=CompanyName");
            var foundCompany = response.Result.FirstOrDefault();
            

            // Get ResourceLink that can be used to create records (fields that contain an object in ServiceNow require a ResourceLink)
            // Could be simplified...
            var callerEmail = "email@my.dom";
            var callerQueryResult = await client.GetByQuery<User>($"email={callerEmail}^active=true");
            var caller = callerQueryResult.Result.First();
            var callerResourceLink = new ResourceLink(caller.Id);
            

            // Create record example
            var sr = new SupportRequest
            {
                Caller = callerResourceLink,
                ShortDescription = "My short description",
                Description = "My long description",
                Language = Language.English,
                // ...
            };
            var response = await client.Post(sr);


            // Add attachment example
            var response await client.PostAttachment<SupportRequest>(record_sys_id, "My attachment name.png", attachmentInBinary);

        }
    }
}
```