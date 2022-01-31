using System;

public class User
{
    public string email;
    public string username;
    public string databaseId;
    public int companyId;

    public User(string email, string username, string databaseId)
    {
        this.email = email;
        this.username = username;
        this.companyId = 0;
        this.databaseId = databaseId;
    }
}
