using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hangman.Models;

namespace Hangman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                secretWord = DataService.GenerateRandomOnlineWord();
            }
            catch (Exception)
            {
                secretWord = DataService.GenerateRandomOfflineWord();
            }

            textBox.Focus();
            lblHangman.Content = hangmanPicures[hangmanPictureCount];
            lblWord.Content = DisplayHashedWord(secretWord);
        }

        private string secretWord;
        private int hangmanPictureCount = 0;
        private List<char> foundLetters = new() { '=' };
        private List<char> usedLetters = new() { '=' };

        private string[] hangmanPicures = new string[]
        {
            "      #####  \n" +
            "      |       #  \n" +
            "              #  \n" +
            "              #  \n" +
            "              #  \n" +
            "              #  \n" +
            "#########",
            "      #####  \n" +
            "      |       #  \n" +
            "     O      #  \n" +
            "              #  \n" +
            "              #  \n" +
            "              #  \n" +
            "#########",
            "      #####  \n" +
            "      |       #  \n" +
            "     O      #  \n" +
            "      |       #  \n" +
            "              #  \n" +
            "              #  \n" +
            "#########",
            "      #####  \n" +
            "      |       #  \n" +
            "     O      #  \n" +
            "   /  |       #  \n" +
            "              #  \n" +
            "              #  \n" +
            "#########",
            "      #####  \n" +
            "      |       #  \n" +
            "     O      #  \n" +
            "  /   |  \\   #  \n" +
            "              #  \n" +
            "              #  \n" +
            "#########",
            "      #####  \n" +
            "      |       #  \n" +
            "     O      #  \n" +
            "  /   |  \\   #  \n" +
            "    /         #  \n" +
            "              #  \n" +
            "#########",
            "      #####  \n" +
            "      |       #  \n" +
            "     O      #  \n" +
            "  /   |  \\   #  \n" +
            "    /  \\     #  \n" +
            "              #  \n" +
            "#########"
        };

        // Display the word
        private static string DisplayHashedWord(string _word)
        {
            StringBuilder _returnString = new StringBuilder();
            int _spacesCount = _word.Length - 1;

            foreach (char _c in _word)
            {
                _returnString.Append("__");

                if (_spacesCount > 0)
                {
                    _returnString.Append(" ");
                    _spacesCount--;
                }
            }

            return _returnString.ToString();
        }

        // Find the letter
        private string CheckForLetter(char _userChar, string _currentWordStatus, string _secretWord)
        {
            if (!usedLetters.Contains(_userChar))
            {
                usedLetters.Add(_userChar);
                if (_secretWord.Contains(_userChar))
                {
                    StringBuilder _returnString = new StringBuilder();
                    int _spacesCount = _secretWord.Length;
                    bool charFound = false;

                    if (!foundLetters.Contains(_userChar))
                    {
                        foundLetters.Add(_userChar);
                    }

                    foreach (char _c in _secretWord)
                    {
                        charFound = false;
                        foreach (char _fc in foundLetters)
                        {
                            if (_fc == _c)
                            {
                                _returnString.Append(_fc);
                                if (_spacesCount > 0)
                                {
                                    _returnString.Append(" ");
                                    _spacesCount--;
                                }
                                charFound = true;
                                break;
                            }
                        }

                        if (!charFound)
                        {
                            _returnString.Append("__");
                            if (_spacesCount > 0)
                            {
                                _returnString.Append(" ");
                                _spacesCount--;
                            }
                        }
                    }

                    return _returnString.ToString();
                }
                else
                {
                    if (hangmanPictureCount < 5)
                    {
                        hangmanPictureCount++;
                        lblHangman.Content = hangmanPicures[hangmanPictureCount];
                        return _currentWordStatus;
                    }
                    else
                    {
                        hangmanPictureCount++;
                        lblHangman.Content = hangmanPicures[hangmanPictureCount];
                        MessageBox.Show($"You lose... The word was {_secretWord}");
                        textBox.IsEnabled = false;
                        button.IsEnabled = false;
                        return _currentWordStatus;
                    }
                }
            }
            else
            {
                return _currentWordStatus;
            }
        }

        // Check for win
        private void CheckForWin(string _inputString)
        {
            if (!_inputString.Contains("__"))
            {
                MessageBox.Show("You Win!");
                textBox.IsEnabled = false;
                button.IsEnabled = false;
            }
        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonPress(sender, e);
            }

            textBox.Text = string.Empty;
        }

        private void ButtonPress(object sender, EventArgs e)
        {
            textBox.Focus();
            lblWord.Content = CheckForLetter(textBox.Text.ToUpper().ToCharArray()[0], lblWord.Content.ToString(), secretWord.ToUpper());
            CheckForWin(lblWord.Content.ToString());
        }
    }
}
