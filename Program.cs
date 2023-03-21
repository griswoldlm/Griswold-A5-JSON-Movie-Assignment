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

            
             

            MovieInformation minfo = new MovieInformation();
            minfo.MovieIDs = "0002";
            minfo.MovieTitles = "Secret Life of Pets 2";
            minfo.MovieGenres = "Animation, Comedy";

            string jsonFile = JsonConvert.SerializeObject(minfo);

            // path to movie data file
            string file = $"{Environment.CurrentDirectory}/movies.json";

            using (var sw = new StreamWriter(file, true))
            {
                sw.WriteLine(jsonFile.ToString());
            }

            // Read and deserialize
            string json = @"{ 'MovieID': '0002', 'MovieTitle': 'Secret Life of Pets 2', 'MovieGenres': 'Animation, Comedy' }";

            MovieInformation m = JsonConvert.DeserializeObject<MovieInformation>(json);

            string movie = m.MovieTitles;


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
                        List<string> MovieIDs = new List<string>();
                        List<string> MovieTitles = new List<string>();
                        List<string> MovieGenres = new List<string>();

                        // Read from movies.csv
                        StreamReader sr = new StreamReader(file);


                        // Remove headers when option 2 is choosen
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();

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
                                string movieId = MovieIDs.Max() + 1;

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

                                // Display all movie info when addition's complete
                                Console.WriteLine($"{movieId}, {movieTitle}, {genresString}");

                                //// Create file from data and follow csv requirements
                                //StreamWriter sw = new StreamWriter(file, true);


                                //sw.WriteLine($"{movieId}, {movieTitle}, {genresString}");

                                //// ALWAYS CLOSE!!!
                                //sw.Close();

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
                                Console.WriteLine($"MovieID: {arr[0]}, MovieTitle: {arr[1]}, MovieGenres: {arr[2]}");
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