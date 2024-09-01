using CharmCheck.Domain.Entities;
using CharmCheck.Domain.Interfaces;
using CharmCheck.Domain.Responses;
using MediatR;

namespace CharmCheck.Application.Features.Ratings.Commands.RatePhoto;

internal class RatePhotoCommandHandler : IRequestHandler<RatePhotoCommand, Result>
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Rating> _ratingRepository;

    public RatePhotoCommandHandler(IGenericRepository<Rating> ratingRepository, IGenericRepository<User> userRepository)
    {
        _ratingRepository = ratingRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(RatePhotoCommand request, CancellationToken cancellationToken)
    {
        var rating = new Rating
        {
            PhotoRating = request.PhotoRating,
            ProfileImageId = new(request.PhotoId),
            ReviewerId = request.UserId,
            RatingDate = DateTime.UtcNow
        };

        await _ratingRepository.AddAsync(rating);
        await _ratingRepository.SaveChangesAsync();

        return Result.Success();
    }
}
