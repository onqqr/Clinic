using Moq;
using OfficeApi.Application.DTOs;
using OfficeApi.Application.Interfaces;
using OfficeApi.Application.Services;
using OfficeApi.Domain.Entities;
using OfficeApi.Domain.Enums;

namespace OfficeApi.Tests.Unit;

public class OfficeServiceTests
{
    [Fact]
    public async Task CreateAsync_ValidRequest_CreatesActiveOffice()
    {
        // Arrange
        var repository = new Mock<IOfficeRepository>();
        var service = new OfficeService(repository.Object);
        var request = new CreateOfficeRequestDto
        {
            City = "Minsk",
            Street = "Lenina",
            HouseNumber = "10",
            OfficeNumber = "5",
            RegistryPhoneNumber = "+375291234567"
        };

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Minsk, Lenina, 10, office 5", result.Address);
        Assert.Equal(OfficeStatus.Active, result.Status);
        repository.Verify(
            x => x.AddAsync(It.Is<Office>(office =>
                office.Id == result.Id &&
                office.City == "Minsk" &&
                office.Status == OfficeStatus.Active)),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingOffice_UpdatesOfficeData()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var office = new Office
        {
            Id = officeId,
            City = "Minsk",
            Street = "Lenina",
            HouseNumber = "10",
            RegistryPhoneNumber = "+375291234567",
            Status = OfficeStatus.Active
        };
        var repository = new Mock<IOfficeRepository>();
        repository
            .Setup(x => x.GetByIdAsync(officeId))
            .ReturnsAsync(office);
        var service = new OfficeService(repository.Object);
        var request = new UpdateOfficeRequestDto
        {
            City = "Grodno",
            Street = "Sovetskaya",
            HouseNumber = "1",
            OfficeNumber = "2",
            RegistryPhoneNumber = "+375291111111"
        };

        // Act
        var result = await service.UpdateAsync(officeId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Grodno, Sovetskaya, 1, office 2", result.Address);
        Assert.Equal("+375291111111", result.RegistryPhoneNumber);
        repository.Verify(
            x => x.UpdateAsync(office),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_OfficeDoesNotExist_ReturnsNull()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var repository = new Mock<IOfficeRepository>();
        repository
            .Setup(x => x.GetByIdAsync(officeId))
            .ReturnsAsync((Office?)null);
        var service = new OfficeService(repository.Object);
        var request = new UpdateOfficeRequestDto
        {
            City = "Grodno",
            Street = "Sovetskaya",
            HouseNumber = "1",
            RegistryPhoneNumber = "+375291111111"
        };

        // Act
        var result = await service.UpdateAsync(officeId, request);

        // Assert
        Assert.Null(result);
        repository.Verify(
            x => x.UpdateAsync(It.IsAny<Office>()),
            Times.Never);
    }

    [Fact]
    public async Task ChangeStatusAsync_ExistingOffice_UpdatesStatus()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var office = new Office
        {
            Id = officeId,
            City = "Minsk",
            Street = "Lenina",
            HouseNumber = "10",
            RegistryPhoneNumber = "+375291234567",
            Status = OfficeStatus.Active
        };
        var repository = new Mock<IOfficeRepository>();
        repository
            .Setup(x => x.GetByIdAsync(officeId))
            .ReturnsAsync(office);
        var service = new OfficeService(repository.Object);
        var request = new ChangeOfficeStatusRequestDto
        {
            Status = OfficeStatus.Inactive
        };

        // Act
        var result = await service.ChangeStatusAsync(officeId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(OfficeStatus.Inactive, result.Status);
        Assert.Equal(OfficeStatus.Inactive, office.Status);
        repository.Verify(
            x => x.UpdateAsync(office),
            Times.Once);
    }

    [Fact]
    public async Task ChangeStatusAsync_OfficeDoesNotExist_ReturnsNull()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var repository = new Mock<IOfficeRepository>();
        repository
            .Setup(x => x.GetByIdAsync(officeId))
            .ReturnsAsync((Office?)null);
        var service = new OfficeService(repository.Object);
        var request = new ChangeOfficeStatusRequestDto
        {
            Status = OfficeStatus.Inactive
        };

        // Act
        var result = await service.ChangeStatusAsync(officeId, request);

        // Assert
        Assert.Null(result);
        repository.Verify(
            x => x.UpdateAsync(It.IsAny<Office>()),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ExistingOffice_DeletesOffice()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var office = new Office
        {
            Id = officeId,
            City = "Minsk",
            Street = "Lenina",
            HouseNumber = "10",
            RegistryPhoneNumber = "+375291234567"
        };
        var repository = new Mock<IOfficeRepository>();
        repository
            .Setup(x => x.GetByIdAsync(officeId))
            .ReturnsAsync(office);
        var service = new OfficeService(repository.Object);

        // Act
        var result = await service.DeleteAsync(officeId);

        // Assert
        Assert.True(result);
        repository.Verify(
            x => x.DeleteAsync(office),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_OfficeDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var officeId = Guid.NewGuid();
        var repository = new Mock<IOfficeRepository>();
        repository
            .Setup(x => x.GetByIdAsync(officeId))
            .ReturnsAsync((Office?)null);
        var service = new OfficeService(repository.Object);

        // Act
        var result = await service.DeleteAsync(officeId);

        // Assert
        Assert.False(result);
        repository.Verify(
            x => x.DeleteAsync(It.IsAny<Office>()),
            Times.Never);
    }
}
