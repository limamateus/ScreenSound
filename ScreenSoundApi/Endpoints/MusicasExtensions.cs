using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSoundApi.Dtos;
using System.Text.Json;

namespace ScreenSoundApi.Endpoints
{
    public  static class MusicasExtensions
    {
        private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
        {
            return musicaList.Select(a => EntityToResponse(a)).ToList();
        }

        private static MusicaResponse EntityToResponse(Musica musica)
        {
            return new MusicaResponse(musica.Id, musica.Nome!);
        }
        public static void AddEndPoitsMusicas(this WebApplication app)
        {
            #region Musicas
            app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) => {

                return Results.Ok(EntityListToResponseList(dal.Listar()));
            });

            app.MapGet("/Musica/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
            {
                var musica = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

                if (musica == null) return Results.NotFound();

                return Results.Ok(EntityToResponse(musica));
            });


            app.MapDelete("/Musica/{id}", ([FromServices] DAL<Musica> dal, int id) =>
            {
                var buscarMusica = dal.RecuperarPor(a => a.Id == id);
                if (buscarMusica == null) return Results.NotFound();

                dal.Deletar(buscarMusica);

                return Results.NoContent();


            });


            app.MapPost("/Musica/Novo", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequest musicaRequest) =>
            {
                var musica = new Musica(musicaRequest.nome,musicaRequest.anoLancamento);
                dal.Adicionar(musica);
                return Results.Ok(EntityToResponse(musica));

            });


            app.MapPut("/Musica/Atualizar/{id}", ([FromServices] DAL<Musica> dal, int id, [FromBody] MusicaRequest musicaRequest) =>
            {
                var buscarMusica = dal.RecuperarPor(a => a.Id == id);
                if (buscarMusica == null) return Results.NotFound();

                buscarMusica.Nome = musicaRequest.nome;               
                buscarMusica.AnoLancamento = musicaRequest.anoLancamento;
                dal.Atualizar(buscarMusica);
                return Results.NoContent();


            });


            #endregion
        }
    }
}
