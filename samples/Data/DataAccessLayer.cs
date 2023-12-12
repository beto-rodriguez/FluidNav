using Sample.ViewModels;

namespace Sample.Data;

public static class DataAccessLayer
{
    public static UserVM[] Users { get; } =
    [
        new() { Name = "Carla Morrison" },
        new() { Name = "Julieta Venegas" },
        new() { Name = "Mon Laferte" },
        new() { Name = "Lila Downs" },
        new() { Name = "Natalia Lafourcade" },
        new() { Name = "Ximena Sariñana" },
        new() { Name = "Ely Guerra" },
        new() { Name = "Ana Tijoux" },
        new() { Name = "Kali Uchis" },
        new() { Name = "Linn Da Quebrada" },
        new() { Name = "Lido Pimienta" },
        new() { Name = "Nathy Peluso" },
        new() { Name = "Rosalía" },
    ];
}
