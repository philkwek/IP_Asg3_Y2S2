using System;
using System.Collections.Generic;

[System.Serializable]
public class AllProject
{
    public List<ProjectData> projects = new List<ProjectData>();
}

[System.Serializable]
public class ProjectData
{
    public int companyId;
    public string creator;
    public string dateCreated;
    public string[] furnitureUsed;
    public string houseType;
    public string[] likes;
    public string nameOfLayout;
    public string noOfBedrooms;
    public string[] pictures;
}
