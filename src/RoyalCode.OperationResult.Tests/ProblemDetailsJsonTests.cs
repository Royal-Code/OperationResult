
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;

namespace RoyalCode.OperationResults.Tests;

public class ProblemDetailsJsonTests
{
    [Fact]
    public void LoadJsonFile()
    {
        // Arrange
        ServiceCollection services = new();
        services.AddOptions();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
        services.AddHttpContextAccessor();
        services.AddProblemDetailsDescriptions(options =>
        {
            options.Descriptor.AddFromJsonFile("problem-details.json");
        });

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ProblemDetailsOptions>>().Value;

        // Act
        options.Descriptor.TryGetDescription("insufficient-credits", out var description1);
        options.Descriptor.TryGetDescription("size-out-of-bounds", out var description2);
        options.Descriptor.TryGetDescription("dependencies-not-found", out var description3);

        // Assert
        Assert.NotNull(description1);
        Assert.NotNull(description2);
        Assert.NotNull(description3);

        Assert.Equal("Insufficient credits", description1.Title);
        Assert.Equal("The partner does not have sufficient credits to obtain the required benefit.", description1.Description);
        Assert.Equal("insufficient-credits", description1.Code);
        Assert.Null(description1.Type);
        Assert.Null(description1.Status);

        Assert.Equal("Size out of bounds", description2.Title);
        Assert.Equal("The size of all the items is above the capacity of the container.", description2.Description);
        Assert.Equal("size-out-of-bounds", description2.Code);
        Assert.Equal("https://example.com/probs/size-out-of-bounds", description2.Type);
        Assert.Null(description1.Status);

        Assert.Equal("Dependencies not found", description3.Title);
        Assert.Equal("One or more dependent records were not found", description3.Description);
        Assert.Equal("dependencies-not-found", description3.Code);
        Assert.Null(description3.Type);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, description3.Status);
    }

    [Fact]
    public void ReadJsonFileFromConfigurations()
    {
        // Arrange
        ServiceCollection services = new();
        services.AddOptions();
        services.AddLogging();
        services.AddSingleton<IConfiguration>(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
        services.AddHttpContextAccessor();
        services.AddProblemDetailsDescriptions();

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<ProblemDetailsOptions>>().Value;

        // Act
        options.Descriptor.TryGetDescription("insufficient-credits", out var description1);
        options.Descriptor.TryGetDescription("size-out-of-bounds", out var description2);
        options.Descriptor.TryGetDescription("dependencies-not-found", out var description3);

        // Assert
        Assert.NotNull(description1);
        Assert.NotNull(description2);
        Assert.NotNull(description3);
    }
}
