namespace ExamOnline.Models
{
    public class AppSetting
    {
        public IConfigurationRoot config { get; set; }
        public AppSetting()
        {
            config = SetBasePath().AddJsonFile("appsettings.json").Build();
        }
        public string GetDbConnection()
        {
            return config.GetConnectionString("DbConnection");
        }
        private IConfigurationBuilder SetBasePath()
        {
            return new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory);
        }
    }
}
