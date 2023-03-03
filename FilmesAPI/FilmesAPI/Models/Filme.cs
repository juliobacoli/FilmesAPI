using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O titulo é obrigatório")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O genero é obrigatório")]
    [MaxLength(50, ErrorMessage = "O tamanho do filme não pode ultrapassar 50 caracteres")]
    public string Genero { get; set; }

    [Required(ErrorMessage = "O duração é obrigatório")]
    [Range(70, 600, ErrorMessage = "A duração deve ter entre 70 e 600 minutos")]
    public int Duracao { get; set; }
}
