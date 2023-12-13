using Sample.ViewModels;

namespace Sample.Data;

public class DataAccessLayer
{
    public UserVM[] Users { get; } =
        Enumerable
            .Range(0, 100)
            .Select(i => new UserVM { Id = i, Name = $"Artist {i}" })
            .ToArray();
}
