using System.Runtime.ConstrainedExecution;
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
using System.Windows.Threading;

namespace autoVerseny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    class Auto
    {
        public Brush color;
        public string name;
        public int speed;

        public Auto(Brush color, string name, int speed)
        {
            this.color = color;
            this.name = name;
            this.speed = speed;
        }
    }
    class Player
    {
        public Auto auto;
        public Button btn;

        public Player(Auto auto, Button btn)
        {
            this.auto = auto;
            this.btn = btn;
        }
    }
    public partial class MainWindow : Window
    {
        int wins = 0;
        List<Auto> autok =new List<Auto>{
            new Auto(Brushes.White, "Céges Skoda", 5),
            new Auto(Brushes.Green, "Zöld Opel Corsa", 5),
            new Auto(Brushes.Gray, "VW Passat 1.9 TDI", 6),
            new Auto(Brushes.Red, "Lyukas kipufogós Suzuki", 7),
            new Auto(Brushes.Purple, "Lila Audi Quattro", 7),
            new Auto(Brushes.Blue, "Kék Ford Mustang", 8),
            new Auto(Brushes.Orange, "Narancssárga Toyota Supra", 8),
            new Auto(Brushes.Black, "Fekete BMW M3", 9),
            new Auto(Brushes.Yellow, "Sárga Lamborghini", 10)
        };
        DispatcherTimer timer;
        Random rnd=new Random();
        int nitroCount = 0;
        private double celvonal = 650;
        Player player;
        List<Player> enemies = new List<Player>();
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval=TimeSpan.FromMilliseconds(20);
            timer.Tick += timer_Tick;

        }

        private void generate()
        {
            Auto car=autok[wins];
            Button btn = new Button
            {
                Width = 90,
                Height = 45,
                Content = car.name,
                Background = car.color,
            };
            btn.Click += jatekos_Click;
            palya.Children.Add(btn);
            Canvas.SetLeft(btn, 20);
            Canvas.SetTop(btn, 50);
            player=new Player(car,btn);
            for (int i = 0; i < 3; i++)
            {
                car = autok[rnd.Next(0, wins+1)];
                btn = new Button
                {
                    Width = 90,
                    Height = 45,
                    Content = car.name,
                    Background = car.color
                };
                palya.Children.Add(btn);
                Canvas.SetLeft(btn, 20);
                Canvas.SetTop(btn, 50 + 50 * (i + 1));
                enemies.Add(new Player(car, btn));

            }
        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            if (nitroCount < 4)
            {
                Mozog(player);

            }
            foreach (Player enemy in enemies)
            {
                Mozog(enemy, false, true);
            }
        }



        private void Mozog(Player player, bool nitro=false,bool enemy=false)
        {
            double aktualisPozicio = Canvas.GetLeft(player.btn);
            double sebesseg =enemy? player.auto.speed * 1.1 : player.auto.speed;
            sebesseg =nitro? player.auto.speed*10:sebesseg;
            double ujpozicio = aktualisPozicio + sebesseg;
            Canvas.SetLeft(player.btn, ujpozicio);
            if(ujpozicio >= celvonal)
            {
                for (int i = 0; i < 4; i++)
                {
                    timer.Stop();
                }
                MessageBox.Show($"A(z) {player.auto.name} nyert");
                if (!enemy)
                {
                    wins++;
                    if (wins <= autok.Count-1)
                    {
                    player.auto = autok[wins];
                    }
                }
                palya.Children.Clear();
                
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            generate();
            nitroCount = 0;
            timer.Start();
        }

        private void jatekos_Click(object sender, RoutedEventArgs e)
        {
            if (nitroCount < 4)
            {
            Mozog(player, true);
            nitroCount++;
            }
            else
            {
                MessageBox.Show("Túl sokat nitróztál, ezért lerobbantál");
            }

        }
    }
}