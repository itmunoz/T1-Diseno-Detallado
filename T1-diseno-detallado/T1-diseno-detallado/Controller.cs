namespace T1_diseno_detallado;

using System.Text.Json;
using System.Linq;

public class Controller
{
    private static List<string> _decksPath;
    private static List<Card> CardsList;
    private static string[] _deck1;
    private static string[] _deck2;
    private static List<SuperStarCard> SuperStarCardsList;
    private static Player Player1;
    private static Player Player2;
    private static Game Game;

    public static void Run()
    {
        CreateFirstObjects();
        WelcomeMessages();

        _decksPath = GetDeckFiles();
        if (_decksPath.Count > 1)
        {
            AskUserForDecks();
            Game.StartGame(Player1, Player2);
        }
        else
            TellUserToAddMoreDecks();
        Console.WriteLine("\nQue tengas un buen día :)");

    }

    public static void WelcomeMessages()
    {
        Console.WriteLine("----------------------");
        Console.WriteLine("Bienvenido!!!");
        Console.WriteLine("Elige alguno de estos mazos:");
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

        bool isDeck1Valid = VerifyDeck(TrueDeck1, Player1);
        bool isDeck2Valid = VerifyDeck(TrueDeck2, Player2);

        Player1.Arsenal = TrueDeck1;
        Player2.Arsenal = TrueDeck2;
    }

    private static bool VerifyDeck(List<Card> deck, Player player)
    {
        bool isLengthValid = VerifyDeckLength(deck, player);
        bool isCardQuantity = VerifyCardQuantity(deck, player);
        bool isHeelAndFaceOk = VerifyHeelAndFace(deck, player);
        bool isSuperstarCardsOk = VerifyCardBySuperstar(deck, player);
        return (isLengthValid && isCardQuantity && isHeelAndFaceOk && isSuperstarCardsOk);
    }

    private static void DeckErrorMessage(Player player)
    {
        Console.WriteLine("Lo lamento, pero el mazo de " + player.Superstar.name + " es inválido.");
        Environment.Exit(0);
    }

    private static bool VerifyCardBySuperstar(List<Card> deck, Player player)
    {
        bool isSuperstarCardsOk = true;
        string[] superstarIndicator = { "Jericho", "HHH", "Kane", "TheRock", "Mankind", "StoneCold", "Undertaker" };

        foreach (var card in deck)
        {
            foreach (var indicator in superstarIndicator)
            {
                if (card.Subtypes.Contains(indicator))
                {
                    if (player.Superstar.cardSubtype != indicator)
                    {
                        isSuperstarCardsOk = false;
                        DeckErrorMessage(player);
                    }
                }
            }
        }
        return isSuperstarCardsOk;
    }

    private static bool VerifyHeelAndFace(List<Card> deck, Player player)
    {
        bool isHeelAndFaceOk = true;
        int numberOfHeelCards = 0;
        int numberOfFaceCards = 0;

        foreach (var card in deck)
        {
            if (card.Subtypes.Contains("Heel"))
            {
                numberOfHeelCards += 1;
            }

            if (card.Subtypes.Contains("Face"))
            {
                numberOfFaceCards += 1;
            }
        }

        if (numberOfFaceCards != 0 && numberOfHeelCards != 0)
        {
            isHeelAndFaceOk = false;
            DeckErrorMessage(player);
        }
        
        return isHeelAndFaceOk;
    }

    private static bool VerifyCardQuantity(List<Card> deck, Player player)
    {
        bool isEveryCardOk = true;

        foreach (var analysed_card in deck)
        {
            int cardCount = 0;

            foreach (var card in deck)
            {
                if (analysed_card.Title == card.Title)
                {
                    cardCount += 1;
                }
            }
            
            if (analysed_card.Subtypes.Contains("Unique"))
            {
                if (cardCount > 1)
                {
                    isEveryCardOk = false;
                    DeckErrorMessage(player);
                }
            }
            else if (analysed_card.Subtypes.Contains("SetUp"))
            {
                isEveryCardOk = true;
            }
            else
            {
                if (cardCount > 3)
                {
                    isEveryCardOk = false;
                    DeckErrorMessage(player);
                }
            }
        }

        return isEveryCardOk;
    }

    private static bool VerifyDeckLength(List<Card> deck, Player player)
    {
        if (deck.Count == 60)
        {
            return true;
        }
        DeckErrorMessage(player);
        return false;
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
        foreach (string file in Directory.GetFiles("Tests_v9/decks"))
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
        superStarsList.Add(new SuperStarCard("HHH", "HHH (Superstar Card)", 10, 3, "None, isn't the starting hand size enough! He is 'The Game' after all!", "HHH"));
        superStarsList.Add(new SuperStarCard("STONE COLD STEVE AUSTIN", "STONE COLD STEVE AUSTIN (Superstar Card)", 7, 5, "Once during your turn, you may draw a card, but you must then take a card from your hand and place it on the bottom of your Arsenal.", "StoneCold"));
        superStarsList.Add(new SuperStarCard("THE UNDERTAKER", "THE UNDERTAKER (Superstar Card)", 6, 4, "Once during your turn, you may discard 2 cards to the Ringside pile and take 1 card from the Ringside pile and place it into your hand.", "Undertaker"));
        superStarsList.Add(new SuperStarCard("MANKIND", "MANKIND (Superstar Card)", 2, 4, "You must always draw 2 cards, if possible, during your draw segment. All damage from opponent is at -1D.", "Mankind"));
        superStarsList.Add(new SuperStarCard("THE ROCK", "THE ROCK (Superstar Card)", 5, 5, "At the start of your turn, before your draw segment, you may take 1 card from your Ringside pile and place it on the bottom of your Arsenal.", "TheRock"));
        superStarsList.Add(new SuperStarCard("KANE", "KANE (Superstar Card)", 7, 2, "At the start of your turn, before your draw segment, opponent must take the top card from his Arsenal and place it into his Ringside pile.", "Kane"));
        superStarsList.Add(new SuperStarCard("CHRIS JERICHO", "CHRIS JERICHO (Superstar Card)", 7, 3, "Once during your turn, you may discard a card from your hand to force your opponent to discard a card from his hand.", "Jericho"));
        return superStarsList;
    }
}