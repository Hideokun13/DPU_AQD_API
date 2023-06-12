using System;
namespace DPU_AQD_API.Models;

public class SQLConection
{
    private const string Endpoint = "119.59.96.90";
    private const string Port = "3306";
    private const string DBUser = "aql_admin";
    private const string DBPwd = "jgLu&9430";
    private const string DBName = "aql_db";
    private const string ConnectionOption = "Convert Zero Datetime=True";

    public string strConnection = "server=" + Endpoint + ";port=" + Port + ";user=" + DBUser + ";password=" + DBPwd + ";database=" + DBName + ";" + ConnectionOption;
    // private string ConnectionString = "server=dpu-aqd-db.cea8uizk3jzd.ap-southeast-1.rds.amazonaws.com;port=3306;user=admin;password=admin1234!;database=DPU_AQD_DB;Convert Zero Datetime=True";
}