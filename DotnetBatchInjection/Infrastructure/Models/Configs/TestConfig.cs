namespace DotnetBatchInjection.Infrastructure.Models.Configs;

[AutoBind]
public class TestConfig
{
    public string Key1 { get; set; }
    public int Key2 { get; set; }
    public bool Key3 { get; set; }
}

[AutoBind]
public class TestConfig2
{
    public string Key1 { get; set; }
    public int Key2 { get; set; }
    public bool Key3 { get; set; }
}