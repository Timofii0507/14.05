using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _14._05
{
    public partial class MainWindow : Window
    {
        private GameDbContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new GameDbContext();
            _context.Database.Migrate(); // Ensure database is created and migrations are applied
            LoadGames();
        }

        private void AddGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var existingGame = _context.Games
                    .FirstOrDefault(g => g.Title == TitleTextBox.Text && g.Developer == DeveloperTextBox.Text);

                if (existingGame == null)
                {
                    var game = new Game
                    {
                        Title = TitleTextBox.Text,
                        Developer = DeveloperTextBox.Text,
                        Genre = GenreTextBox.Text,
                        ReleaseDate = ReleaseDatePicker.SelectedDate ?? DateTime.Now,
                        GameMode = GameModeTextBox.Text,
                        CopiesSold = int.Parse(CopiesSoldTextBox.Text)
                    };

                    _context.Games.Add(game);
                    _context.SaveChanges();
                    LoadGames();
                }
                else
                {
                    MessageBox.Show("Game already exists.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding game: {ex.Message}");
            }
        }

        private void LoadGames()
        {
            GamesListBox.ItemsSource = _context.Games.ToList();
        }

        private void SearchByTitleButton_Click(object sender, RoutedEventArgs e)
        {
            GamesListBox.ItemsSource = _context.Games
                .Where(g => g.Title.Contains(SearchTextBox.Text))
                .ToList();
        }

        private void SearchByDeveloperButton_Click(object sender, RoutedEventArgs e)
        {
            GamesListBox.ItemsSource = _context.Games
                .Where(g => g.Developer.Contains(SearchTextBox.Text))
                .ToList();
        }

        private void SearchByTitleAndDeveloperButton_Click(object sender, RoutedEventArgs e)
        {
            var terms = SearchTextBox.Text.Split(new[] { ',' }, 2);
            if (terms.Length == 2)
            {
                var title = terms[0].Trim();
                var developer = terms[1].Trim();
                GamesListBox.ItemsSource = _context.Games
                    .Where(g => g.Title.Contains(title) && g.Developer.Contains(developer))
                    .ToList();
            }
        }

        private void SearchByGenreButton_Click(object sender, RoutedEventArgs e)
        {
            GamesListBox.ItemsSource = _context.Games
                .Where(g => g.Genre.Contains(SearchTextBox.Text))
                .ToList();
        }

        private void SearchByReleaseYearButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SearchTextBox.Text, out int year))
            {
                GamesListBox.ItemsSource = _context.Games
                    .Where(g => g.ReleaseDate.Year == year)
                    .ToList();
            }
        }

        private void SinglePlayerGamesButton_Click(object sender, RoutedEventArgs e)
        {
            GamesListBox.ItemsSource = _context.Games
                .Where(g => g.GameMode == "Single Player")
                .ToList();
        }

        private void MultiplayerGamesButton_Click(object sender, RoutedEventArgs e)
        {
            GamesListBox.ItemsSource = _context.Games
                .Where(g => g.GameMode == "Multiplayer")
                .ToList();
        }

        private void MaxCopiesSoldGameButton_Click(object sender, RoutedEventArgs e)
        {
            var game = _context.Games
                .OrderByDescending(g => g.CopiesSold)
                .FirstOrDefault();
            if (game != null)
            {
                GamesListBox.ItemsSource = new[] { game };
            }
        }

        private void MinCopiesSoldGameButton_Click(object sender, RoutedEventArgs e)
        {
            var game = _context.Games
                .OrderBy(g => g.CopiesSold)
                .FirstOrDefault();
            if (game != null)
            {
                GamesListBox.ItemsSource = new[] { game };
            }
        }

        private void Top3PopularGamesButton_Click(object sender, RoutedEventArgs e)
        {
            GamesListBox.ItemsSource = _context.Games
                .OrderByDescending(g => g.CopiesSold)
                .Take(3)
                .ToList();
        }

        private void Top3UnpopularGamesButton_Click(object sender, RoutedEventArgs e)
        {
            GamesListBox.ItemsSource = _context.Games
                .OrderBy(g => g.CopiesSold)
                .Take(3)
                .ToList();
        }
    }
}