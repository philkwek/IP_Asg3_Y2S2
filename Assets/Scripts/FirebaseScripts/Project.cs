using System;

public class Project
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

    public Project(int companyId, string creator, string dateCreated,
        string[] furnitureUsed, string houseType, string nameOfLayout,
        string noOfBedrooms, string[] noOfPictures)
    {
        this.companyId = companyId;
        this.creator = creator;
        this.dateCreated = dateCreated;
        this.furnitureUsed = furnitureUsed;
        this.houseType = houseType;
        this.nameOfLayout = nameOfLayout;
        this.noOfBedrooms = noOfBedrooms;
        this.pictures = noOfPictures;
    }
}
