namespace T1_diseno_detallado;

public class Player
{
    public List<Card> Hand;
    public SuperStarCard Superstar;
    public List<Card> Arsenal { get; set; }
    public List<Card> Ringside;
    public List<Card> RingArea;

    public Player(SuperStarCard superstar)
    {
        Superstar = superstar;
    }
}
