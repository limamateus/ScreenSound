using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSoundApi.Dtos;
using System.Text.Json;

namespace ScreenSoundApi.Endpoints
{
    public  static class ArtistasExtensions
    {
        private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
        {
            return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
        }

        private static ArtistaResponse EntityToResponse(Artista artista)
        {
            return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
        }

        public static void AddEndPoitsArtistas( this WebApplication app)
        {
            #region Artistas

            app.MapGet("/Aritistas", ([FromServices] DAL<Artista> dal) =>
            {

                return Results.Ok(EntityListToResponseList(dal.Listar()));
            });

            app.MapGet("/Aritistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
            {
                var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

                if (artista == null) return Results.NotFound();



                return Results.Ok(EntityToResponse(artista));
            });


            app.MapDelete("/Artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
            {
                var buscarArtista = dal.RecuperarPor(a => a.Id == id);
                if (buscarArtista == null) return Results.NotFound();

                dal.Deletar(buscarArtista);

                return Results.NoContent();


            });


            app.MapPost("/Artistas/Novo", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequest artistaRequest) =>
            {
                var artista = new Artista(artistaRequest.nome, artistaRequest.bio, artistaRequest.fotoPerfil);

                dal.Adicionar(artista);

                return Results.Ok(EntityToResponse(artista));

            });


            app.MapPut("/Artistas/Atualizar/{id}", ([FromServices] DAL<Artista> dal, int id, [FromBody] ArtistaRequest artistaRequest) =>
            {
                var buscarArtista = dal.RecuperarPor(a => a.Id == id);
                if (buscarArtista == null) return Results.NotFound();

                buscarArtista.Nome = artistaRequest.nome;
                buscarArtista.Bio = artistaRequest.bio ;
                buscarArtista.FotoPerfil = artistaRequest.fotoPerfil;
                dal.Atualizar(buscarArtista);
                return Results.NoContent();


            });

            #endregion



        }


    }
}
