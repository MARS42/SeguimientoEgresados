using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace SeguimientoEgresados.Utils;

public class VerificarCuestionarioTerminado
{
    public async Task<string> Verificar()
    {
        string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        string ApplicationName = "Google Sheets API .NET Quickstart";

        UserCredential credential;

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
        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        // Define request parameters.
        String spreadsheetId = "1UmZ1fropK_rOYmZ-5eDtTGoJIrP4bb4KtgNLgcgQDxA";
        
        //Datos de correo electrónico
        String range = "Respuestas de formulario 1!B2:B20";
        SpreadsheetsResource.ValuesResource.GetRequest request =
            service.Spreadsheets.Values.Get(spreadsheetId, range);
        
        ValueRange response = await request.ExecuteAsync();
        
        IList<IList<Object>> values = response.Values;
        string msg = "";
        if (values != null && values.Count > 0)
        {
            Console.WriteLine("Name, Major");
            foreach (var row in values)
            {
                // Print columns A and E, which correspond to indices 0 and 4.
                //Console.WriteLine("{0}, {1}", row[0], row[4]);
                foreach (var cell in row)
                {
                    msg += cell + ",";    
                }

                msg += "\n";
            }
        }
        else
        {
            msg = "No data found.";
        }
        return msg;
    }
}