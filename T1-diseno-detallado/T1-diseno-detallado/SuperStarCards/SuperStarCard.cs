namespace T1_diseno_detallado;

public class SuperStarCard
{
    public string name { get ; private set ; }
    public string file_name { get ; private set ; }
    public int handSize { get ; private set ; }
    public int superstarValue { get ; private set ; }
    public string ability { get ; private set ; }

    public string cardSubtype { get ; private set ; }

    public SuperStarCard(string name, string file_name, int handSize, int superstarValue, string ability, string cardSubtype)
    {
        this.name = name;
        this.file_name = file_name;
        this.handSize = handSize;
        this.superstarValue = superstarValue;
        this.ability = ability;
        this.cardSubtype = cardSubtype;
    }
}