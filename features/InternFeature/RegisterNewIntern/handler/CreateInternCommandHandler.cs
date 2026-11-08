using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Persistence;
using MediatR;

namespace LMS___Mini_Version.Feature.InternFeature.Commands
{
    public class RegisterNewInternCommandHandler : IRequestHandler<RegisterNewInternCommand, RequestResponse>
    {
         private readonly IGeneralRepository<Track> _repository;
         private readonly IUnitOfWork _unitOfWork;


        public RegisterNewInternCommandHandler(IGeneralRepository<Intern> repository, IUnitOfWork unitOfWork)
        {
            _context = context;
        }

        public Task<RequestResponse> Handle(RegisterNewInternCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                throw new ArgumentException("Full name is required.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");

            if (request.TrackId <= 0)
                throw new ArgumentException("TrackId must be greater than zero.");

            var normalizedEmail = request.Email.Trim();
            if (_context.Interns.Any(i => i.Email != null && i.Email.Trim().Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("This email is already registered for another intern.");

            var track = _context.Tracks.FirstOrDefault(t => t.Id == request.TrackId);
            if (track == null)
                throw new KeyNotFoundException("The requested track does not exist.");

            if (!track.IsActive)
                throw new InvalidOperationException("The requested track is inactive and cannot accept new applicants.");

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

            return RequestResponse.OK();
           
        }
    }
}
