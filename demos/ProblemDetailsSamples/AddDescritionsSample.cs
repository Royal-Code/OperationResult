
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using RoyalCode.OperationResults;

public class AddDescriptionsSample
{

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddProblemDetailsDescriptions(options =>
        {
            // add instance of ProblemDetailsDescription
            options.Descriptor.AddMany([
                new ProblemDetailsDescription(
                    code: "insufficient-credits",
                    title: "Insufficient credits",
                    description: "The partner does not have sufficient credits to obtain the required benefit."
                ),
                new ProblemDetailsDescription(
                    code: "size-out-of-bounds",
                    type: "https://example.com/probs/size-out-of-bounds",
                    title: "Size out of bounds",
                    description: "The size of all the items is above the capacity of the container."
                ),
                new ProblemDetailsDescription(
                    code: "dependencies-not-found",
                    title: "Dependencies not found",
                    description: "One or more dependent records were not found",
                    status: HttpStatusCode.UnprocessableEntity
                )
            ]);

            // add json file path
            options.Descriptor.AddFromJsonFile("problem-details.json");
        });
    }

}