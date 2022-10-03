using System.Text.Json.Serialization;

namespace T1_diseno_detallado;

public class Card
{
    public string Title  { get ; private set ; }
    public string[] Types  { get ; private set ; }
    public string[] Subtypes  { get ; private set ; }
    public string Fortitude  { get ; private set ; }
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
}
