using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System.Configuration;

namespace SeguimientoEgresados.Services;

public class GoogleSheetsService : IGoogleSheetsService
{
    public UserCredential credential { get; }
    public SheetsService service { get; }

    public GoogleSheetsService()
    {
        string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        string ApplicationName = "Google Sheets API .NET Quickstart";

        using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            string credPath = "token.json";
            
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            
            Console.WriteLine("Credential file saved to: " + credPath);
        }

        // Create Google Sheets API service.
        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public async Task<string> VerificarCuestionario(string email)
    {
        String spreadsheetId = "1UmZ1fropK_rOYmZ-5eDtTGoJIrP4bb4KtgNLgcgQDxA";

        List<Google.Apis.Sheets.v4.Data.Request> requests = new List<Google.Apis.Sheets.v4.Data.Request>();
        
        requests.Add(new Google.Apis.Sheets.v4.Data.Request() {

        });


        // TODO: Assign values to desired properties of `requestBody`:
        Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest();
        requestBody.Requests = requests;

        SpreadsheetsResource.BatchUpdateRequest request = service.Spreadsheets.BatchUpdate(requestBody, spreadsheetId);

        // To execute asynchronously in an async method, replace `request.Execute()` as shown:
        Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetResponse response = await request.ExecuteAsync();

        Console.WriteLine(JsonConvert.SerializeObject(response));
        // Data.BatchUpdateSpreadsheetResponse response = await request.ExecuteAsync();

        //String range = "Respuestas de formulario 1!A2:B20";

        //SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

        //ValueRange response = await request.ExecuteAsync();

        //IList<IList<Object>> values = response.Values;

        //if (values != null && values.Count > 0)
        //{
        //    //Console.WriteLine("Name, Major");
        //    //if (values.SelectMany(row => row.Cast<string>()).Any(cell => cell.Equals(email)))
        //    //{
        //    //    return "Ok";
        //    //}
        //    foreach(var fila in values)
        //    {
        //        string fecha = (string)fila[0];
        //        //string emailf = (string)fila[1];
        //        Console.WriteLine($"{fecha} - {email}");
        //    }
        //}
        //else
        //{
        //    return "Data are null o equals to 0";
        //}
        return "Error";
    }
}