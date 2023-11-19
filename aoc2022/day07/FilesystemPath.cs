using System.Text;

namespace day07;

public class FilesystemPath
{
    private StringBuilder StringBuilder { get; } = new();

    private FilesystemPath(string path)
    {
        StringBuilder = new StringBuilder(path);
    }

    public FilesystemPath()
    {
        
    }
    
    public void In(string name)
    {
        if (name == "/")
        {
            StringBuilder.Clear();
        }
        else
        {
            StringBuilder.Append($"/{name}");
        }
    }

    public bool Out()
    {
        if (StringBuilder.Length <= 0)
        {
            return false;
        }
        
        var lastDirectory = StringBuilder.ToString().LastIndexOf("/", StringComparison.Ordinal);
        StringBuilder.Remove(lastDirectory, StringBuilder.Length - lastDirectory);
        return true;
    }

    public FilesystemPath Clone()
    {
        return new FilesystemPath(StringBuilder.ToString());
    }
    
    public override string ToString()
    {
        var str = StringBuilder.ToString();
        return str == "" ? "/" : str;
    }
}