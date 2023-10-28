using CustomRFQ.Models;

namespace CustomRFQ.Databases;

public class HANA : DB
{
    public override List<Database.Config> _dbs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public override Database.Smtp _smtp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override void LoadConfig()
    {
        throw new NotImplementedException();
    }
    public override Database.EventSender GetEvent(string guid)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<dynamic> Query(string query)
    {
        throw new NotImplementedException();
    }
}
