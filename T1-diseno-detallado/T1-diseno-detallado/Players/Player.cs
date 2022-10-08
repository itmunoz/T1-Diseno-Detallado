namespace T1_diseno_detallado;

public class Player
{
    public List<Card> Hand { get; set; }
    public SuperStarCard Superstar { get; set; }
    public List<Card> Arsenal { get; set; }
    public List<Card> Ringside { get; set; }
    public List<Card> RingArea { get; set; }
    public bool IsFirst { get; set; }

    public Player()
    {
        Hand = new List<Card>();
        Ringside = new List<Card>();
        RingArea = new List<Card>();
    }

    public int GetFortitude()
    {
        int fortitude = 0;
        foreach (var card in RingArea)
        {
            card.Fortitude += fortitude;
        }
        return fortitude;
    }
}
