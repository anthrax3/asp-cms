using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.SessionState;
using WebMatrix.Data;

/// <summary>
/// Summary description for PostHandler
/// </summary>
public class PostHandler : IHttpHandler, IReadOnlySessionState
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
        AntiForgery.Validate();

        if (!WebUser.IsAuthenticated)
        {
            throw new HttpException(401, "You must login !");
        }

        if (!WebUser.HasRole(UserRoles.Admin) &&
            !WebUser.HasRole(UserRoles.Editor) &&
            !WebUser.HasRole(UserRoles.Author))
        {
            throw new HttpException(401, "You do not have permission to do this");
        }



        //treba nam mode jer cemo u zavisnosti od njega, ako je edit da ispravljamo post ako je new da pravimo novi...
        var mode = context.Request.Form["mode"];

        var title = context.Request.Form["postTitle"];
        var content = context.Request.Form["postContent"];
        var slug = context.Request.Form["postSlug"];
        var datePublished = context.Request.Form["postDatePublished"];
        var id = context.Request.Form["postId"];
        var postTags = context.Request.Form["postTags"];
        var authorId = context.Request.Form["postAuthorId"];

        IEnumerable<int> tags = new int[] { };

        if (!string.IsNullOrEmpty(postTags))
        {
            tags = postTags.Split(',').Select(v => Convert.ToInt32(v));
            
        }


        if ((mode == "edit" || mode == "delete") && WebUser.HasRole(UserRoles.Author))
        {
            if (WebUser.UserId != Convert.ToInt32(authorId))
            {
                throw new HttpException(401, "You do not have permission to do this");

            }
        }

        if (string.IsNullOrWhiteSpace(slug))
        {
            CreateSlug(title);
        }

        if (mode == "edit")
        {
            EditPost(Convert.ToInt32(id), title, content, slug, datePublished, Convert.ToInt32(authorId), tags);
        }
        else if(mode == "new")
        {
            CreatePost(title, content, slug, datePublished, WebUser.UserId, tags);

        }
        else if (mode == "delete")
        {
            DeletePost(slug);
        }

        context.Response.Redirect("~/admin/post/");     
    }

    //metoda za formatiranje slug-a...
    private static string CreateSlug(string title)
    {
        title = title.ToLowerInvariant().Replace(" ", "-");
        title = Regex.Replace(title, @"[^0-9a-z-]", string.Empty);

        return title;
    }

    private static void CreatePost(string title, string content, string slug, string datePublished, int authorId, IEnumerable<int> tags)
    {
        // trying if post with specific slug exists
        var result = PostRepository.Get(slug);
        if (result != null)
        {
            throw new HttpException(409, "Slug allready in use");
        }

        DateTime? published = null;

        if (!string.IsNullOrWhiteSpace(datePublished))
        {
            published = DateTime.Parse(datePublished);
        }

        PostRepository.Add(title, content, slug, published, authorId, tags);
    }


    private static void EditPost(int id, string title, string content, string slug, string datePublished, int authorId, IEnumerable<int> tags)
    {
        // trying if post with specific slug exists
        var result = PostRepository.Get(id);
        if (result == null)
        {
            throw new HttpException(404, "No post with that id");
        }

        DateTime? published = null;

        if (!string.IsNullOrWhiteSpace(datePublished))
        {
            published = DateTime.Parse(datePublished);
        }

        // PostRepository.Add(title, content, slug, published, authorId);
        PostRepository.Edit(id, title, content, slug, published, authorId, tags);
    }

    private static void DeletePost(string slug)
    {
        PostRepository.Remove(slug);
    }

}