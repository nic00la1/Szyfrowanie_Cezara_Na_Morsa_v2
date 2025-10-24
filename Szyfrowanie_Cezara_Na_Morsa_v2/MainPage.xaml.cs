namespace Szyfrowanie_Cezara_Na_Morsa_v2
{
    public partial class MainPage : ContentPage
    {
        // Słowniki do tłumaczenia między alfabetem a kodem Morse’a
        private Dictionary<char, string> _textToMorse;
        private Dictionary<string, char> _morseToText;

        /// <summary>
        /// Konstruktor strony głównej.
        /// Inicjalizuje komponenty i słowniki Morse’a.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            InitializeDictionaries();
        }

        /// <summary>
        /// Inicjalizuje słowniki do konwersji pomiędzy tekstem a kodem Morse’a.
        /// </summary>
        private void InitializeDictionaries()
        {
            _textToMorse = new Dictionary<char, string>()
            {
                {'A' , ".-"}, {'B' , "-..."}, {'C' , "-.-."}, {'D' , "-.."},
                {'E' , "."}, {'F' , "..-."}, {'G' , "--."}, {'H' , "...."},
                {'I' , ".."}, {'J' , ".---"}, {'K' , "-.-"}, {'L' , ".-.."},
                {'M' , "--"}, {'N' , "-."}, {'O' , "---"}, {'P' , ".--."},
                {'Q' , "--.-"}, {'R' , ".-."}, {'S' , "..."}, {'T' , "-"},
                {'U' , "..-"}, {'V' , "...-"}, {'W' , ".--"}, {'X' , "-..-"},
                {'Y' , "-.--"}, {'Z' , "--.."},
                {'0' , "-----"}, {'1' , ".----"}, {'2' , "..---"}, {'3' , "...--"},
                {'4' , "....-"}, {'5' , "....."}, {'6' , "-...."}, {'7' , "--..."},
                {'8' , "---.."}, {'9' , "----."}, {' ' , "/"}
            };

            _morseToText = _textToMorse.ToDictionary(kv => kv.Value, kv => kv.Key);
        }

        /// <summary>
        /// Obsługuje kliknięcie przycisku „Przetłumacz”.
        /// Automatycznie rozpoznaje, czy użytkownik wpisał kod Morse’a czy zwykły tekst.
        /// </summary>
        private void Button_Clicked(object sender, EventArgs e)
        {
            string input = entry.Text.ToUpper().Trim();
            string result;

            // Sprawdza, czy to kod Morse’a (zawiera tylko ., -, / lub spacje)
            if (IsMorse(input))
            {
                // Morse → tekst → odszyfrowanie Cezara
                string text = TranslateFromMorse(input);
                result = CaesarCipher(text, -3); // przesunięcie o -3
            }
            else
            {
                // Tekst → szyfr Cezara → Morse
                string encrypted = CaesarCipher(input, 3); // przesunięcie o 3
                result = TranslateToMorse(encrypted);
            }

            resultEntry.Text = result; // wynik w Entry, które można skopiować
        }

        /// <summary>
        /// Sprawdza, czy dany ciąg jest kodem Morse’a.
        /// </summary>
        private bool IsMorse(string input)
        {
            return input.All(c => c == '.' || c == '-' || c == ' ' || c == '/');
        }

        /// <summary>
        /// Szyfruje lub odszyfrowuje tekst szyfrem Cezara o podanym przesunięciu.
        /// </summary>
        private string CaesarCipher(string text, int shift)
        {
            char ShiftChar(char c)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    return (char)(((c + shift - offset + 26) % 26) + offset);
                }
                return c;
            }

            return new string(text.Select(ShiftChar).ToArray());
        }

        /// <summary>
        /// Zamienia tekst na kod Morse’a.
        /// </summary>
        private string TranslateToMorse(string input)
        {
            var output = new List<string>();
            foreach (char c in input)
            {
                if (_textToMorse.ContainsKey(c))
                    output.Add(_textToMorse[c]);
                else
                    output.Add("?");
            }
            return string.Join(" ", output);
        }

        /// <summary>
        /// Zamienia kod Morse’a na zwykły tekst.
        /// </summary>
        private string TranslateFromMorse(string morse)
        {
            var output = new List<char>();
            foreach (var symbol in morse.Split(' '))
            {
                if (_morseToText.ContainsKey(symbol))
                    output.Add(_morseToText[symbol]);
                else
                    output.Add('?');
            }
            return new string(output.ToArray());
        }
    }
}
