using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MusicPlaylistAnalyzer
{
    class Song
    {
        public string Name;
        public string Artist;
        public string Album;
        public string Genre;
        public int Size;
        public int Time;
        public int Year;
        public int Plays;

        public Song(string Name, string Artist, string Album, string Genre, int Size, int Time, int Year, int Plays)
        {
            this.Name = Name;
            this.Artist = Artist;
            this.Album = Album;
            this.Genre = Genre;
            this.Size = Size;
            this.Time = Time;
            this.Year = Year;
            this.Plays = Plays;
        }

        override public string ToString()
        {
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", 
                Name, Artist, Album, Genre, Size, Time, Year, Plays);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string report = null;
            int i;

            List<Song> SongList = new List<Song>();

            try
            {
                if (File.Exists("SampleMusicPlaylist.txt"))
                {
                    StreamReader sr = new StreamReader("SampleMusicPlaylist.txt");
                    i = 0;
                    string line = sr.ReadLine();

                    while ((line = sr.ReadLine()) != null)
                    {
                        i++;

                        try
                        {
                            string[] col = line.Split('\t');

                            if (col.Length > 9)
                            {
                                Console.WriteLine("Invalid number of columns.");

                                break;
                            }
                            else
                            {
                                Song data = new Song(col[0], col[1], col[2], col[3], int.Parse(col[4]), int.Parse(col[5]),
                                    int.Parse(col[6]), int.Parse(col[7]));

                                SongList.Add(data);
                            }
                        }
                        catch
                        {
                            Console.Write("Invalid.");

                            break;
                        }
                    }
                    sr.Close();
                }
                else
                {
                    Console.WriteLine("Could not find file.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("File could not be opened.");
            }

            try
            {
                Song[] songs = SongList.ToArray();

                using (StreamWriter write = new StreamWriter("Report.txt"))
                {
                    write.WriteLine("Music Playlist Report");
                    write.WriteLine("");

                    var plays = from song in songs where song.Plays >= 200 select song;
                    report += "Songs that received 200 or more plays:\n";
                    foreach (Song song in plays)
                    {
                        report += song + "\n";
                    }
                    
                    var alt = from song in songs where song.Genre == "Alternative" select song;
                    i = 0;
                    foreach (Song song in alt)
                    {
                        i++;
                    }
                    report += "Number of Alternative songs: {i} \n";

                    var rap = from song in songs where song.Genre == "Hip-Hop/Rap" select song;
                    i = 0;
                    foreach (Song song in rap)
                    {
                        i++;
                    }
                    report += "Number of Hip-Hop/Rap songs: {i}\n";

                    var fish = from song in songs where song.Album == "Welcome to the Fishbowl" select song;
                    report += "Songs from the album Welcome to the Fishbowl:\n";

                    foreach (Song song in fish)
                    {
                        report += song + "\n";
                    }

                    var songs70s = from song in songs where song.Year < 1970 select song;
                    report += "Songs from before 1970:\n";

                    foreach (Song song in songs70s)
                    {
                        report += song + "\n";
                    }

                    var names = from song in songs where song.Name.Length > 85 select song.Name;
                    report += "Song names longer than 85 characters:\n";

                    foreach (string name in names)
                    {
                        report += name + "\n";
                    }

                    var length = from song in songs orderby song.Time descending select song;
                    report += "Longest song:\n";
                    report += length.First();

                    write.Write(report);
                    write.Close();
                }
            }

            catch (Exception)
            {
                Console.WriteLine("Error: Cannot open or write report.");
            }

            Console.ReadLine();
        }
    }
}
