using System;

public class Company
{
    public int companyId;
    public string companyName;
    public string[] employeeList;

    public Company(int companyId, string companyName, string[] employeeList)
    {
        this.companyId = companyId;
        this.companyName = companyName;
        this.employeeList = employeeList;
    }
}
