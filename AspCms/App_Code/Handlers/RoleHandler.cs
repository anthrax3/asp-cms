using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// Summary description for PostHandler
/// </summary>
public class RoleHandler : IHttpHandler
{
    public bool IsReusable
    {
        get { return false; }
    }


    public void ProcessRequest(HttpContext context)
    {
        //treba nam mode jer cemo u zavisnosti od njega, ako je edit da ispravljamo post ako je new da pravimo novi...
        var mode = context.Request.Form["mode"];

        var name = context.Request.Form["roleName"];
        var id = context.Request.Form["roleId"];

        if (mode == "edit")
        {
            Edit(Convert.ToInt32(id), name);
        }
        else if(mode == "new")
        {
            Create(name);

        }
        else if (mode == "delete")
        {
            Delete(name);
        }

        context.Response.Redirect("~/admin/role/");     
    }

    //metoda za formatiranje friendlyName-a...
    private static string CreateFriendlyName(string name)
    {
        name = name.ToLowerInvariant().Replace(" ", "-");
        name = Regex.Replace(name, @"[^0-9a-z-]", string.Empty);

        return name;
    }

    private static void Create(string name)
    {
        // trying if post with specific slug exists
        var result = RoleRepository.Get(name);
        if (result != null)
        {
            throw new HttpException(409, "Role allready exists");
        }

        RoleRepository.Add(name);
    }


    private static void Edit(int id, string name)
    {
        // trying if post with specific slug exists
        var result = RoleRepository.Get(id);
        if (result == null)
        {
            throw new HttpException(404, "No role with that id");
        }

        // PostRepository.Add(title, content, slug, published, authorId);
        RoleRepository.Edit(id, name);
    }

    private static void Delete(string name)
    {
        RoleRepository.Remove(name);
    }

}