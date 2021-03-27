#include "Header.h"
using namespace std;

/*void Cyclic_Bruteforce_1();*/




int main() {
	setlocale(LC_ALL, "Russian");
	
	while (!(GetAsyncKeyState(VK_ESCAPE) & 0x8000)) {
		
		cin.clear();
		cin.ignore(cin.rdbuf()->in_avail());

		cout << "Выберите алгоритм: 1) Умный 2) Адекватный 3) Слабоумный" << endl;
	
		switch (cin.get()) {
		case '1': 
			quick_bruteforce();
			break;
		case '2': 
			Cyclic_Bruteforce_MThread();
			break;
		case '3': 
			break;
		default: 
			break;
		}

	}
}



/*
void Cyclic_Bruteforce_1() {
	cout << "Введите алфавит" << endl;
	string alphabet; cin >> alphabet; int alphabet_size = alphabet.size();

	cout << "Введите хеш" << endl;
	long long hash; cin >> hash;

	int range = Range(hash); long long n;

	char* password = new char[range + 1];
	memset(password, 0, (range + 1) * sizeof(char));
	
	
	int rank; long long j; unsigned int start_time = clock(), end_time;
	for (int i = 1; i <= range; i++) {
		
		n = pow(alphabet_size, i);
		memset(password, alphabet[0], i * sizeof(char));
		unsigned int start_cycle_time = clock();

		for (long long i = 0; i < n; i++) {
			password[0] = alphabet[i % alphabet_size];

			rank = 0, j = i;
			while (j != 0 && !(j % alphabet_size)) {
				password[rank] = alphabet[0];
				j /= alphabet_size;
				rank++;
			}
			password[rank] = alphabet[j % alphabet_size];


			/*if (!(i % alphabet_size)) {
				rank=0, j = i;
				while (j >= alphabet_size) {
					password[rank] = alphabet[j % alphabet_size];
					j /= alphabet_size;
					rank++;
				}
				password[rank] = alphabet[j];

			}
			if (hash == GetCode(password, range)) {
				end_time = clock() - start_time;
				cout << password << " " << end_time << endl;
			}


		}

		end_time = clock() - start_cycle_time;
		printf_s("%i - digit password bruteforce time: %u\n", i, end_time);
	}
	end_time = clock() - start_time;
	cout << password << " " << end_time << endl;
	delete[] password;
}

*/