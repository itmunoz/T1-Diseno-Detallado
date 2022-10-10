namespace T1_diseno_detallado;

public class Game
{
    private static Player Player1;
    private static Player Player2;

    public static void StartGame(Player player1, Player player2)
    {
        AssignPlayers(player1, player2);
        AssignTurnOrder();
        InitialDraw(player1);
        InitialDraw(player2);

        StartTurns();
    }

    private static void StartTurns()
    {
        bool isGameOver = false;
        while (!isGameOver)
        {
            if (Player1.IsFirst)
            {
                PlayTurn(Player1, Player2);
                PlayTurn(Player2, Player1);
            }
            else
            {
                PlayTurn(Player2, Player1);
                PlayTurn(Player1, Player2);
            }
        }
    }

    private static void PlayTurn(Player player, Player opponent)
    {
        SuperstarAbilityBeforeDraw(player, opponent);
        
        DrawCard(player, opponent);
        bool isTurnOver = false;
        bool usedAbilityThisTurn = false;
        
        while (!isTurnOver)
        {
            StartTurnMessage();
            if (player.Superstar.IsAbilityStartOfTurn || usedAbilityThisTurn)
            {
                StartTurnOptions(player, false);
            }
            else
            {
                StartTurnOptions(player, true);
            }
            
            int selectedOption = AskForNumber(0, 3);
            
            if (selectedOption == 0)
            {
                if (usedAbilityThisTurn)
                {
                    Console.WriteLine("Ya utilizaste tu habilidad este turno");
                }
                else if (player.Superstar.IsAbilityStartOfTurn)
                {
                    Console.WriteLine("Recuerda que " + player.Superstar.name + " solo puede utilizar su habilidad antes de robar");
                }
                else
                {
                    player.Superstar.UseAbility(player, opponent);
                    usedAbilityThisTurn = true;
                }
            } 
            else if (selectedOption == 1)
            {
                ViewCards(player, opponent);
            }
            else if (selectedOption == 2)
            {
                bool wasReversed = PlayCard(player, opponent);
                if (wasReversed)
                {
                    isTurnOver = true;
                }
            }
            else if (selectedOption == 3)
            {
                isTurnOver = true;
            }
            
        }
    }

    private static void SuperstarAbilityBeforeDraw(Player player, Player opponent)
    {
        if (player.Superstar.name == "KANE")
        {
            KaneSuperStarAbility(opponent);
        }
        else if (player.Superstar.name == "MANKIND")
        {
            Console.WriteLine("Mankind utiliza su habilidad y roba una segunda carta");
            DrawCard(player, opponent);
        }
        else if (player.Superstar.name == "THE ROCK")
        {
            TheRockSuperStarAbility(player);
        }
    }

    private static void TheRockSuperStarAbility(Player player)
    {
        if (player.Ringside.Count != 0)
        {
            Console.WriteLine("¿Quieres utilizar la habilidad de The Rock antes de robar?");
            Console.WriteLine("0: No || 1: Sí");
            int selectedOption = AskForNumber(0, 1);
            if (selectedOption == 1)
            {
                RecoverCard(player);
            }
        }
        else
        {
            Console.WriteLine("The Rock no puede usar su habilidad porque no hay cartas en su Ringside");
        }
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

    private static void KaneSuperStarAbility(Player opponent)
    {
        Console.WriteLine("Kane utiliza su súper habilidad y hace 1D a " + opponent.Superstar.name);
        Card lostCard = opponent.Arsenal[opponent.Arsenal.Count - 1];
        opponent.Ringside.Add(lostCard);
        opponent.Arsenal.RemoveAt(opponent.Arsenal.Count - 1);
    }

    private static bool PlayCard(Player player, Player opponent)
    {
        bool ultimateWasReverted = false;
        Console.WriteLine("Estas son las cartas que puedes jugar:");
        List<Card> playableCards = GetPlayableCards(player);
        
        PrintCards(playableCards);
        Console.WriteLine("-------------");
        Console.WriteLine("Ingresa el ID de la carta que quieres jugar. Puedes ingresar '-1' para cancelar.");
        Console.WriteLine("(Ingresa un número entre -1 y " + (playableCards.Count - 1) + ")");
        
        int selectedOption = AskForNumber(-1 , playableCards.Count - 1);

        if (selectedOption != -1)
        {
            Card playedCard = playableCards[selectedOption];

            PlayedCardData(player, playedCard);
            
            bool isCardReverted = ReverseCard(playedCard, player, opponent);

            if (!isCardReverted)
            {
                PutCardInRingArea(playedCard, player);
                ApplyCardEffect(playedCard, player, opponent);
                bool wasPlayedCardReversed = ExecuteCardDamage(playedCard, player, opponent, false, null);
                if (wasPlayedCardReversed)
                {
                    ultimateWasReverted = true;
                }
            }
            else
            {
                PutCardInRingSide(playedCard, player);
                ultimateWasReverted = true;
            }
        }
        return ultimateWasReverted;
    }

    private static bool ReverseCard(Card playedCard, Player player, Player opponent)
    {
        bool isReversed = false;
        
        Console.WriteLine("-----------------------------");
        Console.WriteLine("Pero " + opponent.Superstar.name + " tiene la opción de revertir la carta:");
        Console.WriteLine("");

        List<Card> availableReversals = CheckReversals(playedCard, opponent);

        if (availableReversals.Count != 0)
        {
            bool didReverse = ReversalMenu(player, opponent, availableReversals, playedCard);
            if (didReverse)
            {
                isReversed = true;
            }
        }
        else
        {
            Console.WriteLine("Lo lamento, pero no hay nada que jugar");
            Console.WriteLine("");

            CardNotReverted(playedCard, player, opponent);
        }

        return isReversed;
    }

    private static bool ReversalMenu(Player player, Player opponent, List<Card> availableReversals, Card playedCard)
    {
        bool isReverted = false;
        Console.WriteLine("Estas son las cartas que puedes jugar");
        PrintCards(availableReversals);
        
        Console.WriteLine("Ingresa el ID de la carta que quieres jugar. Puedes ingresar '-1' para cancelar.");
        Console.WriteLine("(Ingresa un número entre -1 y " + (availableReversals.Count - 1) + ")");
        
        int selectedOption = AskForNumber(-1 , availableReversals.Count - 1);

        if (selectedOption != -1)
        {
            ExecuteReversal(player, opponent, availableReversals[selectedOption], playedCard);
            isReverted = true;
        }

        return isReverted;
    }

    private static void ExecuteReversal(Player player, Player opponent, Card selectedCard, Card playedCard)
    {
        Console.WriteLine("");
        Console.WriteLine("-----------------------");
        Console.WriteLine(opponent.Superstar.name + " revierte la carta usando:");

        FinalCardData(selectedCard);
        
        Console.WriteLine("");
        Console.WriteLine("-----------------------");

        ApplyCardEffect(selectedCard, opponent, player);
        
        PutCardInRingArea(selectedCard, opponent);

        ExecuteCardDamage(selectedCard, opponent, player, true, playedCard);

    }

    private static List<Card> CheckReversals(Card playedCard, Player player)
    {
        List<Card> availableReversals = new List<Card>();

        foreach (var checkedCard in player.Hand)
        {
            bool isUsableReversal = checkedCard.CheckReversal(playedCard);
            if (isUsableReversal && checkedCard.GetFortitudeInt() <= player.GetFortitude())
            {
                availableReversals.Add(checkedCard);
            }
        }
        return availableReversals;
    }

    private static void CardNotReverted(Card playedCard, Player player, Player opponent)
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine(opponent.Superstar.name + " no revierte la carta de " + player.Superstar.name);
        
        Console.WriteLine("La carta '" + playedCard.Title + "' es exitosamente jugada");
        FinalCardData(playedCard);
        
        Console.WriteLine("");
        Console.WriteLine(opponent.Superstar.name + " recibe " + playedCard.Damage + " de daño.");
        Console.WriteLine("");
    }

    private static void PlayedCardData(Player player, Card card)
    {
        Console.WriteLine("---------------------");
        Console.WriteLine(player.Superstar.name + " intenta jugar la siguiente carta");
        
        FinalCardData(card);
    }

    private static void FinalCardData(Card card)
    {
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
    }

    private static void PutCardInRingArea(Card playedCard, Player player)
    {
        player.RingArea.Add(playedCard);
        player.Hand.Remove(playedCard);
    }

    private static void PutCardInRingSide(Card playedCard, Player player)
    {
        player.Ringside.Add(playedCard);
        player.Hand.Remove(playedCard);
    }

    private static void ApplyCardEffect(Card playedCard, Player player, Player opponent)
    {
        playedCard.UseCardAbility(player, opponent);
    }

    public static bool ExecuteCardDamage(Card playedCard, Player player, Player opponent, bool isReversal, Card reversedCard)
    {
        bool wasPlayedCardReversed = false;
        int totalDamage = 0;

        if (playedCard.Damage != "#")
        {
            totalDamage += playedCard.GetDamageInt();
        }
        else
        {
            totalDamage += playedCard.SpecialDamage(reversedCard);
        }
        totalDamage -= opponent.Superstar.DamageReduction;

        for (int i = 0; i < totalDamage; i++)
        {
            if (opponent.Arsenal.Count == 0)
            {
                AnnounceWinner(player, opponent);
            }
            else
            {
                Card lostCard = opponent.Arsenal[opponent.Arsenal.Count - 1];
                opponent.Ringside.Add(lostCard);
                opponent.Arsenal.RemoveAt(opponent.Arsenal.Count - 1);
                
                Console.WriteLine("------------------------ " + (i + 1) + "/" + totalDamage + " damage");
                FinalCardData(lostCard);

                if (!isReversal)
                {
                    bool isUsefulReversal = lostCard.CheckReversal(playedCard);
                    if (isUsefulReversal && lostCard.GetFortitudeInt() <= opponent.GetFortitude())
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Esta carta revierte la maniobra de " + player.Superstar.name);

                        if ((i + 1) != totalDamage) // si es que no se completó el daño
                        {
                            DrawCardsStunValue(player, opponent, playedCard);
                        }
                        wasPlayedCardReversed = true;
                        break;
                    }
                }
            }
        }

        return wasPlayedCardReversed;
    }

    private static void DrawCardsStunValue(Player player, Player opponent, Card reversedCard)
    {
        if (reversedCard.GetStunValueInt() != 0)
        {
            Console.WriteLine("");
            Console.WriteLine(player.Superstar.name + " roba " + reversedCard.GetStunValueInt() + " debido a que su carta fue reversada.");
            for (int i = 0; i < reversedCard.GetStunValueInt(); i++)
            {
                DrawCard(player, opponent);
            }
        }
    }

    private static void AnnounceWinner(Player winner, Player loser)
    {
        Console.WriteLine("#####################");
        Console.WriteLine(loser.Superstar.name + " se ha quedado sin cartas en su Arsenal!");
        Console.WriteLine("El ganador es " + winner.Superstar.name + "!!!!!");
        Environment.Exit(0);
    }

    private static List<Card> GetPlayableCards(Player player)
    {
        List<Card> playableCards = new List<Card>();
        foreach (var card in player.Hand)
        {
            if (card.GetFortitudeInt() <= player.GetFortitude() && (card.Types.Contains("Action") || card.Types.Contains("Maneuver")))
            {
                playableCards.Add(card);
            }
        }

        return playableCards;
    }

    private static void ViewCards(Player player, Player opponent)
    {
        Console.WriteLine("Juega " + player.Superstar.name + ". ¿Qué cartas quieres ver?");
        Console.WriteLine("        1. Mi mano.");
        Console.WriteLine("        2. Mi ringside.");
        Console.WriteLine("        3. Mi ring area.");
        Console.WriteLine("        4. El ringside de mi oponente.");
        Console.WriteLine("        5. El ring area de mi oponente.");
        Console.WriteLine("(Ingresa un número entre 0 y 5)");
        
        int selectedOption = AskForNumber(1, 5);

        if (selectedOption == 1)
        {
            PrintCards(player.Hand);
        }
        else if (selectedOption == 2)
        {
            PrintCards(player.Ringside);
        }
        else if (selectedOption == 3)
        {
            PrintCards(player.RingArea);
        }
        else if (selectedOption == 4)
        {
            PrintCards(opponent.Ringside);
        }
        else if (selectedOption == 5)
        {
            PrintCards(opponent.RingArea);
        }
    }

    public static void PrintCards(List<Card> cards)
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

    private static void StartTurnOptions(Player player, bool isAbilityAvailable)
    {
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("Juega " + player.Superstar.name + ". ¿Qué quieres hacer?");
        if (isAbilityAvailable)
        {
            Console.WriteLine("        0. Usar mi super habilidad");
        }
        Console.WriteLine("        1. Ver mis cartas o las cartas de mi oponente");
        Console.WriteLine("        2. Jugar una carta");
        Console.WriteLine("        3. Terminar mi turno");
        Console.WriteLine("(Ingresa un número entre 0 y 3)");

    }

    private static void AssignTurnOrder()
    {
        if (Player1.Superstar.superstarValue > Player2.Superstar.superstarValue)
        {
            Player1.IsFirst = true;
            Player2.IsFirst = false;
        }
        else if (Player1.Superstar.superstarValue < Player2.Superstar.superstarValue)
        {
            Player1.IsFirst = false;
            Player2.IsFirst = true;
        }
        else
        {
            Random coin = new Random();
            int coinResult = coin.Next(0, 2);
            if (coinResult == 0)
            {
                Player1.IsFirst = true;
                Player2.IsFirst = false;
            }
            else
            {
                Player1.IsFirst = false;
                Player2.IsFirst = true;
            }
        }
    }

    private static void InitialDraw(Player player)
    {
        for (int i = 0; i < player.Superstar.handSize; i++)
        {
            Card drawnCard = player.Arsenal[player.Arsenal.Count - 1];
            player.Hand.Add(drawnCard);
            player.Arsenal.RemoveAt(player.Arsenal.Count - 1);
        }
    }

    public static void DrawCard(Player player, Player opponent)
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

    private static void StartTurnMessage()
    {
        Console.WriteLine("");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("Se enfrentan: " + Player1.Superstar.name + " y " + Player2.Superstar.name);
        Console.WriteLine(Player1.Superstar.name + " tiene " + Player1.GetFortitude() + "F, " + Player1.Hand.Count + 
                          " cartas en su mano y le quedan " + Player1.Arsenal.Count + " en su arsenal");
        Console.WriteLine(Player2.Superstar.name + " tiene " + Player2.GetFortitude() + "F, " + Player2.Hand.Count + 
                          " cartas en su mano y le quedan " + Player2.Arsenal.Count + " en su arsenal");
    }

    private static void AssignPlayers(Player player1, Player player2)
    {
        Player1 = player1;
        Player2 = player2;
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

    public static void DiscardCardMenu(Player player, int amountToDiscard)
    {
        Console.WriteLine("-------------------------");
        Console.WriteLine("Jugador " + player.Superstar.name + " debe descartar " + amountToDiscard + " carta(s)");
        PrintCards(player.Hand);
        Console.WriteLine("Selecciona una opción:");
        int selectedId = AskForNumber(0, player.Hand.Count - 1);
        DiscardCard(player, selectedId);
    }

    private static void DiscardCard(Player player, int selectedId)
    {
        Card discardedCard = player.Hand[selectedId];
        player.Ringside.Add(discardedCard);
        player.Hand.RemoveAt(selectedId);
    }
    
    public static void RecoverCardToHand(Player player)
    {
        PrintCards(player.Ringside);
        Console.WriteLine("Ingresa el ID de la carta que quieres recuperar.");
        Console.WriteLine("(Ingresa un número entre 0 y " + (player.Ringside.Count - 1) + ")");
        
        int selectedOption = AskForNumber(0 , player.Ringside.Count - 1);
        
        Card selectedCard = player.Ringside[selectedOption];
        player.Hand.Add(selectedCard);
        player.Ringside.Remove(selectedCard);
    }

    public static void ReceiveOneDamage(Player player)
    {
        Card lostCard = player.Arsenal[player.Arsenal.Count - 1];
        player.Ringside.Add(lostCard);
        player.Arsenal.RemoveAt(player.Arsenal.Count - 1);
    }
}