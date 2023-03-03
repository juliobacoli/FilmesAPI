using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Infrastructure.Data
{
    public class FilmesContext : DbContext
    {
        public FilmesContext(DbContextOptions<FilmesContext> options) 
            : base (options)
        {

        }


    }
}
