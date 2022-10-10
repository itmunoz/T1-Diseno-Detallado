using System.Text.Json.Serialization;
using System.Linq;


namespace T1_diseno_detallado;

public class Card
{
    public string Title  { get ; private set ; }
    public string[] Types  { get ; private set ; }
    public string[] Subtypes  { get ; private set ; }
    public string Fortitude  { get ; set ; }
    public string Damage  { get ; private set ; }
    public string StunValue  { get ; private set ; }
    public string CardEffect  { get ; private set ; }
    
    public Card(string title, string[] types, string[] subtypes, string fortitude, string damage, string stunValue, string cardEffect)
    {
        Title = title;
        Types = types;
        Subtypes = subtypes;
        Fortitude = fortitude;
        Damage = damage;
        StunValue = stunValue;
        CardEffect = cardEffect;
    }

    public int GetFortitudeInt()
    {
        return Int32.Parse(this.Fortitude);
    }

    public int GetDamageInt()
    {
        return Int32.Parse(this.Damage);
    }

    public bool CheckReversal(Card playedCard)
    {
        bool isUsefulReversal = false;

        if (this.Subtypes.Contains("ReversalSubmission") && playedCard.Subtypes.Contains("Submission"))
        {
            isUsefulReversal = true;
        }
        else if (this.Subtypes.Contains("ReversalGrapple") && playedCard.Subtypes.Contains("Grapple"))
        {
            isUsefulReversal = true;
        }
        else if (this.Subtypes.Contains("ReversalStrike") && playedCard.Subtypes.Contains("Strike"))
        {
            isUsefulReversal = true;
        }
        else if (this.Subtypes.Contains("ReversalAction") && playedCard.Types.Contains("Action"))
        {
            isUsefulReversal = true;
        }

        return isUsefulReversal;
    }
}
