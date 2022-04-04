using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;

namespace SeguimientoEgresados.Services;

public interface IGoogleSheetsService
{
    public UserCredential credential { get; }
    public SheetsService service { get; }
    
    public Task<string> VerificarCuestionario(string email);
}