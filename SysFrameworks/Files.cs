using System;
using System.Collections.Generic;
using System.Text;

public class Files
{

    public Files()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string EnsureCorrectFilename(string filename)
    {
        filename = filename.Replace("/", "\\");
        if (filename.Contains("\\"))
        {
            filename = filename.Substring(filename.LastIndexOf("\\") + 1);
        }
        return filename;
    }
}