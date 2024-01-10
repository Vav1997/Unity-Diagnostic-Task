[System.Serializable]
public class Fact
{
    public FactStatus factStatus;
    public string _id;
    public string updatedAt;
    public string createdAt;
    public string user;
    public string text;
    public bool deleted;
    public string type;
    public string source;
    public int __v;
    public bool used;

    [System.Serializable]
    public class FactStatus
    {
        public bool verified;
        public int sentCount;
    }
}