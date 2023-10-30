using CustomRFQ.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CustomRFQ.Databases;

public class SQL : DB
{
    public SqlConnection globalConnection;
    public override List<Database.Config> _dbs { get; set; }
    public override Database.Smtp _smtp { get; set; }

    public SQL(string strConn)
    {
        try
        {
            SqlConnection connection = new(strConn);
            connection.Open();

            globalConnection = connection;

            LoadConfig();
        }
        catch (SqlException ex)
        {
            throw new Exception("Erro BD: " + ex.Message);
        }
    }

    public DataTable SQLQuery(string SQL)
    {
        DataTable dt = new();
        try
        {
            SqlCommand myCommand = new(SQL, globalConnection)
            {
                CommandTimeout = 0
            };
            var myReader = myCommand.ExecuteReader();
            dt.Load(myReader);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro BD: " + ex.Message);
        }
        return dt;
    }

    public void CloseConn()
    {
        try
        {
            if (globalConnection is not null)
                globalConnection.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro BD: " + ex.Message);
        }
    }

    public override void LoadConfig()
    {
        try
        {
            _dbs = globalConnection.Query<Database.Config>("SELECT * FROM [DbConfig]").ToList();
            _dbs.ForEach(a => a.SLApi = new(a.DB, a.Username, a.Password, a.BaseUrl));

            _smtp = globalConnection.Query<Database.Smtp>("SELECT TOP 1 * FROM [SmtpConfig]").FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro BD: " + ex.Message);
        }
    }

    public override Database.EventSender GetEvent(string guid)
    {
        try
        {
            return globalConnection.Query<Database.EventSender>($"SELECT * FROM [EventSender] WHERE Guid = '{guid}'").FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro BD: " + ex.Message);
        }
    }

    public override IEnumerable<dynamic> Query(string query)
    {
        return globalConnection.Query(query);
    }
}
