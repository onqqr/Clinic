using Clinic.Offices.Domain.Entities;
using Clinic.Offices.Domain.Enums;

namespace Clinic.Offices.Tests.Unit;

public class OfficeTests
{
    [Fact]
    public void Address_WithOfficeNumber_IncludesOfficeNumber()
    {
        // Arrange
        var office = new Office
        {
            City = "Minsk",
            Street = "Lenina",
            HouseNumber = "10",
            OfficeNumber = "5"
        };
        var expectedAddress = "Minsk, Lenina, 10, office 5";

        // Act
        var actualAddress = office.Address;

        // Assert
        Assert.Equal(expectedAddress, actualAddress);
    }

    [Fact]
    public void Address_WithoutOfficeNumber_ExcludesOfficeNumber()
    {
        // Arrange
        var office = new Office
        {
            City = "Minsk",
            Street = "Lenina",
            HouseNumber = "10"
        };
        var expectedAddress = "Minsk, Lenina, 10";

        // Act
        var actualAddress = office.Address;

        // Assert
        Assert.Equal(expectedAddress, actualAddress);
    }

    [Fact]
    public void NewOffice_Default_HasActiveStatus()
    {
        // Arrange
        var office = new Office();

        // Act
        var actualStatus = office.Status;

        // Assert
        Assert.Equal(OfficeStatus.Active, actualStatus);
    }
}
