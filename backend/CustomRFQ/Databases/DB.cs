using CustomRFQ.Models;

namespace CustomRFQ.Databases;

public abstract class DB
{
    public abstract List<Database.Config> _dbs { get; set; }

    public abstract void LoadConfig();

    public abstract Database.EventSender GetEvent(string guid);

    public abstract IEnumerable<dynamic> Query(string query);
}
