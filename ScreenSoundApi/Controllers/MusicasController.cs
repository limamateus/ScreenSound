using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json;

namespace ScreenSoundApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicasController : ControllerBase
    {
        private DAL<Musica> _Dal;

        public MusicasController(DAL<Musica> dal)
        {
            _Dal = dal;
        }

        [HttpGet]
        public ActionResult ListDeMusicas()
        {

            // Retornando uma lista.
            return Ok(_Dal.Listar());

        }

        [HttpGet("Nome")]

        public ActionResult<Musica> Listar([FromQuery] string nome)
        {
            // Essa variavel está pegando os musicas do banco de dados.
            //  var dal = new DAL<Musica>(new ScreenSoundContext());
            // Aqui estou realizando um consulta 
            var musica = _Dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
            // Aqui esto validando não foi encontrado
            if (musica == null) return NotFound("Musica não encontrado!");
            // Serealizo em um objeto 

            var xMusica = JsonSerializer.Serialize(musica);
            // Retorno 
            return Ok(xMusica);

        }


        [HttpPost]

        public IActionResult NovoMusica([FromBody] Musica musica)
        {
            //  var dal = new DAL<Musica>(new ScreenSoundContext());

            _Dal.Adicionar(musica);

            return Ok(musica);
        }


        [HttpDelete("{id}")]

        public IActionResult Deletar(int id)
        {
            var buscarMusica = _Dal.RecuperarPor(a => a.Id == id);
            if (buscarMusica == null) return NotFound();

            _Dal.Deletar(buscarMusica);

            return NoContent();


        }


        [HttpPut("{id}")]

        public IActionResult Atualizar(int id, [FromBody] Musica musica)
        {
            var buscarMusica = _Dal.RecuperarPor(a => a.Id == id);
            if (buscarMusica == null) return NotFound();

            buscarMusica.Nome = musica.Nome;
            buscarMusica.Artista = musica.Artista;
            buscarMusica.AnoLancamento = musica.AnoLancamento;

            _Dal.Atualizar(buscarMusica);

            return NoContent();


        }
    }
}
