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
            
            // isGameOver = true;
        }
    }

    private static void PlayTurn(Player player, Player opponent)
    {
        bool isTurnOver = false;
        while (!isTurnOver)
        {
            StartTurnMessage();
            StartTurnOptions(player);
            int selectedOption = AskForNumber(0, 3);

            if (selectedOption == 0)
            {
                UseSuperAbility();
            } 
            else if (selectedOption == 1)
            {
                ViewCards();
            }
            else if (selectedOption == 2)
            {
                PlayCard();
            }
            else if (selectedOption == 3)
            {
                isTurnOver = true;
            }
            
        }
    }

    private static void PlayCard()
    {
        
    }

    private static void ViewCards()
    {
        
    }

    private static void UseSuperAbility()
    {
        
    }

    private static void StartTurnOptions(Player player)
    {
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("Juega " + player.Superstar.name + ". ¿Qué quieres hacer?");
        Console.WriteLine("        0. Usar mi super habilidad");
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
}