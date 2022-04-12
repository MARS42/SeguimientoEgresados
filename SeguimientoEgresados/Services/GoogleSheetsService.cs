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
        string[] Scopes = { SheetsService.Scope.Spreadsheets };
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

        //List<Google.Apis.Sheets.v4.Data.Request> requests = new List<Google.Apis.Sheets.v4.Data.Request>();

        // BasicFilter bf = new()
        // {
        //     Range = new GridRange()
        //     {
        //         SheetId = 748402175,
        //         StartColumnIndex = 1,
        //         EndColumnIndex = 2
        //     },
        //     FilterSpecs = new List<FilterSpec>()
        //     {
        //         new FilterSpec()
        //         {
        //             ColumnIndex = 1,
        //             FilterCriteria = new FilterCriteria()
        //             {
        //                 Condition = new BooleanCondition()
        //                 {
        //                     Type = "TEXT_EQ",
        //                     Values = new List<ConditionValue>()
        //                     {
        //                         new ConditionValue()
        //                         {
        //                             UserEnteredValue = email
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // };
        //
        // SetBasicFilterRequest bfr = new()
        // {
        //     Filter = bf
        // };
        //
        // requests.Add(new Google.Apis.Sheets.v4.Data.Request() {
        //     SetBasicFilter = bfr
        // });
        //
        //
        // Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest
        // {
        //     Requests = requests
        // };
        //
        // SpreadsheetsResource.BatchUpdateRequest request = service.Spreadsheets.BatchUpdate(requestBody, spreadsheetId);
        //
        // // To execute asynchronously in an async method, replace `request.Execute()` as shown:
        // Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetResponse response = await request.ExecuteAsync();
        //
        // Console.WriteLine(JsonConvert.SerializeObject(response));
        // Data.BatchUpdateSpreadsheetResponse response = await request.ExecuteAsync();

        String range = "Respuestas de formulario 1!B:B";

        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

        ValueRange response = await request.ExecuteAsync();

        IList<IList<object>> values = response.Values;

        if (values != null && values.Count > 0)
        {
            //Console.WriteLine("Name, Major");
            if (values.SelectMany(row => row.Cast<string>()).Any(cell => cell.Equals(email)))
            {
                return "Ok";
            }
        }
        else
        {
            return "Data are null o equals to 0";
        }
        return "Error";
    }
    
    public async Task<string> VerificarCuestionario(string email, DateTime fecha)
    {
        String spreadsheetId = "1UmZ1fropK_rOYmZ-5eDtTGoJIrP4bb4KtgNLgcgQDxA";

        String range = "Respuestas de formulario 1!A:B";

        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

        ValueRange response = await request.ExecuteAsync();

        IList<IList<object>> values = response.Values;

        IList<DateTime> fechas = new List<DateTime>(); 

        Console.WriteLine($"Fecha: {fecha}");
        if (values != null && values.Count > 0)
        {
            foreach (IList<object> fila in values)
            {
                if(fila.Count > 1 && fila[1].ToString()!.Equals(email))
                    fechas.Add(DateTime.Parse(fila[0].ToString()!));
            }

            if (fechas.Count <= 0)
                return "Error";
            
            if (fechas.Any(f => DateTime.Compare(f, fecha) > 0))
                return "Ok";
        }
        else
        {
            return "Data are null o equals to 0";
        }
        return "Error";
    }
}