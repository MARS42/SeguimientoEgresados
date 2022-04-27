using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;

namespace SeguimientoEgresados.Services;

public class GoogleSheetsService : IGoogleSheetsService
{
    //public UserCredential credential { get; }
    public SheetsService service { get; }

    public GoogleSheetsService(IOptions<GoogleSheetsOptions> config)
    {
        Console.WriteLine($"{config.Value.ClientId} - {config.Value.ClientSecret} - {config.Value.ServiceAccount} - {config.Value.ServiceAccountPass}");
        string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        string ApplicationName = "Google Sheets API .NET Quickstart";

        // using (var stream =
        //        new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        // {
        //
        // }
        
        // The file token.json stores the user's access and refresh tokens, and is created
        // automatically when the authorization flow completes for the first time.
        
        // string credPath = "token.json";
        // ClientSecrets secrets = new ClientSecrets()
        // {
        //         ClientId = config.Value.ClientId,
        //         ClientSecret = config.Value.ClientSecret
        // };
        // credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //     secrets,
        //     Scopes,
        //     "user",
        //     CancellationToken.None, new FileDataStore(credPath, true)).Result;
        // string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
        // foreach (var file in files)
        // {
        //     
        //     Console.WriteLine(file);   
        // }
        var certificate = new X509Certificate2("seguimiento-egresados-346121-bed230a7fafa.p12", config.Value.ServiceAccountPass, X509KeyStorageFlags.Exportable);
        
        ServiceAccountCredential credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(config.Value.ServiceAccount) {
                Scopes = Scopes
            }.FromCertificate(certificate));
            
        // Create Google Sheets API service.
        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public async Task<string> VerificarCuestionario(bool egresado, string email)
    {
        String spreadsheetId = egresado ? "1fAcF7KnSrsCmNkiW934EvUyTjt4QEyfO5lfwMk0Kgrw" : "1UmZ1fropK_rOYmZ-5eDtTGoJIrP4bb4KtgNLgcgQDxA";

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
    
    public async Task<string> VerificarCuestionario(bool egresado, string email, DateTime fecha)
    {
        String spreadsheetId = egresado ? "1fAcF7KnSrsCmNkiW934EvUyTjt4QEyfO5lfwMk0Kgrw" : "1UmZ1fropK_rOYmZ-5eDtTGoJIrP4bb4KtgNLgcgQDxA";
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