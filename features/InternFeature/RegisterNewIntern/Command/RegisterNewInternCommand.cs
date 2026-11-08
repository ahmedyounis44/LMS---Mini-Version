using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.Feature.InternFeature.Commands
{
    public record RegisterNewInternCommand(string FullName , string Email, int BirthYear ,int TrackId) : IRequest<RequestResponse<int>>;
}
