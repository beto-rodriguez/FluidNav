using Sample.ViewModels;

namespace Sample.Data;

public static class DataAccessLayer
{
    public static UserVM[] Users { get; } =
        Enumerable
            .Range(0, 1000)
            .Select(i => new UserVM { Id = i, Name = $"Artist {i}" })
            .ToArray();
}
