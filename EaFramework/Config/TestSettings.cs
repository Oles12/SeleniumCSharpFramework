namespace EaFramework.Config;
using static EaFramework.Driver.DriverFixture;

public class TestSettings
{
    public BrowserType BrowserType { get; set; }
    public Uri ApplicationUrl { get; set; }
    public float? TimeoutInternal { get; set; }
    
    public TestRunType TestRunType { get; set; }
    
    public Uri GridUri { get; set; }
    
}

public enum TestRunType
{
    Local,
    Grid
}