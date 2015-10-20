using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;


public class RoleRepository
{
    private static readonly string _connectionString = "DefaultConnection";

    // OVO JE KLASA KOJA SLUZI ZA UPIS I ISPIS TAGOVA IZ BAZE. NJENE METODE SU STATICKE 
    // PA SE MOGU KORISTITI BEZ INSTANCIRANJA KLASE U OSTATKU PROJEKTA GDJE ZATREBA...

	public RoleRepository()
	{


	}

    // get metode ::::::>>>>
    public static dynamic Get(int id)
    {
        using (var db = Database.Open(_connectionString))
        {
            var sql = "SELECT * FROM Roles WHERE ID = @0";
            return db.QuerySingle(sql, id);
        }
    }

    public static dynamic Get(string name)
    {
        using (var db = Database.Open(_connectionString))
        {
            var sql = "SELECT * FROM Roles WHERE Name = @0";
            return db.QuerySingle(sql, name);
        }
    }

    public static IEnumerable<dynamic> GetAll(string orderBy = null, string where = null)
    {
        using (var db = Database.Open(_connectionString))
        {
            var sql = "SELECT * FROM Roles";

            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += " ORDER BY " + orderBy;
            }

            return db.Query(sql);
        }
    }
    
    // post metode ::::::>>>>
    public static void Add(string name)
    { 
         using (var db = Database.Open(_connectionString))
        {

            //var sql = "SELECT * FROM Roles WHERE Name = @0";
            //var roleName = db.Execute(sql, name);

            var sql = "INSERT INTO Roles (Name)" + 
                " VALUES (@0)";
             // TODO: Execute sql statement...
            db.Execute(sql, name);
        }
    }

    //edit metoda ::::::>>>>

    public static void Edit(int id, string name)
    {
        using (var db = Database.Open(_connectionString))
        {

            var sql = "UPDATE Roles SET Name = @0  WHERE Id = @1";
            db.Execute(sql, name, id);

        }
    }

    public static void Remove(string roleName)
    {
        using (var db = Database.Open(_connectionString))
        {
            var role = Get(roleName);

            if (role == null)
            {
                return;
            }

            var sql = "DELETE FROM Roles WHERE Name = @0 ";
            db.Execute(sql, roleName);

            sql = "DELETE FROM UsersRolesMap WHERE RoleId = @0";
            db.Execute(sql, role.Id);
        }
    }

    public static IEnumerable<dynamic> GetRolesForUser(int id)
    {
        var sql = "SELECT * FROM Roles r" + 
            " INNER JOIN UsersRolesMap m ON r.Id = m.RoleId" +
            " WHERE m.UserId = @0";

        using (var db = Database.Open(_connectionString))
        {
            return db.Query(sql, id);
        }
    }


}