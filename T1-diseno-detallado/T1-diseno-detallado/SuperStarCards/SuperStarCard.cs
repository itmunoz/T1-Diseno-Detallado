namespace T1_diseno_detallado;

public class SuperStarCard
{
    public string name { get ; private set ; }
    public string file_name { get ; private set ; }
    public int handSize { get ; private set ; }
    public int superstarValue { get ; private set ; }
    public string ability { get ; private set ; }

    public string cardSubtype { get ; private set ; }
    public bool IsAbilityStartOfTurn { get ; private set ; }
    public int DamageReduction { get ; private set ; }

    public SuperStarCard(string name, string file_name, int handSize, int superstarValue, string ability, string cardSubtype, bool IsAbilityStartOfTurn, int damageReduction)
    {
        this.name = name;
        this.file_name = file_name;
        this.handSize = handSize;
        this.superstarValue = superstarValue;
        this.ability = ability;
        this.cardSubtype = cardSubtype;
        this.IsAbilityStartOfTurn = IsAbilityStartOfTurn;
        this.DamageReduction = damageReduction;
    }

    public void UseAbility(Player player, Player opponent)
    {
        if (this.name == "STONE COLD STEVE AUSTIN")
        {
            StoneColdSuperStarAbility(player, opponent);
        }
        else if (this.name == "CHRIS JERICHO")
        {
            JerichoSuperStarAbility(player, opponent);
        }
        else if (this.name == "THE UNDERTAKER")
        {
            UndertakerSuperStarAbility(player);
        }
    }

    private static void UndertakerSuperStarAbility(Player player)
    {
        if (player.Ringside.Count != 0 && player.Hand.Count >= 2)
        {
            PrintCards(player.Hand);
            Console.WriteLine("Escoge una carta de la mano de " + player.Superstar.name + " para descartar");
            int selectedId1 = AskForNumber(0, player.Hand.Count - 1);
            DiscardCard(player, selectedId1);
            
            PrintCards(player.Hand);
            Console.WriteLine("Escoge otra carta de la mano de " + player.Superstar.name + " para descartar");
            int selectedId2 = AskForNumber(0, player.Hand.Count - 1);
            DiscardCard(player, selectedId2);
            
            RecoverCardToHand(player);
        }
        else
        {
            Console.WriteLine("No se cumplen las condiciones para poder utilizar esta habilidad");
        }
    }
    
    private static void StoneColdSuperStarAbility(Player player, Player opponent)
    {
        Console.WriteLine("Robas una carta");
        DrawCard(player, opponent);
        
        PrintCards(player.Hand);
        Console.WriteLine("Escoge una carta de la mano de " + player.Superstar.name + " para descartar");
        int selectedId = AskForNumber(0, player.Hand.Count - 1);
        DiscardCardToBottomOfArsenal(player, selectedId);
    }
    
    private static void JerichoSuperStarAbility(Player player, Player opponent)
    {
        PrintCards(player.Hand);
        Console.WriteLine("Escoge una carta de la mano de " + player.Superstar.name + " para descartar");
        int selectedIdPlayer = AskForNumber(0, player.Hand.Count - 1);
        DiscardCard(player, selectedIdPlayer);

        PrintCards(opponent.Hand);
        Console.WriteLine("Escoge una carta de la mano de " + opponent.Superstar.name + " para descartar");
        int selectedIdOpponent = AskForNumber(0, opponent.Hand.Count - 1);
        DiscardCard(opponent, selectedIdOpponent);
    }

    private static void DiscardCardToBottomOfArsenal(Player player, int selectedId)
    {
        Card discardedCard = player.Hand[selectedId];
        player.Arsenal.Insert(0, discardedCard);
        player.Hand.RemoveAt(selectedId);
    }

    private static void DiscardCard(Player player, int selectedId)
    {
        Card discardedCard = player.Hand[selectedId];
        player.Ringside.Add(discardedCard);
        player.Hand.RemoveAt(selectedId);
    }
    
    private static void PrintCards(List<Card> cards)
    {
        int currentCardNumber = 0;
        foreach (var card in cards)
        {
            Console.WriteLine("------------- Card #" + currentCardNumber);
            Console.WriteLine("Title: " + card.Title);
            Console.WriteLine("Stats: [" + card.Fortitude + "F/" + card.Damage + "D/" + card.StunValue + "SV]");
            
            string typesString = "";
            foreach (var type in card.Types)
            {
                typesString += (type + ", ");
            }
            Console.WriteLine("Types: " + typesString);
            
            string subtypesString = "";
            foreach (var subtype in card.Subtypes)
            {
                subtypesString += (subtype + ", ");
            }
            Console.WriteLine("Subtypes: " + subtypesString);
            
            Console.WriteLine("Effect: " + card.CardEffect);
            Console.WriteLine("");
            currentCardNumber += 1;
        }
    }
    
    private static int AskForNumber(int minValue, int maxValue)
    {
        int number;
        bool wasParseSuccessful;
        do
        {
            string? userInput = Console.ReadLine();
            wasParseSuccessful = int.TryParse(userInput, out number);
            if (!wasParseSuccessful || number < minValue || number > maxValue)
            {
                Console.WriteLine("Input inválido. Intenta de nuevo");
            }
        } while (!wasParseSuccessful || number < minValue || number > maxValue);

        return number;
    }

    private static void RecoverCardToHand(Player player)
    {
        PrintCards(player.Ringside);
        Console.WriteLine("Ingresa el ID de la carta que quieres recuperar.");
        Console.WriteLine("(Ingresa un número entre 0 y " + (player.Ringside.Count - 1) + ")");
        
        int selectedOption = AskForNumber(0 , player.Ringside.Count - 1);
        
        Card selectedCard = player.Ringside[selectedOption];
        player.Hand.Add(selectedCard);
        player.Ringside.Remove(selectedCard);
        
    }
    
    private static void RecoverCard(Player player)
    {
        PrintCards(player.Ringside);
        Console.WriteLine("Ingresa el ID de la carta que quieres recuperar. Puedes ingresar '-1' para cancelar.");
        Console.WriteLine("(Ingresa un número entre -1 y " + (player.Ringside.Count - 1) + ")");
        
        int selectedOption = AskForNumber(-1 , player.Ringside.Count - 1);

        if (selectedOption != -1)
        {
            Card selectedCard = player.Ringside[selectedOption];
            player.Arsenal.Insert(0, selectedCard);
            player.Ringside.Remove(selectedCard);
        }
    }
    
    private static void DrawCard(Player player, Player opponent)
    {
        if (player.Arsenal.Count == 0)
        {
            AnnounceWinner(opponent, player);
        }
        else
        {
            Card drawnCard = player.Arsenal[player.Arsenal.Count - 1];
            player.Hand.Add(drawnCard);
            player.Arsenal.RemoveAt(player.Arsenal.Count - 1);
        }
    }
    
    private static void AnnounceWinner(Player winner, Player loser)
    {
        Console.WriteLine("#####################");
        Console.WriteLine(loser.Superstar.name + " se ha quedado sin cartas en su Arsenal!");
        Console.WriteLine("El ganador es " + winner.Superstar.name + "!!!!!");
        Environment.Exit(0);
    }
}