namespace T1_diseno_detallado;

using System.Text.Json;

public class Controller
{
    private static List<string> _decksPath;
    private static List<Card> CardsList;
    private static string[] _deck1;
    private static string[] _deck2;
    private static List<SuperStarCard> SuperStarCardsList;
    private static Player Player1;
    private static Player Player2;
    
    public static void Run()
    {
        CreateFirstObjects();
        
        Console.WriteLine("----------------------");
        Console.WriteLine("Bienvenido!!!");
        Console.WriteLine("Elige alguno de estos mazos:");

        _decksPath = GetDeckFiles();
        if (_decksPath.Count > 1)
            AskUserForDecks();
        else
            TellUserToAddMoreDecks();
        Console.WriteLine("\nQue tengas un buen día :)");

    }

    private static void TellUserToAddMoreDecks()
    {
        Console.WriteLine("\nEn este minuto no tienes suficientes mazos");
        Console.WriteLine("Por favor agrega más masos.");
    }

    private static void AskUserForDecks()
    {
        _deck1 = AskForDecks();
        _deck2 = AskForDecks();

        AssignSuperstar(_deck1, Player1);
        AssignSuperstar(_deck2, Player2);

        List<Card> TrueDeck1 = CreateDeck(_deck1);
        List<Card> TrueDeck2 = CreateDeck(_deck2);

        foreach (var VARIABLE in TrueDeck1)
        {
            Console.WriteLine(VARIABLE.Damage);
        }

        bool isDeck1Valid = VerifyDeck(TrueDeck1);
        bool isDeck2Valid = VerifyDeck(TrueDeck2);
    }
    
    private static bool VerifyDeck(List<Card> deck)
    {
        
        return true;
    }
    
    private static List<Card> CreateDeck(string[] deck)
    {
        List<Card> TrueDeck = new List<Card>();

        for (int i = 1; i < deck.Length; i++)
        {
            int cardQuantityInt;
            string currentCard = deck[i];
            string cardName = currentCard.Substring(2);
            string cardQuantityString = currentCard.Substring(0, 1);
            bool isCardParsable = int.TryParse(cardQuantityString, out cardQuantityInt);
            
            for (int j = 0; j < CardsList.Count; j++)
            {
                if (CardsList[j].Title == cardName)
                {
                    for (int k = 0; k < cardQuantityInt; k++)
                    {
                        TrueDeck.Add(new Card(CardsList[j].Title, CardsList[j].Types, CardsList[j].Subtypes, CardsList[j].Fortitude, CardsList[j].Damage, CardsList[j].StunValue, CardsList[j].CardEffect));
                    }
                }
            }
        }
        return TrueDeck;
    }

    private static void VerifySuperstar(Player player)
    {
        if (player.Superstar is null)
        {
            Console.WriteLine("No se encontró una superestrella válida en este mazo");
            Environment.Exit(0);
        }
    }

    private static void AssignSuperstar(string[] deck, Player player)
    {
        string SuperStar = deck[0];
        for (int i = 0; i < SuperStarCardsList.Count; i++)
        {
            if (SuperStarCardsList[i].file_name == SuperStar)
            {
                player.Superstar = SuperStarCardsList[i];
                break;
            }
        }
        VerifySuperstar(player);
    }

    private static string[] AskForDecks()
    {
        ShowDecksPaths();
        int deckId = AskForNumber(0, _decksPath.Count - 1);
        string deckPath = _decksPath[deckId];
        string[] text = File.ReadAllLines(deckPath);
        _decksPath.RemoveAt(deckId);
        
        
        return text;
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

    private static void ShowDecksPaths()
    {
        for (int i = 0; i < _decksPath.Count; i++)
            Console.WriteLine(i + "- " + _decksPath[i]);
        Console.WriteLine("Ingresa un número entre " + 0 + " y " + (_decksPath.Count - 1));
    }

    private static List<string> GetDeckFiles()
    {
        List<string> decksPath = new List<string>();
        foreach (string file in Directory.GetFiles("Tests_v5/decks"))
            if (file.EndsWith(".txt"))
                decksPath.Add(file);

        return decksPath;
    }
    private static void CreateFirstObjects()
    {
        CardsList = LoadCards();
        SuperStarCardsList = CreateSuperStars();
        Player1 = new Player();
        Player2 = new Player();
    }
    
    private static List<Card> LoadCards() {
        string fileName = "cartas_v7/cards.json";
        string jsonString = File.ReadAllText(fileName);
        List<Card> cardsList = JsonSerializer.Deserialize<List<Card>>(jsonString);
        return cardsList;
    }
    
    private static List<SuperStarCard> CreateSuperStars()
    {
        List<SuperStarCard> superStarsList = new List<SuperStarCard>();
        superStarsList.Add(new SuperStarCard("HHH", "HHH (Superstar Card)", 10, 3, "None, isn't the starting hand size enough! He is 'The Game' after all!"));
        superStarsList.Add(new SuperStarCard("STONE COLD STEVE AUSTIN", "STONE COLD STEVE AUSTIN (Superstar Card)", 7, 5, "Once during your turn, you may draw a card, but you must then take a card from your hand and place it on the bottom of your Arsenal."));
        superStarsList.Add(new SuperStarCard("THE UNDERTAKER", "THE UNDERTAKER (Superstar Card)", 6, 4, "Once during your turn, you may discard 2 cards to the Ringside pile and take 1 card from the Ringside pile and place it into your hand."));
        superStarsList.Add(new SuperStarCard("MANKIND", "MANKIND (Superstar Card)", 2, 4, "You must always draw 2 cards, if possible, during your draw segment. All damage from opponent is at -1D."));
        superStarsList.Add(new SuperStarCard("THE ROCK", "THE ROCK (Superstar Card)", 5, 5, "At the start of your turn, before your draw segment, you may take 1 card from your Ringside pile and place it on the bottom of your Arsenal."));
        superStarsList.Add(new SuperStarCard("KANE", "KANE (Superstar Card)", 7, 2, "At the start of your turn, before your draw segment, opponent must take the top card from his Arsenal and place it into his Ringside pile."));
        superStarsList.Add(new SuperStarCard("CHRIS JERICHO", "CHRIS JERICHO (Superstar Card)", 7, 3, "Once during your turn, you may discard a card from your hand to force your opponent to discard a card from his hand."));
        return superStarsList;
    }
}