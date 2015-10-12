using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// Summary description for PostHandler
/// </summary>
public class PostHandler : IHttpHandler
{
	public PostHandler()
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
        var title = context.Request.Form["postTitle"];
        var content = context.Request.Form["postContent"];
        var slug = CreateSlug(title);

        var result = PostRepository.Get(slug);
        if (result != null)
        {
            throw new HttpException(409, "Slug allready in use");
        }

        PostRepository.Add(title, content, 1, slug);

        context.Response.Redirect("~/admin/post/");     
    }

    //metoda za formatiranje slug-a...
    private static string CreateSlug(string title)
    {
        title = title.ToLowerInvariant().Replace(" ", "-");
        title = Regex.Replace(title, @"[^0-9a-z-]", string.Empty);

        return title;
    }



}