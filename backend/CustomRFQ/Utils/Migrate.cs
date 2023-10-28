using static CustomRFQ.Models.ServiceLayer;
using System.Text.Json;

namespace CustomRFQ.Utils;

public class Migrate
{
    private readonly SLApi _sL;
    public Migrate(SLApi sL)
    {
        _sL = sL;
        Process();
    }

    private void Process()
    {
    }

    private void CreateTable(string name, string description, BoUTBTableType type)
    {
        try
        {
            (string success, Erro failed) table = _sL.Get($"UserTablesMD('{name}')").Result;

            if (table.failed != null && table.failed.error.code == -2028)
            {
                string payload = JsonSerializer.Serialize(new UserTablesMD.Value()
                {
                    TableName = name,
                    TableDescription = description,
                    TableType = type.ToString()
                });

                (string success, Erro failed) create = _sL.Post("UserTablesMD", payload).Result;

                if (create.failed != null)
                    throw new Exception(create.failed.error.message.value);
            }
            else if (table.failed != null)
                throw new Exception(table.failed.error.message.value);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void CreateField(string table, string code, string description, BoFieldTypes type, int size = 0, string defaultValue = null, BoFldSubTypes subType = BoFldSubTypes.st_None, bool mandatory = false, List<UserFieldsMD.ValidValuesMD> fieldValues = null)
    {
        try
        {
            (string success, Erro failed) field = _sL.Get($"UserFieldsMD?$filter=Name eq '{code}' and TableName eq '{table}'").Result;

            if (field.success != null)
            {
                UserFieldsMD.Root objField = JsonSerializer.Deserialize<UserFieldsMD.Root>(field.success);

                if (objField.value.Count == 0)
                {
                    UserFieldsMD.Value body = new UserFieldsMD.Value()
                    {
                        Name = code,
                        Type = type.ToString(),
                        EditSize = size,
                        Description = description,
                        SubType = subType.ToString(),
                        DefaultValue = defaultValue,
                        TableName = table,
                        Mandatory = mandatory ? "tYES" : "tNO",
                        ValidValuesMD = fieldValues
                    };

                    (string success, Erro failed) create = _sL.Post("UserFieldsMD", JsonSerializer.Serialize(body)).Result;

                    if (create.failed != null)
                        throw new Exception(create.failed.error.message.value);
                }
            }
            else
                throw new Exception(field.failed.error.message.value);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private enum BoFieldTypes
    {
        db_Alpha = 0,
        db_Memo = 1,
        db_Numeric = 2,
        db_Date = 3,
        db_Float = 4
    }

    private enum BoFldSubTypes
    {
        st_None = 0,
        st_Phone = 35,
        st_Percentage = 37,
        st_Address = 63,
        st_Link = 66,
        st_Image = 73,
        st_Measurement = 77,
        st_Price = 80,
        st_Quantity = 81,
        st_Rate = 82,
        st_Sum = 83,
        st_Time = 84
    }

    private enum BoUTBTableType
    {
        bott_NoObject = 0,
        bott_MasterData = 1,
        bott_MasterDataLines = 2,
        bott_Document = 3,
        bott_DocumentLines = 4,
        bott_NoObjectAutoIncrement = 5
    }
}
