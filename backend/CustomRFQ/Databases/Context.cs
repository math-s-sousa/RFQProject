namespace CustomRFQ.Databases;

public class Context
{
    public readonly DB _conn;
    public readonly string _dbType;

    public Context(IConfiguration conf)
    {
        _dbType = conf.GetValue<string>("DbType");

        if (_dbType == "MSSQL")
            _conn = new SQL(conf.GetConnectionString("Default"));
        else
            _conn = new HANA();   
    }
}
