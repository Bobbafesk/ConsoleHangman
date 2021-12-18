using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace ConsoleHangman
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                WriteLine(" Please chose a number for wat you want to do?" +
                    "\n 0:Exit" +
                    "\n 1:Game Hangmen" +
                    ""
                    );
                int action = GetIntFromUser();
                switch (action)
                {
                    case 0:
                        running = false;
                        break;

                    case 1:
                        Clear();
                        PlayHang();
                        break;

                    default:
                        WriteLine(" Something went wrong, chose 1 for Play Hangman and 0 to exit.");
                        break;
                }
            }
        }

        private static void PlayHang()
        {
            int guesses = 0;
            string hangWord = RandomHangWord();
            Char [] guessWordResult = new char [hangWord.Length];
            StringBuilder strB_AllLetters = new StringBuilder();
            StringBuilder strBGuessedWrongLetters = new StringBuilder();

            // Fill the char array with underscore
            FillArrayWithUnderscore(guessWordResult);
          
            WriteLine("\r\n Random ord är: " + hangWord);

            WriteLine("\n Welcome to the Hangman game." + 
                "\n You got 10 rounds before you got hanged!" +
                "\n Guess a letter or the whole word!" +
                "");

            string inString = "";

            while (guesses < 10 && guessWordResult.Contains<char>('_'))
            {
                guesses++; // Starts at round 1

                // Writes total update of the round
                WriteUpdate(strBGuessedWrongLetters, guessWordResult, guesses);

                inString = ReadLine().ToUpper();
                // Check if the Input is one letter
                if (inString.Length == 1)
                {
                    // Update word reult with rigth char or update wrong word
                    if (!UpdateWordWithRightChar(inString, hangWord, guessWordResult))
                    {
                        if (!InputIsInStrB(inString, strBGuessedWrongLetters)) // Not in list before
                        {
                            strBGuessedWrongLetters.Append(inString);
                        }
                    }

                    // Append all letters that are guessed if it not in list before
                    if (InputIsInStrB(inString, strB_AllLetters))
                    {
                        guesses--; // Will not consume a round if already guessed char
                    }
                    else
                    {
                        strB_AllLetters.Append(inString);
                    }
                }
                // Check if input is correct word and updates result
                else if(inString.Length == hangWord.Length)
                {
                    if ( SheckWordCorrect(inString, hangWord))
                    {
                        UpdateResultWord(guessWordResult, hangWord);
                    }
                }
                // Wrong amount of letters
                else
                {
                    guesses--; // not consume a guess
                    WriteLine(" Wrong amount of letters, please wrigth one letter or guess for whole word"); 
                }
            }
            // Won or lose
            if (guesses < 11 && !guessWordResult.Contains<char>('_'))
            {               
                string result = new string(guessWordResult); ;

                WriteLine("\r\n Congratulation you won the game, before you where hanged. The word was {0}, and you found it in rounds {1}.\r\n",
                    result, guesses);
            }
            else { WriteLine("\r\n You got henged men!!\r\n"); }
        }

        /***************  
         *  Help methods
         ***************/

        private static void UpdateResultWord(char[] guessWordResult, string hangWord)
        {
            for (int i = 0; i < hangWord.Length; i++)
            {
                guessWordResult[i] = hangWord[i];
            }
        }

        private static bool UpdateWordWithRightChar(string inString, string hangWord, char[] guessWordResult)
        {
            bool rightChar = false;
            for (int i = 0; i < hangWord.Length; i++)
            {
                if (hangWord[i] == inString[0])
                {
                    guessWordResult[i] = inString[0];
                    rightChar = true;
                }
            }
            return rightChar;
        }

        private static void FillArrayWithUnderscore(char[] guessWordResult)
        {
            for (int i = 0; i < guessWordResult.Length; i++)
            {
                guessWordResult[i] = '_';
            }
        }

        // Check if input string is in stringBuilder
        static bool InputIsInStrB(String inputString, StringBuilder strB)
        {
            return strB.ToString().Contains(inputString);
        }

        private static void WriteUpdate(StringBuilder strBGuessedWrongLetters, char [] guessWordResult, int guesses)
        {
            WriteLine(" ******************");
            WriteLine(" Round: {0} \r\n Wrong letters: {1}", guesses, strBGuessedWrongLetters);
            WriteLine();
            Write(" Word: ");

            // Write the guessed word with underscore if any left 
            for (int i = 0; i < guessWordResult.Length; i++)
            {
                Write( guessWordResult[i] + " ");
            }
            Write("\r\n Make a guess: ");
        }



        private static bool SheckWordCorrect(string inputStr, string hangManW)
        {
           return inputStr.Equals(hangManW, StringComparison.OrdinalIgnoreCase);
        }

        public static string RandomHangWord()
        {
            Random rnd = new Random();
            string[] hangManWords =
            {"JAZZ", "RABBIT", "GAME", "FUN",
             "RADIO", "DRESS", "BLUEBERRY", "TEST",
             "GLAD", "FISCH", "SUNBEAM","BEAR"};

            int nameIndex = rnd.Next(hangManWords.Length);

            return hangManWords[nameIndex];
        }


        //******************************************************************
        //******************************************************************

        static int GetIntFromUser()
        {
            int number = 0;
            bool success = false;
            do
            {
                try
                {
                    number = int.Parse(ReadLine());
                    success = true;
                }
                catch (OverflowException)
                {
                    WriteLine("Your value is too big");
                }
                catch (ArgumentNullException)
                {
                    WriteLine("Could not parse, value was null");
                }
                catch (FormatException error)
                {
                    WriteLine(error.Message);

                    WriteLine("Wrong format");
                }
            } while (!success);

            return number;

        }
    }
}