namespace day02;

public class Outcome
{
    private OutcomeType OutcomeType { get; }

    private Outcome(OutcomeType outcomeType)
    {
        OutcomeType = outcomeType;
    }

    public static Outcome FromHands(Hand h1, Hand h2)
    {
        var outcomeType = OutcomeType.Loss;
        
        if ((h1.HandType == HandType.Scissors && h2.HandType == HandType.Rock)
            || (h1.HandType == HandType.Paper && h2.HandType == HandType.Scissors)
            || (h1.HandType == HandType.Rock && h2.HandType == HandType.Paper))
        {
            outcomeType = OutcomeType.Win;
        }
        else if (h1.HandType == h2.HandType)
        {
            outcomeType = OutcomeType.Draw;
        }

        return new Outcome(outcomeType);
    }

    public static Outcome FromString(string str)
    {
        var outcomeType = str switch
        {
            "X" => OutcomeType.Loss,
            "Y" => OutcomeType.Draw,
            "Z" => OutcomeType.Win,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
        };

        return new Outcome(outcomeType);
    }

    public int PlayAgainst(Hand other)
    {
        var response = GetResponse(other);
        return GetScore() + response.GetScore();
    }
    
    public int GetScore()
    {
        return OutcomeType switch
        {
            OutcomeType.Win => 6,
            OutcomeType.Draw => 3,
            OutcomeType.Loss => 0,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Hand GetResponse(Hand other)
    {
        if (OutcomeType == OutcomeType.Draw)
        {
            return other;
        }

        return other.HandType switch
        {
            HandType.Rock when OutcomeType == OutcomeType.Win => new Hand(HandType.Paper),
            HandType.Paper when OutcomeType == OutcomeType.Win => new Hand(HandType.Scissors),
            HandType.Scissors when OutcomeType == OutcomeType.Win => new Hand(HandType.Rock),
            HandType.Rock when OutcomeType == OutcomeType.Loss => new Hand(HandType.Scissors),
            HandType.Paper when OutcomeType == OutcomeType.Loss => new Hand(HandType.Rock),
            HandType.Scissors when OutcomeType == OutcomeType.Loss => new Hand(HandType.Paper),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}