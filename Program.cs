using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System.IO;
using Griswold_A5_JSON_Movie_Assignment;

namespace Griswold_A4_Movie_Library_Assignment 
{
    public class Program 
    {
        static void Main(string[] args)
        {

            // path to movie data file
             

            MovieInformation minfo = new MovieInformation();
            minfo.MovieIDs = "0001";
            minfo.MovieTitles = "Secret Life of Pets";
            minfo.MovieGenres = "Comedy";

            string jsonFile = JsonConvert.SerializeObject(minfo);

            string file = $"{Environment.CurrentDirectory}/movies.json";

            using (var sw = new StreamWriter(file, true))
            {
                sw.WriteLine(jsonFile.ToString());
            }

            // Read and deserialize
            string json = @"{
            'MovieID:' '0001',
            'MovieTitle:' 'Secret Life of Pets',
            'MovieGenres:' ['Comedy']
            }";

            MovieInformation m = JsonConvert.DeserializeObject<MovieInformation>(json);

            string movie = m.MovieTitles;





            //List<Program> _movie = new List<Program>();

            //_movie.Add(new Program());



            // Make sure file exists
            if (!File.Exists(file))
                {
                    {
                        Console.WriteLine("File does not exist. \nFile has been created.");

                        // Create file
                        using (var sw = new StreamWriter(file, true))
                        {
                            sw.WriteLine(jsonFile.ToString());
                            sw.Close();
                        }
                }

                }
                else
                {
                    string choice;
                    do
                    {
                        // Choices for user to choose
                        Console.WriteLine("1. Add Movie(s)");
                        Console.WriteLine("2. Display Movies");
                        Console.WriteLine("3. Exit");

                        // Receive user input
                        choice = Console.ReadLine();

                        // Lists to be populated with new movies -- eventually turn into classes, methods, etc.
                        List<UInt64> MovieIDs = new List<UInt64>();
                        List<string> MovieTitles = new List<string>();
                        List<string> MovieGenres = new List<string>();

                        // Read from movies.csv
                        StreamReader sr = new StreamReader(file);


                        // Remove headers when option 2 is choosen
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

                            // Look for quotes that surround commas in movie titles -- use IndexOf
                            int idx = line.IndexOf('"');

                            // No quote no comma
                            if (idx == -1)
                            {
                                // Split with commma, make as arrays like ticketing program
                                string[] movieDetails = line.Split(',');

                                // Array 0 - movie id
                                MovieIDs.Add(UInt64.Parse(movieDetails[0]));

                                // Array 1 - movie title
                                MovieTitles.Add(movieDetails[1]);

                                // Array 2 - movie genre(s) -- need to replace | w/ comma(s) b/c csv file
                                MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                            }
                            // Contains quotes and commas -- still need to replace | w/ comma(s) b/c csv file
                            else
                            { 
                                // Find movieID 
                                MovieIDs.Add(UInt64.Parse(line.Substring(0, idx - 1)));

                                // Remove movieID and find first quote
                                line = line.Substring(idx + 1);

                                // Find next quote
                                idx = line.IndexOf('"');

                                // Find whole movie title
                                MovieTitles.Add(line.Substring(0, idx));

                                // Remove whole movie title and last comma
                                line = line.Substring(idx + 2);

                                // Replace all | w/ commas in genres b/c csv file
                                MovieGenres.Add(line.Replace("|", ", "));
                            }
                        }
                        // Always close!!
                        sr.Close();

                        // Ask for movie info to add movie to csv
                        if (choice == "1")
                        {
                            // Ask for movie title
                            Console.WriteLine("What is the title of the movie you would like to add?");

                            // Input title
                            string movieTitle = Console.ReadLine();

                            // Check for duplicates
                            List<string> LowerCaseMovieTitles = MovieTitles.ConvertAll(t => t.ToLower());
                            if (LowerCaseMovieTitles.Contains(movieTitle.ToLower()))
                            {
                                Console.WriteLine("A movie with this title already exists.");
                            }
                            else
                            {
                                // New movie id - use max value + 1 -- similiar to sql window function LAST_VALUE
                                UInt64 movieId = MovieIDs.Max() + 1;

                                // Ask for movie genre(s)
                                List<string> genres = new List<string>();

                                string genre;

                                do
                                {
                                    // Ask for genre(s)
                                    Console.WriteLine("What genre(s) does this movie have? Or 'quit' to complete movie genre(s)");

                                    // Input genre
                                    genre = Console.ReadLine();

                                    // If user enters "quit" or doesn't add genre
                                    if (genre != "quit" && genre.Length > 0)
                                    {
                                        genres.Add(genre);
                                    }

                                } while (genre != "quit");

                                // ZERO added
                                if (genres.Count == 0)
                                {
                                    genres.Add("none");
                                }

                                // Display genre(s) with | as delimiter
                                string genresString = string.Join("|", genres);

                                // Put quotes around commma(s) in movie title
                                movieTitle = movieTitle.IndexOf(',') != -1 ? $"\"{movieTitle}\"" : movieTitle;

                                // Display all movie info when addition's complete
                                Console.WriteLine($"{movieId}, {movieTitle}, {genresString}");

                                // Create file from data and follow csv requirements
                                StreamWriter sw = new StreamWriter(file, true);

                                // NOT WORKING - Says /movies.csv is already in use
                                //List<Program> _data = new List<Program>();

                                //_data.Add(new Program());
                                //using (StreamWriter file1 = File.CreateText($"{Environment.CurrentDirectory}/movies.csv"))
                                //{
                                //    JsonSerializer serializer = new JsonSerializer();

                                //    // Serialize object directly into file stream
                                //    serializer.Serialize(file1, _data);
                                //}

                                sw.WriteLine($"{movieId}, {movieTitle}, {genresString}");

                                // ALWAYS CLOSE!!!
                                sw.Close();

                                // Add movie info to list
                                MovieIDs.Add(movieId);
                                MovieTitles.Add(movieTitle);
                                MovieGenres.Add(genresString);
                            }
                        }
                        // Display Movies
                        else if (choice == "2")
                        {
                            StreamReader sr1 = new StreamReader(file);

                            // Skip header
                            sr1.ReadLine();

                            // Read movie.csv file
                            while (!sr1.EndOfStream)
                            {
                                var line = sr1.ReadLine();
                                string[] arr = line.Split(',');
                                Console.WriteLine($"ID: {arr[0]}, Title: {arr[1]}, Genre(s): {arr[2]}");
                            }

                        }
                        sr.Close(); // Always close!!!

                        // Exit program
                        if (choice == "3")
                        {
                            // Leave Program
                            Console.WriteLine("Closing program.");
                            break;
                        }

                    } while (choice == "1" || choice == "2");
                }
        }
    }
}