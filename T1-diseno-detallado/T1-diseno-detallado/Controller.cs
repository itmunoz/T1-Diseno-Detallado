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
        Console.WriteLine("\nEn este minuto no tienes suficientes masos");
        Console.WriteLine("Por favor agrega más masos.");
    }

    private static void AskUserForDecks()
    {
        _deck1 = AskForDecks();
        _deck2 = AskForDecks();
        
        Console.WriteLine(_deck1[0]);
        Console.WriteLine("########################");
        Console.WriteLine(_deck2[0]);

        VerifySuperstar(_deck1);
        VerifySuperstar(_deck2);

        // List<Card> TrueDeck1 = CreateDeck(_deck1);
        // List<Card> TrueDeck2 = CreateDeck(_deck2);

        // bool isDeck1Valid = VerifyDeck(_deck1);
        // bool isDeck2Valid = VerifyDeck(_deck2);
    }

    private static bool VerifySuperstar(string[] deck)
    {
        string SuperStar = deck[0];
        
        
        return true;
    }

    private static List<Card> CreateDeck(string deck)
    {
        List<Card> TrueDeck = new List<Card>();

        return TrueDeck;
    }

    private static bool VerifyDeck(string deck)
    {
        
        return true;
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
        superStarsList.Add(new SuperStarCard("KANE", "KANE (SuperstarCard)", 7, 2, "At the start of your turn, before your draw segment, opponent must take the top card from his Arsenal and place it into his Ringside pile."));
        superStarsList.Add(new SuperStarCard("CHRIS JERICHO", "CHRIS JERICHO, (Superstar Card)", 7, 3, "Once during your turn, you may discard a card from your hand to force your opponent to discard a card from his hand."));
        return superStarsList;
    }
}