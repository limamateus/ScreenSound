using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScreenSoundApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistasController : ControllerBase
    {
        private DAL<Artista> _Dal;

        public ArtistasController(DAL<Artista> dal)
        {
            _Dal = dal;
        }

        [HttpGet]
        public ActionResult ListDeArtistas()
        {

            // Retornando uma lista.
            return Ok(_Dal.Listar());

        }

        [HttpGet("Nome")]

        public ActionResult<Artista> Listar([FromQuery] string nome)
        {
            // Essa variavel está pegando os artistas do banco de dados.
            //  var dal = new DAL<Artista>(new ScreenSoundContext());
            // Aqui estou realizando um consulta 
            var artista = _Dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            // Aqui esto validando não foi encontrado
            if (artista == null) return NotFound("Artista não encontrado!");
            // Serealizo em um objeto 

            var xArtista = JsonSerializer.Serialize(artista);
            // Retorno 
            return Ok(xArtista);

        }


        [HttpPost]

        public IActionResult NovoArtista([FromBody] Artista artista)
        {
            //  var dal = new DAL<Artista>(new ScreenSoundContext());

            _Dal.Adicionar(artista);

            return Ok(artista);
        }


        [HttpDelete("{id}")]

        public IActionResult Deletar(int id)
        {
            var buscarArtista = _Dal.RecuperarPor(a => a.Id == id);
            if (buscarArtista == null) return NotFound();

            _Dal.Deletar(buscarArtista);

            return NoContent();


        }


        [HttpPut("{id}")]

        public IActionResult Atualizar(int id, [FromBody] Artista artista)
        {
            var buscarArtista = _Dal.RecuperarPor(a => a.Id == id);
            if (buscarArtista == null) return NotFound();

            buscarArtista.Nome = artista.Nome;
            buscarArtista.Bio = artista.Bio;
            buscarArtista.FotoPerfil = artista.FotoPerfil;

            _Dal.Atualizar(buscarArtista);

            return NoContent();


        }

    }
}
