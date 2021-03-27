#include <iostream>
#include <cmath>
#include <Windows.h>
#include <ctime>
#include <cstdio>
#include <thread>
using namespace std;

long long GetCode(const char* St, int range);

int Range(long long hash);

void quick_bruteforce();

void QB(long long hash, string password, int count_of_powers[], int** powers_of_symbols);

void Cyclic_Bruteforce();

void Cyclic_Bruteforce_1();

void Cyclic_Bruteforce_MThread();

void CB(long long hash, string alphabet, long long start_i, int range);

void CB(long long hash, string alphabet, int range);




long long GetCode(const char* St, int range)
{
	long long N = 0;
	for (int i = range - 1; i >= 0; i--)
	{
		int C = (int)St[i];
		N = N * 10 + C * C;

	}
	return N;

}

int Range(long long hash) {
	int range(-3);
	while (hash > 0) {
		hash /= 10; range++;
	}
	return range;
}

void quick_bruteforce() {
	cout << "Âגוהטעו אכפאגטע," << endl;
	string alphabet; cin >> alphabet; int alphabet_size = alphabet.size();

	cout << "Âגוהטעו ץור" << endl;
	long long hash; cin >> hash;

	int range = Range(hash);

	string password;

	int count_of_powers[10]{ 0 };
	for (int i = 0; i < alphabet_size; i++) {
		count_of_powers[(int)alphabet[i] * alphabet[i] % 10]++;
	}
	int count_of_powers_copy[10];
	memcpy(count_of_powers_copy, count_of_powers, 10*sizeof(int));

	int** powers_of_symbols = new int* [10];
	for (int i = 0; i < 10; i++) {
		powers_of_symbols[i] = new int[count_of_powers[i]]();

	}

	for (int i = 0; i < alphabet_size; i++) {
		int power = (int)alphabet[i] * alphabet[i];
		powers_of_symbols[power % 10][--count_of_powers_copy[power % 10]] = power;
	}
	QB(hash, password, count_of_powers, powers_of_symbols);
	
}
	
void QB(long long hash, string password, int count_of_powers[], int** powers_of_symbols) {
	string password_copy(password);
	if (hash>0) {
		int n = hash % 10;
		password_copy += " ";
		for (int j = 0; j < count_of_powers[n]; j++) {
			password_copy[password_copy.length()-1]= (char)sqrt(powers_of_symbols[n][j]);
			int hash_copy = hash;
			hash_copy -= powers_of_symbols[n][j];
			hash_copy /= 10;
			if (!hash_copy) 	cout << password_copy << endl;
			else QB(hash_copy, password_copy, count_of_powers, powers_of_symbols);


		}
	}
}

void Cyclic_Bruteforce() {
	cout << "Âגוהטעו אכפאגטע," << endl;
	string alphabet; cin >> alphabet; int alphabet_size = alphabet.size();

	cout << "Âגוהטעו ץור" << endl;
	long long hash; cin >> hash;

	int range = Range(hash); long long n = pow(alphabet_size, range - 1) + pow(alphabet_size, range - 2), j(0);

	char* password = new char[range + 1];
	memset(password, 0, (range + 1) * sizeof(char));
	int* index_of_symbols = new int[range];
	memset(index_of_symbols, 0, range * sizeof(int));

	long long i(0); 	unsigned int start_time = clock(), end_time;

	while (!(GetAsyncKeyState(VK_CONTROL) & 0x8000 && GetAsyncKeyState(0x51)) && i < n) {
		j = 0;
		for (j = 0; j < alphabet_size; j++) {
			if (hash == GetCode(password, range)) {
				end_time = clock() - start_time;
				cout << password << " " << end_time << endl;
			}
			password[0] = alphabet[j];

		}
		int k(1);
		while (password[k] == alphabet[alphabet_size - 1] && k < range - 1) {
			password[k] = alphabet[0];
			index_of_symbols[k] = 0;
			k++;
		}
		if (k < range) {
			password[k] = alphabet[index_of_symbols[k]];
			index_of_symbols[k]++;
		}
		i++;
	}
	cout << password << endl;
	delete[] password;
	delete[] index_of_symbols;
}

void Cyclic_Bruteforce_1() {
	cout << "Âגוהטעו אכפאגטע" << endl;
	string alphabet; cin >> alphabet; int alphabet_size = alphabet.size();

	cout << "Âגוהטעו ץור" << endl;
	long long hash; cin >> hash;

	int range = Range(hash); long long n;

	unsigned int start_time = clock(), end_time;
	for (int rank = 1; rank <= range; rank++) {
		CB(hash, alphabet, rank);
	}
	end_time = clock() - start_time;
	cout << "Full password bruteforce time: " << end_time << endl;
}

void Cyclic_Bruteforce_MThread() {
	thread threads[4];
	cout << "Âגוהטעו אכפאגטע" << endl;
	string alphabet; cin >> alphabet; int alphabet_size = alphabet.size();

	cout << "Âגוהטעו ץור" << endl;
	long long hash; cin >> hash;

	int range = Range(hash); long long j,n;

	char* password = new char[range + 1];
	memset(password, 0, (range + 1) * sizeof(char));

	
	unsigned int start_time = clock(), end_time;
	if (range > 4) {
		for (int rank = 1; rank <= 4; rank++)
			CB(hash, alphabet, rank);
		for (int i = 5; i <=range; i++) {
			n = pow(alphabet_size, i);
			for (int k = 0; k < 4; k++) {
				j = n / 4 * k;

				threads[k] = thread((void(*)(long long, string, long long, int))CB, hash, alphabet, j, i);
			}
			
			for (int k = 0; k < 4; k++) threads[k].join();
		}
		
	}
	else {
		for (int i = 1; i <= range; i++)
			CB(hash, alphabet, i);
	}
	end_time = clock() - start_time;
	cout << "Full password bruteforce time: " << end_time << endl;
	delete[] password;
	
}

void CB(long long hash, string alphabet, int range) {
	int alphabet_size = alphabet.size();
	
	char* password = new char[range + 1];
	password[range] = 0;
	memset(password, alphabet[0], (range) * sizeof(char));
	
	
	int rank; long long j, n = pow(alphabet_size, range);
	unsigned int start_time = clock(), end_time;
	for (long long i = 0; i < n; i++) {
		password[0] = alphabet[i % alphabet_size];

		rank = 0, j = i;
		while (j != 0 && !(j % alphabet_size)) {
			password[rank] = alphabet[0];
			j /= alphabet_size;
			rank++;
		}
		password[rank] = alphabet[j % alphabet_size];


		
		if (hash == GetCode(password, range)) {
			end_time = clock() - start_time;
			cout << password << " " << end_time << endl;
		}


	}
	
	end_time = clock() - start_time;
	printf_s("%i - digit password bruteforce time: %u\n", range, end_time);
	delete[] password;
}

void CB(long long hash, string alphabet, long long start_i, int range) {
	int alphabet_size = alphabet.size();

	char* password = new char[range + 1];
	password[range] = 0;
	memset(password, alphabet[0], (range) * sizeof(char));

	int rank = 0; long long j = start_i;
	while (j >= alphabet_size) {
		password[rank] = alphabet[j % alphabet_size];
		j /= alphabet_size;
		rank++;
	}
	password[rank] = alphabet[j];

	long long n = pow(alphabet_size, range);
	n = start_i + n / 4;
	unsigned int start_time = clock(), end_time;
	for (long long i = start_i; i < n; i++) {
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

		}*/
		if (hash == GetCode(password, range)) {
			end_time = clock() - start_time;
			cout << password << " " << end_time << endl;
		}


	}

	end_time = clock() - start_time;
	/*printf_s("%i - digit password bruteforce time since %i: %u\n", range, end_time, start_i);*/
	cout << range << " - digit password bruteforce time since " << start_i << ": " << end_time << endl;
	delete[] password;
}