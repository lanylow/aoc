#include <iostream>
#include <string_view>
#include <fstream>
#include <deque>
#include <vector>
#include <numeric>

std::deque<uint8_t> input;

class file_iterator {
public:
  class iterator {
  public:
    file_iterator* ptr;

    bool operator!=(const iterator& other) const {
      return ptr->stream.peek() != -1;
    }

    iterator& operator++() {
      return *this;
    }

    std::string operator*() const {
      return ptr->next();
    }
  };

  file_iterator(std::string name, char delim) {
    stream.open(name);
    this->delim = delim;
  }

  iterator begin() {
    iterator i;
    i.ptr = this;
    return i;
  }

  iterator end() {
    return begin();
  }

  std::string next() {
    std::string res;
    int i;

    while ((i = stream.get()) != -1 && i != delim)
      res.push_back(i);

    return res;
  }

  char delim;
  std::ifstream stream;
};

class packet {
public:
  packet(std::deque<uint8_t>& bits) {
    int bits_read = 6;

    ver = read(bits, 3);
    id = read(bits, 3);
    data = 0;

    switch (id) {
      case 4: data = read_num(bits, bits_read); break;
      default: {
        bool b = bits.front();
        bits.pop_front();

        if (b) {
          data = read(bits, 11);

          while (sub_packets.size() != data) {
            packet child(bits);
            sub_packets.push_back(child);
          }
        }
        else {
          data = read(bits, 15);
          uint64_t t = bits.size() - data;

          while (bits.size() != t) {
            packet child(bits);
            sub_packets.push_back(child);
          }
        }

        break;
      }
    }
  }

  int read(std::deque<uint8_t>& bits, int cnt) {
    int res = 0;

    while (cnt) {
      res <<= 1;
      res |= bits.front();
      bits.pop_front();
      cnt--;
    }

    return res;
  }

  int64_t read_num(std::deque<uint8_t>& bits, int cnt) {
    int64_t res = 0;
    int r = 0;

    do {
      r = read(bits, 5);
      res <<= 4;
      res |= r & 0x0F;
      cnt += 5;
    } while (r & 0x10);

    return res;
  }

  size_t sub_packet_cnt() {
    size_t res = sub_packets.size();

    for (auto& packet : sub_packets)
      res += packet.sub_packet_cnt();

    return res;
  }

  int64_t eval() const {
    switch (id) {
      case 4: return data;
      case 0: return std::accumulate(sub_packets.begin(), sub_packets.end(), 0ll, [](auto i, auto j) { return j.eval() + i; });
      case 1: return std::accumulate(sub_packets.begin(), sub_packets.end(), 1ll, [](auto i, auto j) { return j.eval() * i; });
      case 2:
      case 3: {
        int64_t min = std::numeric_limits<int64_t>::max();
        int64_t max = 0;

        for (auto& sp : sub_packets) {
          int64_t e = sp.eval();
          min = std::min(min, e);
          max = std::max(max, e);
        }

        if (id == 2) return min;
        return max;
      }
      case 5: return sub_packets[0].eval() > sub_packets[1].eval() ? 1 : 0;
      case 6: return sub_packets[0].eval() < sub_packets[1].eval() ? 1 : 0;
      case 7: return sub_packets[0].eval() == sub_packets[1].eval() ? 1 : 0;
    }

    return -1;
  }

  int ver, id;
  int64_t data;
  std::vector<packet> sub_packets;
};

void parse() {
  for (auto l : file_iterator("input.txt", '\n')) {
    for (auto c : l) {
      char n = 0;

      if (c >= 'A') n = c - 'A' + 10;
      else n = c - '0';

      for (int i = 4; i != 0;) {
        i--;
        input.push_back((n >> i) & 1);
      }
    }
  }
}

int total_version(packet& p) {
  int res = p.ver;

  for (auto& sp : p.sub_packets)
    res += total_version(sp);

  return res;
}

int main(int argc, char** argv) {
  parse();

  packet p(input);

  std::cout << "Versions of all packets added up (Part 1): " << total_version(p) << std::endl;
  std::cout << "Result of evaluating the expression represented by your hexadecimal-encoded BITS transmission (Part 2): " << p.eval() << std::endl;

  system("pause");
}