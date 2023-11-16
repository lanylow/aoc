namespace day02;

public class Hand
{
    public HandType HandType { get; }

    public Hand(HandType handType)
    {
        HandType = handType;
    }
    
    public static Hand FromString(string str)
    {
        var handType = str switch
        {
            "A" => HandType.Rock,
            "B" => HandType.Paper,
            "C" => HandType.Scissors,
            
            "X" => HandType.Rock,
            "Y" => HandType.Paper,
            "Z" => HandType.Scissors,
            
            _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
        };

        return new Hand(handType);
    }

    public int PlayAgainst(Hand other)
    {
        var outcome = Outcome.FromHands(other, this);
        return outcome.GetScore() + GetScore();
    }

    public int GetScore()
    {
        return HandType switch
        {
            HandType.Rock => 1,
            HandType.Paper => 2,
            HandType.Scissors => 3,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}