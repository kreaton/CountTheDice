namespace CountTheDice.Game;

public sealed record GameLevel(int Number, string Name, int DiceCount, int MinChoice, int MaxChoice);

public sealed class DiceGame
{
    public const int ChoiceCount = 10;

    public static readonly IReadOnlyList<GameLevel> Levels =
    [
        new(1, "Niveau 1", DiceCount: 2, MinChoice: 2, MaxChoice: 12),
        new(2, "Niveau 2", DiceCount: 3, MinChoice: 2, MaxChoice: 18),
        new(3, "Niveau 3", DiceCount: 3, MinChoice: 2, MaxChoice: 24),
    ];

    private readonly Random _random;

    public DiceGame(Random? random = null)
    {
        _random = random ?? Random.Shared;
        Level = Levels[0];
    }

    public GameLevel Level { get; private set; }
    public int[] Dice { get; private set; } = [];
    public int[] Choices { get; private set; } = [];
    public int Sum => Dice.Sum();

    public void SetLevel(GameLevel level)
    {
        Level = level;
        NewRound();
    }

    public void NewRound()
    {
        Dice = RollDice();
        Choices = BuildChoices(Sum);
    }

    public int[] RollDice() =>
        Enumerable.Range(0, Level.DiceCount).Select(_ => _random.Next(1, 7)).ToArray();

    public bool IsCorrect(int choice) => choice == Sum;

    private int[] BuildChoices(int sum)
    {
        var wrong = Enumerable.Range(Level.MinChoice, Level.MaxChoice - Level.MinChoice + 1)
            .Where(n => n != sum)
            .ToArray();
        _random.Shuffle(wrong);

        var choices = wrong.Take(ChoiceCount - 1).Append(sum).ToArray();
        _random.Shuffle(choices);
        return choices;
    }
}
