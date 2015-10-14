using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// Summary description for PostHandler
/// </summary>
public class TagHandler : IHttpHandler
{
	public TagHandler()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public bool IsReusable
    {
        get { return false; }
    }


    public void ProcessRequest(HttpContext context)
    {
        //treba nam mode jer cemo u zavisnosti od njega, ako je edit da ispravljamo post ako je new da pravimo novi...
        var mode = context.Request.Form["mode"];

        var name = context.Request.Form["tagName"];
        var friendlyName = context.Request.Form["tagFriendlyName"];
        var id = context.Request.Form["tagId"];

        if (string.IsNullOrWhiteSpace(friendlyName))
        {
            friendlyName =  CreateFriendlyName(name);
        }

        if (mode == "edit")
        {
            EditTag(Convert.ToInt32(id), name, friendlyName);
        }
        else if(mode == "new")
        {
            CreateTag(name, friendlyName);

        }
        else if (mode == "delete")
        {
            DeleteTag(friendlyName);
        }

        context.Response.Redirect("~/admin/tag/");     
    }

    //metoda za formatiranje friendlyName-a...
    private static string CreateFriendlyName(string name)
    {
        name = name.ToLowerInvariant().Replace(" ", "-");
        name = Regex.Replace(name, @"[^0-9a-z-]", string.Empty);

        return name;
    }

    private static void CreateTag(string name, string friendlyName)
    {
        // trying if post with specific slug exists
        var result = TagRepository.Get(friendlyName);
        if (result != null)
        {
            throw new HttpException(409, "Tag allready in use");
        }


        TagRepository.Add(name, friendlyName);
    }


    private static void EditTag( int id, string name, string friendlyName)
    {
        // trying if post with specific slug exists
        var result = TagRepository.Get(id);
        if (result == null)
        {
            throw new HttpException(404, "No post with that id");
        }

        // PostRepository.Add(title, content, slug, published, authorId);
        TagRepository.Edit(id, name, friendlyName);
    }

    private static void DeleteTag(string friendlyName)
    {
        TagRepository.Remove(friendlyName);
    }

}