using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string Filename = "";
        private static TimeSpan CurrentPositionMiliseconds = TimeSpan.FromMilliseconds(0);
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            videoSlider.Value = mediaElement.Position.TotalSeconds;
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsVideoSelected())
            {
                MessageBox.Show("Выберите файл");
                return;
            }
            if (CurrentPositionMiliseconds.TotalMilliseconds > 0)
            {
                mediaElement.Play();
                mediaElement.Position = CurrentPositionMiliseconds;
            } else
            {
                mediaElement.Play();
            }
        }

        private void Init()
        {
            mediaElement.Source = new Uri(Filename);
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            mediaElement.Play();
            mediaElement.Pause();
            mediaElement.Position = TimeSpan.FromMilliseconds(1);
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsVideoSelected())
            {
                CurrentPositionMiliseconds = mediaElement.Position;
                mediaElement.Pause();
            }
        }

        private void videoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(videoSlider.Value);
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
            mediaElement.Source = new Uri(filename);
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            mediaElement.Play();
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = mediaElement.NaturalDuration.TimeSpan;
            videoSlider.Maximum = ts.TotalSeconds;
            timer.Start();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mediaElement.Position = TimeSpan.Parse(listBox.SelectedItem.ToString());
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                listBox.Items.Clear();
                Filename = openFileDialog.FileName;
                Init();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsVideoSelected())
            {
                MessageBox.Show("Выберите файл");
                return;
            }
            listBox.Items.Add(mediaElement.Position);
        }

        private bool IsVideoSelected()
        {
            return Filename.Length != 0;
        }
    }
}
