namespace CharmCheck.Application.DTOs;

public record PhotoAnalyticsEntry(
    string PhotoId,
    string PhotoUrl,
    Dictionary<int, Dictionary<int, int>> Ratings);
