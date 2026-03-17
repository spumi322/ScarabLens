namespace ScarabLens;

public class ScarabResponse
{
    public List<ScarabLine> Lines { get; set; } = [];
    public List<ScarabItem> Items { get; set; } = [];
}

public class ScarabLine
{
    public string  Id           { get; set; } = "";
    public decimal PrimaryValue { get; set; }
}

public class ScarabItem
{
    public string Id   { get; set; } = "";
    public string Name { get; set; } = "";
}
