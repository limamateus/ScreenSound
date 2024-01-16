namespace ScreenSoundApi.Dtos
{
    public record  MusicaRequest (string nome, int? anoLancamento, int ArtistaId, ICollection<GeneroRequest> Generos = null);
    

}
