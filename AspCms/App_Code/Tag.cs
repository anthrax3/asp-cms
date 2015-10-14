﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using WebMatrix.Data;


public class Tag
{
	public Tag()
	{
		
	}

    private static WebPageRenderingBase Page 
    {
        get 
        {
            return WebPageContext.Current.Page;
        }
    }

    public static string Mode
    {
        get 
        {
            if (Page.UrlData.Any())
            {
                return Page.UrlData[0].ToLower();
            }
            return string.Empty;
        }
    }

    public static string FreindlyName
    {
        get
        {
            if (Mode != "new")
            {
                return Page.UrlData[1];
            }

            return string.Empty;
        }
    }

    public static dynamic Current
    {
        get 
        {
            var result = TagRepository.Get(FreindlyName);

            return result ?? CreateTagObject();
        }
    }

    private static dynamic CreateTagObject()
    {
        dynamic obj = new ExpandoObject();

        obj.Id = 0;
        obj.Name = "";
        obj.UrlFriendlyName = "";

        return obj;
    }
}