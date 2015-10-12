using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// Summary description for BlogPostRepository
/// </summary>
public class PostRepository
{
    private static readonly string _connectionString = "DefaultConnection";

    // OVO JE KLASA KOJA SLUZI ZA UPIS I ISPIS POSTOVA IZ BAZE. NJENE METODE SU STATICKE 
    // PA SE MOGU KORISTITI BEZ INSTANCIRANJA KLASE U OSTATKU PROJEKTA GDJE ZATREBA...

	public PostRepository()
	{


	}

    // get metode ::::::>>>>
    public static dynamic Get(int id)
    {
        using (var db = Database.Open(_connectionString))
        {
            var sql = "SELECT * FROM Posts WHERE ID = @0";
            return db.QuerySingle(sql, id);
        }
    }

    public static dynamic Get(string slug)
    {
        using (var db = Database.Open(_connectionString))
        {
            var sql = "SELECT * FROM Posts WHERE Slug = @0";
            return db.QuerySingle(sql, slug);
        }
    }

    public static IEnumerable<dynamic> GetAll()
    {
        using (var db = Database.Open(_connectionString))
        {
            var sql = "SELECT * FROM Posts";
            return db.Query(sql);
        }
    }
    
    // post metode ::::::>>>>
    public static void Add(string title, string content, int authorId, string slug)
    { 
         using (var db = Database.Open("DefaultConnection"))
        {

            var sql = "INSERT INTO Posts (Title, Content, AuthorId, Slug) " +
                      "VALUES (@0, @1, @2, @3)" ;
            db.Execute(sql, title, content, authorId, slug);
        }
    }


}