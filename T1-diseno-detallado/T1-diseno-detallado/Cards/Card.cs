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
    public int TemporaryDamage { get ; set ; }

    public Card(string title, string[] types, string[] subtypes, string fortitude, string damage, string stunValue, string cardEffect)
    {
        Title = title;
        Types = types;
        Subtypes = subtypes;
        Fortitude = fortitude;
        Damage = damage;
        StunValue = stunValue;
        CardEffect = cardEffect;
        TemporaryDamage = 0;
    }

    public int GetFortitudeInt()
    {
        
        return Int32.Parse(this.Fortitude);
    }

    public int GetDamageInt()
    {
        return Int32.Parse(this.Damage);
    }

    public int SpecialDamage(Card reversedCard)
    {
        int totalDamage = 0;
        if (this.Title == "Knee to the Gut" || this.Title == "Rolling Takedown")
        {
            totalDamage += reversedCard.GetDamageInt();
        }
        return totalDamage;
    }
    
    public int GetStunValueInt()
    {
        return Int32.Parse(this.StunValue);
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
        else if (this.Title == "Elbow to the Face" && playedCard.GetDamageInt() <= 7 && playedCard.Types.Contains("Maneuver"))
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Ensugiri" && playedCard.Title == "Kick")
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Drop Kick" && playedCard.Title == "Drop Kick")
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Belly to Belly Suplex" && playedCard.Title == "Belly to Belly Suplex")
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Vertical Suplex" && playedCard.Title == "Vertical Suplex")
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Belly to Back Suplex" && playedCard.Title == "Belly to Back Suplex")
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Rolling Takedown" && playedCard.Subtypes.Contains("Grapple") && playedCard.GetDamageInt() <= 7)
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Knee to the Gut" && playedCard.Subtypes.Contains("Strike") && playedCard.GetDamageInt() <= 7)
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Manager Interferes" && playedCard.Types.Contains("Maneuver"))
        {
            isUsefulReversal = true;
        }
        else if (this.Title == "Clean Break" && playedCard.Title == "Jockeying for Position")
        {
            isUsefulReversal = true;
        }

        return isUsefulReversal;
    }

    public void UseCardAbility(Player player, Player opponent)
    {
        if (this.Title == "Manager Interferes" || this.Title == "Double Leg Takedown" || this.Title == "Reverse DDT")
        {
            Game.DrawCard(player, opponent);
        }
        else if (this.Title == "Headlock Takedown" || this.Title == "Standing Side Headlock")
        {
            Game.DrawCard(opponent, player);
        }
        else if (this.Title == "Clean Break")
        {
            for (int i = 0; i < 4; i++)
            {
                Game.DiscardCardMenu(opponent, i);
            }
            Game.DrawCard(player, opponent);
        }
        else if (this.Title == "Fireman's Carry" || this.Title == "Whaddya Got?")
        {
            Console.WriteLine("");
            Console.WriteLine("Las cartas de " + opponent.Superstar.name + " son las siguientes:");
            Game.PrintCards(opponent.Hand);
        }
        else if (this.Title == "Running Elbow Smash" || this.Title == "Kick")
        {
            Game.ReceiveOneDamage(player);
        }
        // Jugador descarta una carta
        else if (this.Title == "Head Butt" || this.Title == "Arm Drag" || this.Title == "Arm Bar")
        {
            Game.DiscardCardMenu(player, 1);
        }
        // Oponente descarta una carta
        else if (this.Title == "Figure Four Leg Lock" || 
                 this.Title == "Ankle Lock" || 
                 this.Title == "Samoan Drop" || 
                 this.Title == "Bear Hug" || 
                 this.Title == "Spinning Heel Kick" || 
                 this.Title == "Choke Hold" || 
                 this.Title == "Boston Crab" ||
                 this.Title == "Torture Rack")
        {
            Game.DiscardCardMenu(opponent, 1);
        }
        else if (this.Title == "Spit At Opponent")
        {
            Game.DiscardCardMenu(player, 1);
            for (int i = 4; i > 0; i--)
            {
                Game.DiscardCardMenu(opponent, i);
            }
        }
        else if (this.Title == "Offer Handshake")
        {
            for (int i = 0; i < 3; i++)
            {
                Game.DrawCard(player, opponent);
            }
            Game.DiscardCardMenu(player, 1);
        }
        else if (this.Title == "Roll Out of the Ring")
        {
            for (int i = 2; i > 0; i--)
            {
                Game.DiscardCardMenu(player, i);
            }
            for (int i = 2; i > 0; i--)
            {
                Game.RecoverCardToHand(player);
            }
        }
        else if (this.Title == "Fisherman's Suplex")
        {
            Game.ReceiveOneDamage(player);
            Game.DrawCard(player, opponent);
        }
        else if (this.Title == "Sleeper" || this.Title == "Camel Clutch")
        {
            Game.SearchForCardInArsenal(player, "Maintain Hold");
            Game.ShuffleCards(player.Arsenal);
        }
        else if (this.Title == "Not Yet")
        {
            Game.PassCardToArsenalFromHand(player);
            for (int i = 0; i < 2; i++)
            {
                Game.DrawCard(player, opponent);
            }
        }
        else if (this.Title == "Recovery")
        {
            for (int i = 0; i < 2; i++)
            {
                Game.PassCardToArsenalFromRingside(player);
            }
            Game.DrawCard(player, opponent);
        }
        else if (this.Title == "Puppies! Puppies!")
        {
            for (int i = 0; i < 5; i++)
            {
                Game.PassCardToArsenalFromRingside(player);
            }
            for (int i = 0; i < 2; i++)
            {
                Game.DrawCard(player, opponent);
            }
        }
        else if (this.Title == "Chicken Wing")
        {
            for (int i = 0; i < 2; i++)
            {
                Game.PassCardToArsenalFromRingside(player);
            }
        }
        else if (this.Title == "Pump Handle Slam")
        {
            for (int i = 2; i > 0; i--)
            {
                Game.DiscardCardMenu(player, i);
            }
        }
    }
}
