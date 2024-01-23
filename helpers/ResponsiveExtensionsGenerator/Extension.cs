


public struct Extension(string name, string[] constants, string[] breakpoints, string[] templates)
{
    public string Name { get; set; } = name;
    public string[] Templates { get; set; } = templates;
    public string[] Constants { get; set; } = constants;
    public string[] Breakpoints { get; set; } = breakpoints;
}
