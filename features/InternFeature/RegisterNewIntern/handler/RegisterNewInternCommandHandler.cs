using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LMS___Mini_Version.Feature.InternFeature.Commands
{
    public class RegisterNewInternCommandHandler : IRequestHandler<RegisterNewInternCommand, RequestResponse<int>>
    {
         private readonly IGeneralRepository<Intern> _repository;
        private readonly IGeneralRepository<Track> _repositoryTrack;
        private readonly IUnitOfWork _unitOfWork;


        public RegisterNewInternCommandHandler(IGeneralRepository<Intern> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork= unitOfWork;
        }

        public async Task<RequestResponse<int>> Handle(RegisterNewInternCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var Errors = new Dictionary<string, string[]>();


                if (string.IsNullOrWhiteSpace(request.FullName))
                    Errors.Add("FullName", ["FullName is required."]);

                if (string.IsNullOrWhiteSpace(request.Email))
                    Errors.Add("Email", ["Email is required."]);

                if (request.TrackId <= 0)
                    Errors.Add("TrackId", ["TrackId must be greater than zero."]);

                var track = _repository.GetAll().Where(t => t.Id == request.TrackId && t.Track.IsActive).Select(t => new { t.Id, TrackName = t.Track.Name }).FirstOrDefault();
                if (track?.Id == 0)
                    Errors.Add("TrackId", ["The requested track does not exist or Not Active."]);

                var normalizedEmail = request.Email.Trim();
                if (_repository.GetAll().Where(i => i.Email != null && i.Email.Trim().Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase)).Any())
                    Errors.Add("Email", ["This email is already registered for another intern."]);

                if (Errors.Count == 0)
                {
                    var intern = new Intern
                    {
                        FullName = request.FullName.Trim(),
                        Email = normalizedEmail,
                        BirthYear = request.BirthYear,
                        Status = "Pending",
                        TrackId = request.TrackId
                    };


                    _repository.Add(intern);

                    await _unitOfWork.SaveChangesAsync();

                    return RequestResponse<int>.OK(intern.Id, 201);

                }
                else
                {

                    return RequestResponse<int>.Fail(400, Errors);
                }
            }
            catch (Exception ex)
            {

                return RequestResponse<int>.Fail(400, new Dictionary<string, string[]>() { { "Exception", new[] { ex.Message } } });
            }
           
        }
    }
}
