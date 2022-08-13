#include <deque>
#include <string>
#include <cstdint>
#include <fstream>
#include <set>

std::pair<std::deque<std::uint64_t>, std::deque<std::uint64_t>> GetCardsFromFile(std::string file) {
  std::fstream input(file);

  if (!input.good()) {
    throw std::runtime_error("Couldn't open the input file.");
  }

  std::deque<std::uint64_t> player_cards[2];
  std::string line;

  for (int i = 0; i < 2; ++i) {
    std::getline(input, line);

    while (std::getline(input, line)) {
      if (line.empty()) {
        break;
      }

      player_cards[i].emplace_back(std::stoll(line));
    }
  }

  return std::make_pair(player_cards[0], player_cards[1]);
}

std::uint64_t GetScore(const std::deque<std::uint64_t>& rCards) {
  std::uint64_t output{ 0 };
  std::uint64_t multiplier{ 1 };

  for (auto i = rCards.rbegin(); i != rCards.rend(); ++i, ++multiplier) {
    output += (*i) * multiplier;
  }

  return output;
}

std::pair<int, std::uint64_t> SimulateGame(int part, std::deque<std::uint64_t> playerOne, std::deque<std::uint64_t> playerTwo) {
  std::set<std::pair<std::deque<std::uint64_t>, std::deque<std::uint64_t>>> used_cards;

  while (!playerOne.empty() && !playerTwo.empty()) {
    if (part == 2 && !used_cards.emplace(std::make_pair(playerOne, playerTwo)).second) {
      return std::make_pair(1, GetScore(playerOne));
    }

    std::uint64_t playerOneCard{ playerOne.front() };
    std::uint64_t playerTwoCard{ playerTwo.front() };

    playerOne.pop_front();
    playerTwo.pop_front();

    int winner{ playerOneCard > playerTwoCard ? 1 : 2 };

    if (part == 2 && playerOneCard <= playerOne.size() && playerTwoCard <= playerTwo.size()) {
      winner = SimulateGame(part, std::deque<std::uint64_t>(playerOne.begin(), playerOne.begin() + playerOneCard), std::deque<std::uint64_t>(playerTwo.begin(), playerTwo.begin() + playerTwoCard)).first;
    }

    if (winner == 1) {
      playerOne.push_back(playerOneCard);
      playerOne.push_back(playerTwoCard);
    }
    else {
      playerTwo.push_back(playerTwoCard);
      playerTwo.push_back(playerOneCard);
    }
  }

  return std::make_pair(playerTwo.empty() ? 1 : 2, playerTwo.empty() ? GetScore(playerOne) : GetScore(playerTwo));
}

int main() {
  auto [player_one, player_two] { GetCardsFromFile("input.txt") };

  std::printf("Winning player's score: %lld\n", SimulateGame(1, player_one, player_two).second);
  std::printf("Winning player's score after a recursion game: %lld\n", SimulateGame(2, player_one, player_two).second);
}