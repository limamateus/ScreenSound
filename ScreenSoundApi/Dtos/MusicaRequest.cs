namespace ScreenSoundApi.Dtos
{
    public record  MusicaRequest (string nome, int? anoLancamento, int ArtistaId, string NomeDoArista, ICollection<GeneroRequest> Generos = null);
    

}
