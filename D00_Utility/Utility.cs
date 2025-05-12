using System;
using System.Text;

namespace D00_Utility
{
    public static class Utility
    {
        #region Method to SetUnicodeConsole     
        public static void SetUnicodeConsole()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }
        #endregion

        #region Method to Writetitlle     
        public static void WriteTitle(string title, string beginTitle = "\n\n\n", string endTitle = "\n")
        {

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine($"{beginTitle}{new string('-', 20)}");

            Console.WriteLine(title.ToUpper());

            Console.WriteLine($"{new string('-', 20)}{endTitle}");

            Console.ForegroundColor = ConsoleColor.White;

        }
        #endregion

        #region Method to PauseConsole

        public static void PauseConsole()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("Press any key to continue...");
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }
        #endregion

        #region Method to TerminateConsole
        public static void TerminateConsole()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\n\n Prime qualquer tecla para terminares.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }
        #endregion

        #region Method to write a ErrorMessage
        public static void WriteErrorMessage(string message, string beginMessage = "", string endMessage = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{beginMessage}{message}{endMessage}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion

        #region Method to write a normal message       
        public static void WriteMessage(string message, string beginMessage = "", string endMessage = "", ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write($"{beginMessage}{message}{endMessage}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion

        #region Method to write a message with a specified color      

        public static void WriteMessageWithColor(string message, ConsoleColor color)
        {
            // Save the current color so it can be reset later
            var currentColor = Console.ForegroundColor;

            // Change the console color to the specified color
            Console.ForegroundColor = color;

            // Print the message in the new color
            Console.WriteLine(message);

            // Reset the console color to the original color
            Console.ForegroundColor = currentColor;
        }
        #endregion

        #region Method to CenterText
        public static void CenterText(string message)
        {
            int windowWidth = Console.WindowWidth;
            int startPos = (windowWidth / 2) - (message.Length / 2);
            Console.SetCursorPosition(startPos, Console.CursorTop);
            Console.WriteLine(message);
        }

        #endregion

        #region Method ShowGreetings
        public static void ShowGreeting()
        {
            string greeting = "Welcome to the Vacation Manager!";
            var currentTime = DateTime.Now;

            if (currentTime.Hour < 12)
            {
                greeting = "Good morning, " + greeting;
            }
            else if (currentTime.Hour < 18)
            {
                greeting = "Good afternoon, " + greeting;
            }
            else
            {
                greeting = "Good evening, " + greeting;
            }

            Console.WriteLine(greeting);
            Console.WriteLine($"Today's Date: {currentTime:dddd, MMMM dd, yyyy}");
        }
        #endregion

        #region MethodtoReadPassword
        public static string ReadPassword(string prompt)
        {
            Console.Write(prompt);
            string password = string.Empty;

            while (true)
            {
                var key = Console.ReadKey(intercept: true); // Não mostra o que é digitado

                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
                else
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return password;
        }
        #endregion
    }
}




