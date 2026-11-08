using MediatR;

namespace LMS___Mini_Version.Feature.InternFeature.Commands
{
    public class RegisterNewInternCommand(string FullName ,string   int BirthYear ,int TrackId) : IRequest<RequestResponse>;
}
