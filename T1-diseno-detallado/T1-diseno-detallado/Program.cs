// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using T1_diseno_detallado;
using System.Text.Json;

List<Card> LoadCards() {
    string fileName = "cartas_v6/cards.json";
    string jsonString = File.ReadAllText(fileName);
    // Console.WriteLine(jsonString);
    // List < Dictionary<string, string> > cardsList = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(jsonString);
    List<Card> cardsList = JsonSerializer.Deserialize<List<Card>>(jsonString);
    // Console.WriteLine(cardsList[0].Damage);
    return cardsList;
}

List<SuperStarCard> CreateSuperStars()
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

// Load();

// Card card = new Card();

List<SuperStarCard> superstars = CreateSuperStars();

Console.WriteLine(superstars[5].name);

List<Card> all_cards = LoadCards();

// SuperStarCard hhh = new SuperStarCard(5, 10, "seco", "HHH");
// Console.WriteLine(hhh.name);
//Player player1 = new Player(hhh);

//player1.Arsenal = all_cards;

//Console.WriteLine(player1.Superstar.ability);
