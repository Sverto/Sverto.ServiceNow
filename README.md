# .NET ServiceNow API Library
Library that uses the ServiceNow Table API to get or create new records.

## Functionality
- Create ServiceNow records (implement your own record type by inheriting [Record](./Sverto.ServiceNow/API/Record.cs))
- Get ServiceNow records
- Add attachments to existing records

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
            var instance = "instancename"; // Your ServiceNow instance name
            var credential = new NetworkCredential("user", "password"); // ServiceNow Authentication by username (user requires API permissions)
            using var client = new ServiceNowClient(instance, credential); // Init client

            // Get record by number (example)
            var number = new RecordNumber("SR1234567"); // Supports CHG/PRB/CTASK/SR
            RestResponseSingle response = await GetByNumber<SupportRequest>(number);
            if (!response.IsError)
            {
                SupportRequest sr = response.Result;
                // ...
            }

            // Get records by query example
            RestResponseQuery response = await client.GetByQuery<SupportRequest>("name=CompanyName");
            var foundCompany = response.Result.FirstOrDefault();
            
            // Get ResourceLink that can be used to create records (fields that contain an object in ServiceNow require a ResourceLink), could be simplified...
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
